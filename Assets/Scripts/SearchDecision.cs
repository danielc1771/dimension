using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "EnemyAI/Decisions/Search")]
public class SearchDecision : Decision
{
    public override bool Decide(EnemyController controller)
    {
        bool playerVisible = Search(controller);
        return playerVisible;
    }

    private bool Search(EnemyController controller)
    {
        RaycastHit hit;

        Debug.DrawRay(controller.lineOfSight.position, controller.lineOfSight.forward.normalized * 8, Color.green);

        if(Physics.SphereCast(controller.lineOfSight.position, 3, controller.lineOfSight.forward, out hit, 8) 
            && hit.collider.CompareTag("Player"))
        {
            controller.chasePlayer = hit.transform;
            return true;
        }
        else
        {
            return false;
        }

    }
}
