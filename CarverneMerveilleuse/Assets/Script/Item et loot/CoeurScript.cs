using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class CoeurScript : MonoBehaviour
{
    private PlayerController player;
    private Rigidbody2D rb;
    public float distanceToGet;
    public int nbPVRendu;
    public float speed = 4;
    

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(Random.Range(-3,4), Random.Range(-3,4)) * 2, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        rb.drag = 2.3f;
    }

    private void Update()
    {
        if ((player.transform.position - transform.position).magnitude <= distanceToGet)
        {
            transform.position = Vector2.MoveTowards(gameObject.transform.position, player.transform.position, speed);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            player.life += nbPVRendu;
            /*if (player.life > player.lifeDepard)
            {
                player.life = player.lifeDepard;
            }*/
            Destroy(gameObject);
        }
    }
}
