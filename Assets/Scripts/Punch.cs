using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Detects collision with enemy
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            //makes sure animation is running/punch key has been pressed. if this check it not here, the player can walk into the enemy and damage it
            //if (transform.parent.parent.GetComponent<PlayerController>().IsPunching())
            {
                GameObject enemy = collider.gameObject;
                EnemyAI temp = (EnemyAI)enemy.GetComponent(typeof(EnemyAI));

                //DamageEnemy takes in a force vector. FIXME these values are just for testing. might want to lower them
                temp.DamageEnemy(new Vector3(10000f, 10000f, 10000f), collider.gameObject.GetComponent<Rigidbody>());
            }
        }
        else return;
    }


}
