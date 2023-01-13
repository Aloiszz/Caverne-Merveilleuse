using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CACHitBoxGrosEnnemis : MonoBehaviour
{
    public GrosEnnemiScript parent;

    private void Start()
    {
        parent = transform.parent.GetComponent<GrosEnnemiScript>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerController.instance.LoseLife(parent.CacDamage);
            PlayerController.instance.rb.AddForce((PlayerController.instance.transform.position - transform.position).normalized * 100,ForceMode2D.Impulse);
        }
    }
}
