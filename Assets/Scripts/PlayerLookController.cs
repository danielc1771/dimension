using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookController : MonoBehaviour
{

    public float yLowClampAngle = -30.0f;
    public float yClampRange = 60.0f;
    public float lookSensitivity = 30.0f;
    public float smoothingWeight = 3.0f;

    private GameObject playerGameObject;
    private Vector2 smoothMouse;
    private bool cursorIsVisible = false;
    private Vector2 mouseAbsolute;
    private Rigidbody playerRigidBody;

    void Start()
    {
        playerGameObject = this.transform.parent.gameObject;
        playerRigidBody = playerGameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        lookHandler();
    }

    void lookHandler()
    {
        Vector2 mouseDelta = Vector2.Scale(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")), 
            new Vector2(lookSensitivity, lookSensitivity));

        smoothMouse.x = Mathf.Lerp(smoothMouse.x, mouseDelta.x, 1f / smoothingWeight);
        smoothMouse.y = Mathf.Lerp(smoothMouse.y, mouseDelta.y, 1f / smoothingWeight);
        mouseAbsolute += smoothMouse;
        mouseAbsolute.y = Mathf.Clamp(mouseAbsolute.y, yLowClampAngle, yLowClampAngle + yClampRange);

        transform.localRotation = Quaternion.AngleAxis(-mouseAbsolute.y, Vector3.right);
        playerRigidBody.rotation = Quaternion.Euler(playerRigidBody.rotation.eulerAngles + new Vector3(0f, lookSensitivity * Input.GetAxis("Mouse X"), 0f));
        playerRigidBody.MoveRotation(Quaternion.Euler(playerRigidBody.rotation.eulerAngles + new Vector3(0f, lookSensitivity * Input.GetAxis("Mouse X"), 0f)));
    }

    private void fixedUpdate()
    {
        Cursor.visible = cursorIsVisible;
        Cursor.lockState = !cursorIsVisible ? CursorLockMode.Locked : CursorLockMode.None;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorIsVisible = !cursorIsVisible;
        }
    }

}