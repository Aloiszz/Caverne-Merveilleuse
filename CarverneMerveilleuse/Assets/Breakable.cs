using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Breakable : MonoBehaviour
{
    public GameObject dent;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 8)
        {
            for (int i = 0; i < Random.Range(0,3); i++)
            {
                if (i > 0)
                {
                    gameObject.transform.DOMove(new Vector3(Random.Range(-3, 4), Random.Range(-3, 4)), 0.1f);
                    Instantiate(dent, gameObject.transform.position, Quaternion.identity,
                        RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].transform);
                }
            }
            Destroy(gameObject);
        }
    }
    
}
