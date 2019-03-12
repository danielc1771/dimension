using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public bool beenPunched = false;
    public bool patrolOnly = false;

    public State currentState;
    public Transform[] patrolPoints;
    public NavMeshAgent agent;
    public Transform lineOfSight;
    public State remainState;
    public GameObject projectile;

    [HideInInspector] public int nextPatrolPoint;
    [HideInInspector] public Transform chasePlayer;
    [HideInInspector] public float stateTimeElapsed = 0;

    void Awake()
    {
        chasePlayer = patrolPoints[0];
        agent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
       // if (!patrolOnly)
        currentState.UpdateState(this);
    }

    void OnDrawGizmos()
    {
        if(currentState != null && lineOfSight != null)
        {
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(lineOfSight.position, 2);
        }
    }

    public void TransitionToState(State nextState)
    {
        if(nextState != remainState && !patrolOnly)
        {
            currentState = nextState;
        }
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    private void OnExitState()
    {
        stateTimeElapsed = 0;
    }

    public void DamageEnemy(bool punch, Rigidbody enemy)
    {
        if (beenPunched || !punch)
        {
            Destroy(gameObject, 0);
        }
        else
        {
            beenPunched = true;
            //enemy.AddForce(force);
        }
    }

}
