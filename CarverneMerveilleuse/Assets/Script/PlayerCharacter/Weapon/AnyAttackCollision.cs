using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyAttackCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("CAC") | col.CompareTag("Boss") | col.CompareTag("Dist") | col.CompareTag("Gros"))
        {
            col.GetComponent<Mechant>().OtherHit();
        }
    }
}
