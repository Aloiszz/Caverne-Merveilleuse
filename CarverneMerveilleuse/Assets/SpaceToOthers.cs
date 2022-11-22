using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class SpaceToOthers : MonoBehaviour
{
    public GameObject ennemi;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Boss") || other.CompareTag("CAC") || other.CompareTag("Gros") || other.CompareTag("Dist") && other.gameObject != ennemi)
        {
            ennemi.transform.position = Vector2.MoveTowards(ennemi.transform.position,
                other.transform.position, -0.8f * Time.deltaTime);
            
        }
    }
}
