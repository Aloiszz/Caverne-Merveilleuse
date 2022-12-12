using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trouScript : MonoBehaviour
{
    private Vector2 actualVelocity;
    private Rigidbody2D playerRB;
    private Collider2D coll;
    private bool test = true;

    private void Start()
    {
        playerRB = PlayerController.instance.gameObject.GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    private void Update()
    {
        Debug.Log(PlayerController.instance.isInHole);
        if (PlayerController.instance.isDashing)
        {
            coll.isTrigger = true;
            
        }
        else
        {
            coll.isTrigger = false;
        }

        if (PlayerController.instance.isInHole)
        {
            PlayerController.instance.dashTrail.SetActive(true);
            PlayerController.instance.isDashing = true;
            playerRB.velocity = PlayerController.instance.actualVelocity;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerController.instance.isInHole = true;
            PlayerController.instance.actualVelocity = playerRB.velocity.normalized * new Vector2(50, 50);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && test)
        {
            PlayerController.instance.isInHole = true;
            PlayerController.instance.actualVelocity = playerRB.velocity.normalized * new Vector2(50, 50);
            test = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.instance.dashTrail.SetActive(false);
            PlayerController.instance.isInHole = false;
            PlayerController.instance.dashTrail.SetActive(false);
            PlayerController.instance.isDashing = false;
            test = true;
        }
    }
}
