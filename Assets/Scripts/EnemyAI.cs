using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    Transform player;

    public Transform[] patrolPoints;
    float minDist = 8;
    float distToPlayer = 10;
    public int speed = 2;
    int currentPoint;
    bool playerSpotted = false;


    void Start()
    {
        //Find where Player is
        player = GameObject.FindWithTag("Player").transform;

        //agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        if(!playerSpotted)
        {
            transform.position = Vector3.MoveTowards(transform.position, patrolPoints[currentPoint].position, speed * Time.deltaTime);
            distToPlayer = Vector3.Distance(transform.position, player.position);
        }

        if (distToPlayer <= minDist)
        {
            playerSpotted = true;
            transform.LookAt(player);
        }

        if ((Vector3.Distance(transform.position, patrolPoints[currentPoint].position) < 0.2) && !playerSpotted)
        {
            GoToNextPoint();
        }

        if (playerSpotted)
        {
            transform.LookAt(player);
            //transform.position = Vector3.MoveTowards(transform.position, player.position, (speed + 2) * Time.deltaTime);
            transform.position += transform.forward * (speed*2) * Time.deltaTime;
        }

    }

    void GoToNextPoint()
    {
        currentPoint++;
        if (currentPoint > patrolPoints.Length - 1)
        {
            currentPoint = 0;
        }
    }
}