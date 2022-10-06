using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

using DG.Tweening;

public class RoomSpawnerV2 : MonoBehaviour
{
    
    private int rand;
    public bool colliderVierge;
    public Direction direction;
    public GameObject spawnpoint;
    
    
    public enum Direction
    {
        Top,
        Down,
        Right,
        Left
    }
    
    private void Start()
    {
        Direction myDirection;
        
        Tag();
    }

    void Tag()
    {
        switch (direction)
        {
            case Direction.Top :
                spawnpoint = GameObject.FindGameObjectWithTag("SpawnPointTop");
                break;
            case Direction.Down :
                spawnpoint = GameObject.FindGameObjectWithTag("SpawnPointDown");
                break;
            case Direction.Right :
                spawnpoint = GameObject.FindGameObjectWithTag("SpawnPointRight");
                break;
            case Direction.Left :
                spawnpoint = GameObject.FindGameObjectWithTag("SpawnPointLeft");
                break;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (!colliderVierge)
            {
                KeepMemoryDirection();
                InstatiateRoom();
                Tag();
                TeleportPlayer(); 
                Debug.Log("Nouvelle Room");
                colliderVierge = true;
            }
            else
            {
                if (direction == RoomManager.instance.roomMemoryDirection[^1])
                {
                    //KeepMemoryDirection();
                    Return();
                    Debug.Log("On Revient en arrire");
                }
                else
                {
                    Debug.Log("Ok jsuis perdo");
                    ChangeDavis();
                }
            }
        }
    }
    
    public void TeleportPlayer()
    {
        PlayerController.instance.transform.position = spawnpoint.transform.position;
    }

    public void InstatiateRoom()
    {
        RoomManager.instance.roomMemoryIndex++;
        RoomManager.instance.roomMemoryDirectionIndex++;
        transform.parent.gameObject.SetActive(false);

        switch (direction)
        {
            case Direction.Top :
                rand = Random.Range(0, RoomManager.instance.roomTemplateDown.Count);
                Instantiate(RoomManager.instance.roomTemplateDown[rand], spawnpoint.transform.position,
                    transform.rotation).transform.GetChild(0).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
                break;  
            
            case Direction.Down :
                rand = Random.Range(0, RoomManager.instance.roomTemplateTop.Count);
                Instantiate(RoomManager.instance.roomTemplateTop[rand], spawnpoint.transform.position, 
                    transform.rotation).transform.GetChild(1).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
                break;
            
            case Direction.Right :
                rand = Random.Range(0, RoomManager.instance.roomTemplateLeft.Count);
                Instantiate(RoomManager.instance.roomTemplateLeft[rand], spawnpoint.transform.position, 
                    transform.rotation).transform.GetChild(2).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
                break;
            
            case Direction.Left :
                rand = Random.Range(0, RoomManager.instance.roomTemplateRight.Count);
                Instantiate(RoomManager.instance.roomTemplateRight[rand], spawnpoint.transform.position, 
                    transform.rotation).transform.GetChild(3).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;;
                break;
        }
    }

    public void KeepMemoryDirection()
    {
        switch (direction)
        {
            case Direction.Top:
                RoomManager.instance.roomMemoryDirection.Add(Direction.Down);
                break;
            
            case Direction.Down:
                RoomManager.instance.roomMemoryDirection.Add(Direction.Top);
                break; 
            
            case Direction.Right:
                RoomManager.instance.roomMemoryDirection.Add(Direction.Left);
                break;
            
            case Direction.Left:
                RoomManager.instance.roomMemoryDirection.Add(Direction.Right);
                break;
        }
    }

    public void Return()
    {
        RoomManager.instance.roomMemoryIndex--;
        RoomManager.instance.roomMemoryDirectionIndex--;
        
        RoomManager.instance.roomMemoryDirection.RemoveAt(RoomManager.instance.roomMemoryDirectionIndex);
        //PlayerController.instance.transform.position -= new Vector3(10,0,0);
        
        
        RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex+1].SetActive(false);
        //RoomManager.instance.roomMemory.RemoveAt(RoomManager.instance.roomMemory.Count - 1);
        RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].SetActive(true);
    }

    public void ChangeDavis()
    {
        TeleportPlayer();
        KeepMemoryDirection();
        RoomManager.instance.roomMemoryDirectionIndex++;
        
        RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex+1].SetActive(true);
        RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].SetActive(false);
        RoomManager.instance.roomMemoryIndex++;
    }
}
