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
    private Collider2D coll;
    

    private void Start()
    {
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(Random.Range(-3,4), Random.Range(-3,4)) * 2, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        rb.drag = 2.3f;
    }

    private void Update()
    {
        if ((PlayerController.instance.gameObject.transform.position - transform.position).magnitude <= distanceToGet)
        {
            coll.isTrigger = true;
            transform.position = Vector2.MoveTowards(gameObject.transform.position, PlayerController.instance.gameObject.transform.position, speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerController.instance.life += nbPVRendu;
            /*if (player.life > player.lifeDepard)
            {
                player.life = player.lifeDepard;
            }*/
            Destroy(gameObject);
        }
    }
}
