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
        }

    }
}