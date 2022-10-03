using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    
    private void Start()
    {
        RoomManager.instance.roomMemory.Add(this.gameObject);
    }
}
