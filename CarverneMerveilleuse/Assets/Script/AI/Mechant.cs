using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Mechant : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject player;
    public float speed;

    public bool see = false;

    public float distanceToPlayer = 5f;

    public GameObject projo;

    public float TimeBeforeShoot = 3f;

    private bool canShoot = true;

    private bool canRandomMove = true;

    private Vector2 playerDir;

    public float shootForce = 3f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerDir =(player.transform.position - transform.position);
        OnSeePlayer();
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        //PlayerController.instance.LoseLife(); 
        if (col.gameObject.CompareTag("Player"))
        {
            see = true;
        }
    }
    

    private void OnSeePlayer()
    {
        if (see)
        {
            StopCoroutine(RandomMove());
            if (playerDir.magnitude < distanceToPlayer && !CompareTag("CAC"))
                {
                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position,
                        -speed * Time.deltaTime);
                    
                    
                }
                else
                {
                    transform.position =
                        Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

                }
            if (canShoot && !CompareTag("CAC"))
            {
                StartCoroutine(Shoot());
            }
        }
        else if (canRandomMove)
        {
            StartCoroutine(RandomMove());
        }
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        GameObject projectile = Instantiate(projo, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().AddForce(playerDir * shootForce);
        yield return new WaitForSeconds(TimeBeforeShoot);
        canShoot = true;
        
    }

    IEnumerator RandomMove()
    {
        canRandomMove = false;
        rb.AddForce(new Vector2(Random.Range(-9f, 10f),Random.Range(-9f,10f)) * 25);
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(3f);
        canRandomMove = true;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Rigidbody2D>().AddForce(playerDir * 2000);
        }
    }
}
