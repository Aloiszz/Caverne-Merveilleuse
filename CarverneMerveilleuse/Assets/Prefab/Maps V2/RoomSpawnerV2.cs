using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomSpawnerV2 : MonoBehaviour
{
    
    private int rand;
    public bool colliderVierge;
    public Direction direction;
    
    
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
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (!colliderVierge)
            {
                KeepMemoryDirection();
                TeleportPlayer(); 
                InstatiateRoom();
                Debug.Log("Nouvelle Room");
                colliderVierge = true;
            }
            else
            {
                if (direction == RoomManager.instance.roomMemoryDirection[^1])
                {
                    KeepMemoryDirection();
                    Return();
                    Debug.Log("On Revient en arrire");
                }
            }
        }
    }
    
    public void TeleportPlayer()
    {
        PlayerController.instance.transform.position += new Vector3(10,10,0);
    }

    public void InstatiateRoom()
    {
        RoomManager.instance.roomMemoryIndex++;
        transform.parent.gameObject.SetActive(false);

        switch (direction)
        {
            case Direction.Top :
                rand = Random.Range(0, RoomManager.instance.roomTemplateDown.Count);
                Instantiate(RoomManager.instance.roomTemplateDown[rand], PlayerController.instance.transform.position, 
                    transform.rotation).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
                
                break;
            
            case Direction.Down :
                rand = Random.Range(0, RoomManager.instance.roomTemplateTop.Count);
                Instantiate(RoomManager.instance.roomTemplateTop[rand], PlayerController.instance.transform.position, 
                    transform.rotation).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
                break;
            
            case Direction.Right :
                rand = Random.Range(0, RoomManager.instance.roomTemplateLeft.Count);
                Instantiate(RoomManager.instance.roomTemplateLeft[rand], PlayerController.instance.transform.position, 
                    transform.rotation).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
                break;
            
            case Direction.Left :
                rand = Random.Range(0, RoomManager.instance.roomTemplateRight.Count);
                Instantiate(RoomManager.instance.roomTemplateRight[rand], PlayerController.instance.transform.position, 
                    transform.rotation).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;;
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
        
        RoomManager.instance.roomMemoryDirection.RemoveAt(RoomManager.instance.roomMemoryDirectionIndex);
        PlayerController.instance.transform.position -= new Vector3(10,10,0);
        
        
        RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex+1].SetActive(false);
        //RoomManager.instance.roomMemory.RemoveAt(RoomManager.instance.roomMemory.Count - 1);
        RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].SetActive(true);
    }
}
