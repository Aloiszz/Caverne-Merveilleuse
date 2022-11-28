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
        }
    }
}
