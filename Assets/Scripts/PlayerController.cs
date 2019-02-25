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

    private int whichHand = 0;
    private bool playerHasJumped = false;
    private bool jumpedRightWall = false;
    private bool jumpedLeftWall = false;
    private bool doubleJumpAvailable = false;
    private bool kickKeyPressed = false;
    private bool punchKeyPressed = false;
    private bool punchingLeft = false;
    private bool punchingRight = true;

    private Animator animator;
    private bool walking = false;
    private bool running = false;
    private bool jumping = false;

    LayerMask leftWall = LayerMask.GetMask("LeftWall");
    LayerMask rightWall = LayerMask.GetMask("RightWall");

    void Start()
    {
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
        animator.SetBool("PunchingRight", punchingRight && punchKeyPressed);
        animator.SetBool("PunchingLeft", punchingLeft && punchKeyPressed);
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
            bool touchingWallLeft = (Physics.Raycast(transform.position, Vector3.left, wallTouchRadius, leftWall));
            bool touchingWallRight = (Physics.Raycast(transform.position, Vector3.right, wallTouchRadius, rightWall));

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
                        playerRigidbody.velocity = new Vector3(wallJumpHorizontalSpeed, wallJumpVerticalSpeed, 0f);
                        audioGenerator.PlaySound(PlayerAudioIndex.JUMP);
                    }
                    if (touchingWallRight && !jumpedRightWall)
                    {
                        doubleJumpAvailable = false;
                        playerHasJumped = true;
                        jumpedRightWall = true;
                        jumpedLeftWall = false;
                        playerRigidbody.velocity = new Vector3(wallJumpHorizontalSpeed, wallJumpVerticalSpeed, 0f);
                        audioGenerator.PlaySound(PlayerAudioIndex.JUMP);
                    }
                    if(doubleJumpAvailable)
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
            if (!punchKeyPressed)
            {
                punchKeyPressed = true;
                audioGenerator.PlaySound(PlayerAudioIndex.PUNCH);
                punchingLeft = !punchingLeft;
                punchingRight = !punchingRight;
            }
            else
            {
                punchKeyPressed = false;
            }
            //StartCoroutine(PunchAnimation());
        }
        else {
            punchKeyPressed = false;
        }
        if (KickKeyPressed())
        {
            //StartCoroutine(KickAnimation());
        }
        return;
    }

    IEnumerator PunchAnimation()
    {
        //updates the punchKeyPressed value twice so when unity detects a collision with the enemy, it checks to see if the attack button has been pressed
        //punchKeyPressed = true;



        //plays the punch sound
        audioGenerator.PlaySound(PlayerAudioIndex.PUNCH);

        //this waits for 1 second before changing the value of punchKeyPressed. a more accurate name for punchKeyPressed would be canPunch or runningPunchAnimation
        //NOTE the 1 second wait time should be changed to reflect the duration of the punching animation
        yield return new WaitForSeconds(1);

        //punchKeyPressed = false;
        //animator.SetBool("PunchingLeft", punchKeyPressed);
    }

    //same as punch animation method above
    IEnumerator KickAnimation()
    {
        kickKeyPressed = true;
        //call animation
        //play sound
        yield return new WaitForSeconds(0.1f);
        kickKeyPressed = false;
    }

    //Detects collision with enemy
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //makes sure animation is running/punch key has been pressed. if this check it not here, the player can walk into the enemy and damage it
            if (punchKeyPressed)
            {
                GameObject enemy = collision.gameObject;
                EnemyAI temp = (EnemyAI)enemy.GetComponent(typeof(EnemyAI));

                //DamageEnemy takes in a force vector. FIXME these values are just for testing. might want to lower them
                temp.DamageEnemy(new Vector3(10000f, 10000f, 10000f), collision.gameObject.GetComponent<Rigidbody>());
            }
        }
        else return;
    }

    private bool JumpKeyPressed()
    {
        return Input.GetAxis("Jump") > 0f;
    }

    private bool PunchKeyPressed()
    {
        return Input.GetAxis("Punch") > 0f;
    }

    private bool KickKeyPressed()
    {
        return Input.GetAxis("Kick") > 0f;
    }

    private bool IsGrounded()
    {
        float distance = playerCollider.bounds.extents.y + 0.1f;
        //if (Physics.Raycast(transform.position, -Vector3.up, distance, leftWall) || Physics.Raycast(transform.position, -Vector3.up, distance, rightWall))
        // return false;
        return Physics.Raycast(transform.position, -Vector3.up, distance);
    }
}