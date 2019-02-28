using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public float walkSpeed = 8f;
    public float jumpSpeed = 8f;
    public float wallJumpVerticalSpeed = 8f;
    public float wallJumpHorizontalSpeed = 20f;
    public float runSpeedMultiplier = 1.5f;
    public float wallTouchRadius = 0.6f;

    private HudManager hud = new HudManager();
    private PlayerAudioGenerator audioGenerator;

    private Rigidbody playerRigidbody;
    private Collider playerCollider;

    private bool playerHasJumped = false;
    private bool jumpedRightWall = false;
    private bool jumpedLeftWall = false;
    private bool doubleJumpAvailable = false;


    private bool punchingHand = false;
    private bool punchAvailable = true;
    private bool kickAvailable = true;

    private Animator animator;
    private bool walking = false;
    private bool running = false;
    private bool jumping = false;

    LayerMask leftWall;
    LayerMask rightWall;

    void Start()
    {
        leftWall = LayerMask.GetMask("LeftWall");
        rightWall = LayerMask.GetMask("RightWall");
        audioGenerator = GetComponent<PlayerAudioGenerator>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        hud.Refresh();
    }


    void Update()
    {
        WalkHandler();
        JumpHandler();
        AttackHandler();
    }

    private void FixedUpdate()
    {
        animator.SetBool("Walking", walking);
        animator.SetBool("Sprinting", running);
        animator.SetBool("Jumping", playerHasJumped);
        animator.SetBool("InAir", !IsGrounded());
        animator.SetBool("PunchingRight", !punchAvailable && !punchingHand);
        animator.SetBool("PunchingLeft", !punchAvailable && punchingHand);
        animator.SetBool("Kicking", !kickAvailable);
    }


    private void WalkHandler()
    {
        running = Input.GetButton("Run");
        walking = IsHoldingWalking();
        float distance = GetWalkHandlerDistance();
        Vector3 movement = GetWalkHandlerMovement(distance);
        animator.SetFloat("Walk", movement.x + movement.y);
        Vector3 currentPosition = transform.position;
        Vector3 newPosition = currentPosition + movement;
        playerRigidbody.MovePosition(newPosition);
    }

    private bool IsHoldingWalking()
    {
        return Input.GetKey(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.A)
            || Input.GetKey(KeyCode.D);
    }

    private float GetWalkHandlerDistance()
    {
        float distance = walkSpeed * Time.deltaTime;
        return IsGrounded() && Input.GetButton("Run") ? distance *= runSpeedMultiplier : distance;
    }

    private Vector3 GetWalkHandlerMovement(float distance)
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal") * distance, 0f, Input.GetAxis("Vertical") * distance);
        return transform.TransformDirection(movement);
    }

    void JumpHandler()
    {

        if (JumpKeyPressed())
        {
            bool touchingWallLeft = (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), wallTouchRadius, leftWall));
            bool touchingWallRight = (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), wallTouchRadius, rightWall));
            // print("LEFT: " + touchingWallLeft + " RIGHT: " + touchingWallRight);
            print("LEFT: " + Vector3.left + " RIGHT: " + Vector3.right);
            
            if (!playerHasJumped)
            {
                if (IsGrounded())
                {
                    playerHasJumped = true;
                    doubleJumpAvailable = true;
                    jumpedRightWall = false;
                    jumpedLeftWall = false;
                    playerRigidbody.velocity += new Vector3(0f, jumpSpeed, 0f);
                    audioGenerator.PlaySound(PlayerAudioIndex.JUMP);
                }
                else
                {
                    if (touchingWallLeft && !jumpedLeftWall)
                    {
                        doubleJumpAvailable = false;
                        playerHasJumped = true;
                        jumpedLeftWall = true;
                        jumpedRightWall = false;
                        //  playerRigidbody.velocity = new Vector3(-wallJumpHorizontalSpeed, wallJumpVerticalSpeed, 0f);
                        Vector3 rbVelocity = transform.TransformDirection(Vector3.right);
                        rbVelocity *= wallJumpHorizontalSpeed;
                        rbVelocity += new Vector3(0f, wallJumpVerticalSpeed, 0f);
                        playerRigidbody.velocity = rbVelocity;
                        audioGenerator.PlaySound(PlayerAudioIndex.JUMP);
                    }
                    if (touchingWallRight && !jumpedRightWall)
                    {
                        doubleJumpAvailable = false;
                        playerHasJumped = true;
                        jumpedRightWall = true;
                        jumpedLeftWall = false;
                        //playerRigidbody.velocity = new Vector3(wallJumpHorizontalSpeed, wallJumpVerticalSpeed, 0f);
                        Vector3 rbVelocity = transform.TransformDirection(Vector3.left);
                        rbVelocity *= wallJumpHorizontalSpeed;
                        rbVelocity += new Vector3(0f, wallJumpVerticalSpeed, 0f);
                        playerRigidbody.velocity = rbVelocity;
                        audioGenerator.PlaySound(PlayerAudioIndex.JUMP);
                    }
                    if (doubleJumpAvailable)
                    {
                        playerHasJumped = true;
                        doubleJumpAvailable = false;
                        jumpedRightWall = false;
                        jumpedLeftWall = false;
                        playerRigidbody.velocity = new Vector3(0f, jumpSpeed, 0f);
                        audioGenerator.PlaySound(PlayerAudioIndex.JUMP);
                    }
                }
            }
        }
        else
        {
            playerHasJumped = false;
        }

    }

    // If an attack key is pressed, go to the method below
    void AttackHandler()
    {
        if (PunchKeyPressed())
        {
            if (punchAvailable && kickAvailable)
            {
                StartCoroutine(Punch());
            }
        }

        if (KickKeyPressed())
        {
            if (punchAvailable && kickAvailable)
            {
                StartCoroutine(Kick());
            }
        }
    }

    private IEnumerator Punch()
    {
        punchAvailable = false;
        punchingHand = !punchingHand;
        audioGenerator.PlaySound(PlayerAudioIndex.PUNCH);
        yield return new WaitForSeconds(.3f);
        punchAvailable = true;
    }

    public bool IsPunching()
    {
        return !punchAvailable;
    }

    //same as punch animation method above
    IEnumerator Kick()
    {
        kickAvailable = false;
        yield return new WaitForSeconds(.3f);
        kickAvailable = true;
    }

    ////Detects collision with enemy
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        //makes sure animation is running/punch key has been pressed. if this check it not here, the player can walk into the enemy and damage it
    //        if (!punchAvailable)
    //        {
    //            GameObject enemy = collision.gameObject;
    //            EnemyAI temp = (EnemyAI)enemy.GetComponent(typeof(EnemyAI));

    //            //DamageEnemy takes in a force vector. FIXME these values are just for testing. might want to lower them
    //            temp.DamageEnemy(new Vector3(10000f, 10000f, 10000f), collision.gameObject.GetComponent<Rigidbody>());
    //        }
    //    }
    //    else return;
    //}

    private bool JumpKeyPressed()
    {
        return Input.GetAxis("Jump") > 0f;
    }
    private bool PunchKeyPressed()
    {
        //return Input.GetAxis("Punch") > 0f;
        return Input.GetKeyDown(KeyCode.Space);
    }

    private bool KickKeyPressed()
    {
        // return Input.GetAxis("Kick") > 0f;
        return Input.GetKeyDown(KeyCode.Mouse1);
    }

    private bool IsGrounded()
    {
        float distance = playerCollider.bounds.extents.y + 0.1f;
        //if (Physics.Raycast(transform.position, -Vector3.up, distance, leftWall) || Physics.Raycast(transform.position, -Vector3.up, distance, rightWall))
        // return false;
        return Physics.Raycast(transform.position, -Vector3.up, distance);
    }
}