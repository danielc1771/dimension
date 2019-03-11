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
            GameObject player = GameObject.FindWithTag("Player");
            PlayerController temp1 = (PlayerController)player.GetComponent(typeof(PlayerController));
            GameObject enemy = collider.gameObject;
            EnemyController temp = (EnemyController)enemy.GetComponent(typeof(EnemyController));

            if (temp1.IsPunching())
            {
                temp.DamageEnemy(true, collider.gameObject.GetComponent<Rigidbody>());
            }
            if (temp1.IsKicking())
            {
                temp.DamageEnemy(false, collider.gameObject.GetComponent<Rigidbody>());


                // temp.DamageEnemy(new Vector3(0f, 1000000f, 0f), collider.gameObject.GetComponent<Rigidbody>());
                // collider.gameObject.GetComponent<Rigidbody>().AddForce(1000f, 10000f, 1000f);
            }
        }
        else return;
    }


}
