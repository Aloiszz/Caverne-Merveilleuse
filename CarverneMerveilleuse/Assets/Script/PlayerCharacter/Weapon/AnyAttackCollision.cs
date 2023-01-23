using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;
public class AnyAttackCollision : MonoBehaviour
{
    
    [SerializeField]private GameObject bloodPS;
    [SerializeField]private GameObject bloodPSFloor;
    private int rand;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("CAC") | col.CompareTag("Boss") | col.CompareTag("Dist") | col.CompareTag("Gros"))
        {
            col.GetComponent<Mechant>().OtherHit();
            
            rand = Random.Range(1, 3);
            for (int i = 0; i < rand; i++)
            {
                Instantiate(bloodPS, col.transform.position, Quaternion.identity,
                    RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].transform);
                
                Instantiate(bloodPSFloor, col.transform.position, Quaternion.identity, 
                    RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].transform);
            }
        }
    }
}
