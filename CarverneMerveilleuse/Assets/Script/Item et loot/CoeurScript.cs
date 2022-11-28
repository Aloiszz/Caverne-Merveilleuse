using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoeurScript : MonoBehaviour
{
    private PlayerController player;
    public float distanceToGet;
    public int nbPVRendu;
    public float speed = 4;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if ((player.transform.position - transform.position).magnitude <= distanceToGet)
        {
            transform.position = Vector2.MoveTowards(gameObject.transform.position, player.transform.position, speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            player.life += nbPVRendu;
            if (player.life > player.lifeDepard)
            {
                player.life = player.lifeDepard;
            }
            Destroy(gameObject);
        }
    }
}
