using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "EnemyAI/Actions/Chase")]

public class ChaseAction : Action
{
    public override void Act(EnemyController controller)
    {
        Chase(controller);
    }

    public void Chase(EnemyController controller)
    {
      
        controller.agent.SetDestination(controller.chasePlayer.position);
    }
}
