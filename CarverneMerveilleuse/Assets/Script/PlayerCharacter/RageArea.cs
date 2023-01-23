using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("CAC") | col.CompareTag("Dist") | col.CompareTag("Gros"))
        {
            col.GetComponent<Mechant>().ReceiveLightDamage();
            col.GetComponent<Mechant>().RageArea();
        }
    }
}
