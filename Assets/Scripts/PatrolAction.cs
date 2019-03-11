using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "EnemyAI/Actions/Patrol")]
public class PatrolAction : Action
{
    public override void Act(EnemyController controller)
    {
        Patrol(controller);
    }

    private void Patrol(EnemyController controller)
    {

        RaycastHit hit;


        Debug.DrawRay(controller.lineOfSight.position, controller.lineOfSight.forward.normalized * 12, Color.red);

        if (Physics.SphereCast(controller.lineOfSight.position, 6, controller.lineOfSight.forward, out hit, 12)
            && hit.collider.CompareTag("Player"))
        {
            controller.agent.SetDestination(controller.chasePlayer.position);
        }
        else
        {
            controller.agent.destination = controller.patrolPoints[controller.nextPatrolPoint].position;


            if (controller.agent.remainingDistance <= controller.agent.stoppingDistance && !controller.agent.pathPending)
            {
                controller.nextPatrolPoint = (controller.nextPatrolPoint + 1) % controller.patrolPoints.Length;
            }
        }


    }
}


