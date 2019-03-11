using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    public int numOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Image Blood;

    Rigidbody rb;

    bool starActive = false;
    float starDuration = 3.0f;

    bool tookDamage = false;
    float bloodDuration = 0.5f;

    // Start is called before the first frame update
    void Start()
    { 
    currentHealth = numOfHearts;
    }

    // Update is called once per frame
    void Update()
    {
        if(starActive && starDuration > 0.0f)
        {
            starDuration -= Time.deltaTime;
        }
        else if (starActive && starDuration <= 0.0f) {
            starActive = false;
            starDuration = 3.0f;
        }
        else if (tookDamage && bloodDuration > 0.0f)
        {
            Blood.color = new Color(Blood.color.r, 0, 0, 0.3f);
            bloodDuration -= Time.deltaTime;
        }
        else if (tookDamage && bloodDuration <= 0.0f)
        {
            Blood.color = new Color(Blood.color.r, 0, 0, 0f);
            tookDamage = false;
            bloodDuration = 0.5f;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" && !starActive)
        {
            rb = GetComponent<Rigidbody>();
            rb.AddForce(rb.transform.forward * -25f,ForceMode.Impulse);
            Debug.Log("OUCH");
            DealDamage(1);
        }
        if (collision.gameObject.tag == "Heart")
        {
            Destroy(collision.gameObject);
            pickedUpHeart();
        }

        if (collision.gameObject.tag == "Star")
        { 
            Destroy(collision.gameObject);
            pickedUpStar();
        }


    }

    void DealDamage(int damageValue)
    {
        if(currentHealth > 1)
        {
            tookDamage = true;
        }
        currentHealth -= damageValue;

        bool changed = false;
        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i].sprite == fullHeart && !changed)
            {
                hearts[i].sprite = emptyHeart;
                changed = true;
            }
        }

        if (currentHealth <= 0)
        {
            playerDeath();
        }
    }

    void pickedUpHeart()
    {
        if(currentHealth == 3)
        {
            return;
        }

        bool changed = false;
        for (int i = hearts.Length -1 ; i >= 0; i--)
        {
            if (hearts[i].sprite == emptyHeart && !changed)
            {
                hearts[i].sprite = fullHeart;
                changed = true;
            }
        }
        currentHealth += 1;
    }

    void pickedUpStar()
    {
        starActive = true;
        //float starDuration = 3.0f;
        //while(starDuration >= 0.0f)
        //{
        //    starDuration -= Time.deltaTime;

        //}
        //starActive = false;
    }
    public void playerDeath()
    {
        Debug.Log("YOU DIED.");
        transform.position = new Vector3(5, 2, -5);
        currentHealth = 3;
        for (int i = hearts.Length - 1; i >= 0; i--)
        {
            if (hearts[i].sprite == emptyHeart)
            {
                hearts[i].sprite = fullHeart;
            }
        }
    }
}
