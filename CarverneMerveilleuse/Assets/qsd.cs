using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class qsd : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        progElise.instance.transform.localScale = new Vector3(10,10,10);
    }
}
