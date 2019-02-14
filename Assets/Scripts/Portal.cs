using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    public float transportHorizontalPadding = 1f;
    public float transportVerticalPadding = 1f;

    private static readonly int PORTAL_A_INDEX = 0;
    private static readonly int PORTAL_B_INDEX = 1;

    private Rigidbody playerRigidBody;
    private GameObject oppositePortal;

    void Start()
    {
        playerRigidBody = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
        InitOppositePortal();
    }


    private void InitOppositePortal()
    {
        switch (transform.gameObject.tag)
        {
            case "Portal A":
                oppositePortal = GetPortal(PORTAL_B_INDEX);
                break;
            case "Portal B":
                oppositePortal = GetPortal(PORTAL_A_INDEX);
                break;
        }
    }

    private GameObject GetPortal(int portalIndex)
    {
        return transform.parent.gameObject.transform.parent.GetChild(portalIndex).gameObject;
    }

    void Update()
    {
    
    }

    private void OnTriggerEnter(Collider other)
    {
        playerRigidBody.transform.position = GetOppositePortalPosition();
    }


    private Vector3 GetOppositePortalPosition()
    {
        return oppositePortal.transform.position + new Vector3(0f, transportVerticalPadding, 
            transportHorizontalPadding);
    }
}
