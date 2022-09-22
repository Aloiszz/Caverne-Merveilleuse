using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public List<GameObject> bottomRooms;
    public List<GameObject> topRooms;
    public List<GameObject> leftRooms;
    public List<GameObject> rightRooms;

    public GameObject closedRooms;

    public GameObject bossRooms;

    public List<GameObject> rooms;
    
    
    private int count = 0;
    public int maxRoom = 15;
    private int spawnBossRoom = 0;
    
    private GameObject roomSpawner;

    private void Update()
    {
        CountRoomSpawned();
    }

    private void Start()
    {
        
    }


    void CountRoomSpawned()
    {
        if (rooms.Count >= maxRoom)
        {
            bottomRooms.Clear();
            topRooms.Clear();
            leftRooms.Clear();
            rightRooms.Clear();
            
            spawnBossRoom += 1;
            if (spawnBossRoom == 1)
            {
                BossRoom();
            }
        }
    }

    void BossRoom()
    {
        Instantiate(bossRooms, new Vector3(rooms[^1].transform.position.x, rooms[^1].transform.position.y+10, rooms[^1].transform.position.z),
            bossRooms.transform.rotation);
    }
}
