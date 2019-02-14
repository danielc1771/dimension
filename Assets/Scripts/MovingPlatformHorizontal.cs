using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformHorizontal : MonoBehaviour
{
    public bool moveInZ = true;
    public bool moveInX = false;
    public float translationTime = 5f;
    public float speed = 1f;

    Rigidbody movingPlatformRigidBody;
    Rigidbody playerRigidBody;
    int movementDirection = 0;

    void Start()
    {
        movingPlatformRigidBody = GetComponent<Rigidbody>();
        playerRigidBody = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
        AdjustConstraints();
        InvokeRepeating("ChangeVelocity", 1f, translationTime);

    }

    void AdjustConstraints()
    {
        if(moveInX && moveInZ)
        {
            movingPlatformRigidBody.constraints = RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY
            | RigidbodyConstraints.FreezePositionY;
        } else if(!moveInZ && moveInX)
        {
            movingPlatformRigidBody.constraints = RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY
            | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        } else if (moveInZ && !moveInX)
        {
            movingPlatformRigidBody.constraints = RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY
            | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX;
        }
        else
        {
            movingPlatformRigidBody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    void ChangeVelocity()
    {
        movingPlatformRigidBody.velocity = GetMovement();
    }

    private Vector3 GetMovement() {
        float zMovement = moveInZ ? 1f * speed : 0f;
        float xMovement = moveInX ? 1f * speed : 0f;
        return movementDirection++ % 2 == 0 ? new Vector3(xMovement, 0f, zMovement) : 
            new Vector3(-xMovement, 0f, -zMovement);
    }

    void Update()
    {
        if (Physics.Raycast(transform.position, new Vector3(0, 1, 0), 0.5f))
        {
            playerRigidBody.velocity = new Vector3(0f, playerRigidBody.velocity.y, 0f);
            playerRigidBody.velocity += movingPlatformRigidBody.velocity;
        }
    }
}
