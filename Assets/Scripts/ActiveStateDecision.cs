using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "EnemyAI/Decisions/ActiveState")]
public class ActiveStateDecision : Decision
{
    public override bool Decide(EnemyController controller)
    {
        bool chaseTarget = controller.chasePlayer.gameObject.activeSelf;
        return chaseTarget;
    }
}
