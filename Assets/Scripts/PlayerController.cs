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

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        hud.Refresh();
    }

    void Update()
    {
        WalkHandler();
        JumpHandler();
    }

    void WalkHandler() 
    {
        float distance = getWalkHandlerDistance();
        Vector3 movement = getWalkHandlerMovement(distance);
        Vector3 currentPosition = transform.position;
        Vector3 newPosition = currentPosition + movement;
        playerRigidbody.MovePosition(newPosition);
    }

    private float getWalkHandlerDistance() {
        float distance = walkSpeed * Time.deltaTime;
        return IsGrounded() && Input.GetButton("Run") ? distance *= runSpeedMultiplier : distance;
    }

    private Vector3 getWalkHandlerMovement(float distance) {
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