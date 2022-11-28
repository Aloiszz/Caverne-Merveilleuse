using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class petroleColl : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("CAC") | col.CompareTag("Boss") | col.CompareTag("Dist") | col.CompareTag("Gros"))
        {
            if (!col.GetComponent<Mechant>().isInPetrole)
            {
                col.GetComponent<Mechant>().OtherHit();
                col.GetComponent<Mechant>().isInPetrole = true;
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("CAC") | col.CompareTag("Boss") | col.CompareTag("Dist") | col.CompareTag("Gros"))
        {
            col.GetComponent<Mechant>().isInPetrole = false;
        }
    }
}
