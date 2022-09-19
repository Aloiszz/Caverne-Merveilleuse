using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mechant : MonoBehaviour
{
    public GameObject player;
    public float speed;

    public bool see = false;

    public float distanceToPlayer = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Attack();   
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        //PlayerController.instance.LoseLife();   
        see = true;
    }
    

    private void Attack()
    {
        if (see)
        {
            if ((player.transform.position - transform.position).magnitude < distanceToPlayer && !CompareTag("CAC"))
                {
                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position,
                        -speed * Time.deltaTime);
                }
                else
                {
                    transform.position =
                        Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

                }
        }
    }
    
}
