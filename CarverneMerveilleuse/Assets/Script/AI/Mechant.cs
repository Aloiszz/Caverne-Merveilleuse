
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Mechant : MonoBehaviour
{
    private PlayerController player;

    [Header("AI Config")]
    public float life;
    public float lifeDepart;
    public GameObject lifeBarre;

    
    [Header("AI Physics")]
    public Rigidbody2D rb;
    public float linearDragDeceleration;
    public float linearDragMultiplier;
    public float forcelightDamage;
    
    [Header("Loot")]
    public GameObject dent;
    public GameObject goldenDent;
    
    [HideInInspector] public bool invokeByBoss;
    


    void Start()
    {
        lifeDepart = life;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
        rb.drag = linearDragDeceleration * linearDragMultiplier; 
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!CompareTag("Boss"))
        {
            lifeBarre.transform.localScale = new Vector2(life / lifeDepart, 0.1f);
            if (life > lifeDepart * 2 / 3)
            {
                lifeBarre.GetComponent<SpriteRenderer>().color = Color.green;
            }
            else if (life <= lifeDepart * 2 / 3 && life > lifeDepart / 3)
            {
                lifeBarre.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
            else
            {
                lifeBarre.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }

        Death();
       
    }
    
    void Death()
    {
        if (life <= 0)
        {
            if (!invokeByBoss)
            {
                for (int i = 0; i < Random.Range(1, 5); i++)
                {
                    Instantiate(dent, gameObject.transform.position + new Vector3(Random.Range(-3, 4), Random.Range(-3, 4)), Quaternion.identity);
                }

                if (Random.Range(1, 101) == 100)
                {
                    Instantiate(goldenDent, gameObject.transform.position + new Vector3(Random.Range(-3, 4), Random.Range(-3, 4)), Quaternion.identity);
                }
            }

            Destroy(gameObject);
        }
    }

    public void ReceiveLightDamage()
    {
        if (PlayerLightAttack.instance.countInput == PlayerLightAttack.instance.playerLightAttack.combo)
        {
            life -= PlayerLightAttack.instance.playerLightAttack.lastLightDamage;
        }
        else
        {
            life -= PlayerLightAttack.instance.playerLightAttack.lightDamage;
        }
        
        rb.AddForce((transform.position - player.transform.position) * forcelightDamage);
    }

}
