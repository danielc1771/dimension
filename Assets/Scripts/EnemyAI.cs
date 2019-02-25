using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    Transform player;

    public Transform[] patrolPoints;
    float minDist = 8;
    float distToPlayer;
    public int speed = 2;
    int currentPoint = 0;
    public bool playerSpotted = false;

    int maxHealth = 2;
    int currentHealth = 2;


    void Start()
    {
        //Find where Player is
        player = GameObject.FindWithTag("Player").transform;
 
        //agent = GetComponent<NavMeshAgent>();
       
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[currentPoint].position, speed * Time.deltaTime);
        distToPlayer = Vector3.Distance(transform.position, player.position);

        if (distToPlayer <= minDist)
        {
            playerSpotted = true;
            transform.LookAt(player);
        }

        if ((Vector3.Distance(transform.position, patrolPoints[currentPoint].position) < 0.2) && !playerSpotted)
        {
            GoToNextPoint();
        }

        if(playerSpotted)
        {
            if(player.position.y < transform.position.y)
            {
               playerSpotted = false;
            }
            transform.position = Vector3.MoveTowards(transform.position, player.position, (speed) * Time.deltaTime);
        }
    }

    void GoToNextPoint()
    {
        currentPoint++;
        if (currentPoint > patrolPoints.Length-1)
        {
            currentPoint = 0;
        }
    }

    // Damages/Knocks back the enemy
    public void DamageEnemy(Vector3 force, Rigidbody enemy)
    {
        //enemy has 2 hit points
        currentHealth--;

        //destroy the enemy game object
        if (currentHealth <= 0)
        {
            Destroy(gameObject, 0);
        }
        //if the enemy is still alive, knock back
        else
        {
            enemy.AddForce(force);
        }

    }
}