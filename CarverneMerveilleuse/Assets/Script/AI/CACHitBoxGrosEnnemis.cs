using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CACHitBoxGrosEnnemis : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerController.instance.LoseLife();
            PlayerController.instance.rb.AddForce((PlayerController.instance.transform.position - transform.position).normalized * 100,ForceMode2D.Impulse);
        }
    }
}
