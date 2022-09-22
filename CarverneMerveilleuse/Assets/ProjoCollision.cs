using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjoCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Rigidbody2D>().AddForce((col.transform.position - transform.position) * 500);
            Destroy(gameObject);
            PlayerController.instance.LoseLife();
        }

        else if(!col.gameObject.CompareTag("CAC") | !col.gameObject.CompareTag("Boss") | !col.gameObject.CompareTag("Dist"))
        {
            Destroy(gameObject);
        }

    }
}