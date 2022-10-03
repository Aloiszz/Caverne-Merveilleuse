using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;

    private RoomTemplates templates;
    private int rand;
    public bool spawned = false;
    public bool isCollision;
    
    public static float lastFrameSpawn;
    
    
    IEnumerator DoSpawn()
    {
        while (lastFrameSpawn - Time.timeSinceLevelLoad > -0.1f)
        {
            yield return null;
        }
        InstantiateRoom();
    }
    
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke(nameof(Spawn), 0.1f);
    }

    void InstantiateRoom()
    {
        if (isCollision)
        {
            return;
        }
        if (spawned == false)
        {
            lastFrameSpawn = Time.timeSinceLevelLoad;
            if (openingDirection == 1)
            {
                rand = Random.Range(0, templates.topRooms.Count);
                Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
            }
            else if (openingDirection == 2)
            {
                rand = Random.Range(0, templates.bottomRooms.Count);
                Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
            }
            else if (openingDirection == 3)
            {
                rand = Random.Range(0, templates.rightRooms.Count);
                Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
            }
            else if (openingDirection == 4)
            {
                rand = Random.Range(0, templates.leftRooms.Count);
                Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
            }
            spawned = true;
        }
    }
    
    
    // Update is called once per frame
    void Spawn()
    {
        StartCoroutine(DoSpawn());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            isCollision = true;
            spawned = true;
            Debug.Log(transform.position + transform.parent.gameObject.name);
            
            if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                Instantiate(templates.closedRooms, transform.position, quaternion.identity);
                Destroy(other.gameObject);
            }
        }
    }
}
