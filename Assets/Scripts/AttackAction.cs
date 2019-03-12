using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "EnemyAI/Actions/Attack")]
public class AttackAction : Action
{
    public override void Act(EnemyController controller)
    {
        Attack(controller);
    }

    private void Attack(EnemyController controller)
    {
        RaycastHit hit;
        GameObject proj;
     //   NavMeshAgent nav;

        Debug.DrawRay(controller.lineOfSight.position, controller.lineOfSight.forward.normalized * 12, Color.red);

        if (Physics.SphereCast(controller.lineOfSight.position, 6 , controller.lineOfSight.forward, out hit, 12)
            && hit.collider.CompareTag("Player"))
        {
            controller.agent.transform.LookAt(controller.chasePlayer.transform);
            if (controller.CheckIfCountDownElapsed(3))
            {
                proj = Instantiate(controller.projectile, controller.lineOfSight.position, controller.projectile.transform.rotation);
                //nav = proj.GetComponent<NavMeshAgent>();
                //nav.SetDestination(controller.chasePlayer.position);
                Rigidbody rb = proj.GetComponent<Rigidbody>();
                rb.MovePosition(controller.chasePlayer.position);
            }

           
            //rb.velocity = controller.lineOfSight.forward.normalized * 6;
           // proj.transform.position = Vector3.MoveTowards(controller.agent.transform.forward, controller.chasePlayer.position, 5 * Time.deltaTime);
        }


        //controller.agent.destination = controller.patrolPoints[controller.nextPatrolPoint].position;


        //if (controller.agent.remainingDistance <= controller.agent.stoppingDistance && !controller.agent.pathPending)
        //{
        //    controller.nextPatrolPoint = (controller.nextPatrolPoint + 1) % controller.patrolPoints.Length;
        //}
    }
}