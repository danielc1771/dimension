using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 8f;
    public float jumpSpeed = 8f;
    public float wallJumpVerticalSpeed = 8f;
    public float wallJumpHorizontalSpeed = 5f;
    public float runSpeedMultiplier = 1.5f;
    public float wallTouchRadius = 0.6f;


    private HudManager hud = new HudManager();
    private Rigidbody playerRigidbody;
    private Collider playerCollider;
    private bool playerHasJumped = false;
    private bool jumpedRightWall = false;
    private bool jumpedLeftWall = false;
    private bool doubleJumpAvailable = false;
    private Animator animator;
    private bool walking = false;
    private bool running = false;
    private bool jumping = false;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        hud.Refresh();
    }

    void Update()
    {
        WalkHandler();
        JumpHandler();
    
    }

    private void FixedUpdate()
    {
        animator.SetBool("Walking", walking);
        animator.SetBool("Sprinting", running);
        animator.SetBool("Jumping", playerHasJumped);
        animator.SetBool("InAir", !IsGrounded());
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

    private float GetWalkHandlerDistance() {
        float distance = walkSpeed * Time.deltaTime;
        return IsGrounded() && Input.GetButton("Run") ? distance *= runSpeedMultiplier : distance;
    }

    private Vector3 GetWalkHandlerMovement(float distance) {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal") * distance, 0f, Input.GetAxis("Vertical") * distance);
        return transform.TransformDirection(movement);
    }

    void JumpHandler()
    {

        if (JumpKeyPressed())
        {
            bool touchingWallLeft = (Physics.Raycast(transform.position, Vector3.left, wallTouchRadius));
            bool touchingWallRight = (Physics.Raycast(transform.position, Vector3.right, wallTouchRadius));

            if (!playerHasJumped)
            {
                if (IsGrounded())
                {
                    playerHasJumped = true;
                    doubleJumpAvailable = true;
                    jumpedRightWall = false;
                    jumpedLeftWall = false;
                    playerRigidbody.velocity += new Vector3(0f, jumpSpeed, 0f);
                }
                else
                {
                    if (touchingWallLeft)
                    {
                        doubleJumpAvailable = false;
                        playerHasJumped = true;
                        jumpedLeftWall = true;
                        jumpedRightWall = false;
                        playerRigidbody.velocity = new Vector3(wallJumpHorizontalSpeed, wallJumpVerticalSpeed, 0f);
                    }
                    if (touchingWallRight)
                    {
                        doubleJumpAvailable = false;
                        playerHasJumped = true;
                        jumpedRightWall = true;
                        jumpedLeftWall = false;
                        playerRigidbody.velocity = new Vector3(-wallJumpHorizontalSpeed, wallJumpVerticalSpeed, 0f);
                    }
                    if(doubleJumpAvailable)
                    {
                        playerHasJumped = true;
                        doubleJumpAvailable = false;
                        jumpedRightWall = false;
                        jumpedLeftWall = false;
                        playerRigidbody.velocity = new Vector3(0f, jumpSpeed, 0f);
                    }
                }
            }
        }
        else
        {
            playerHasJumped = false;
        }

    }

    private bool JumpKeyPressed()
    {
        return Input.GetAxis("Jump") > 0f;
    }

    private bool IsGrounded()
    {
        float distance = playerCollider.bounds.extents.y + 0.1f;
        return Physics.Raycast(transform.position, -Vector3.up, distance);
    }
}