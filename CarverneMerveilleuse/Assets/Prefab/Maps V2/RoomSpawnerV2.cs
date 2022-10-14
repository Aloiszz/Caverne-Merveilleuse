using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

using DG.Tweening;
using UnityEditor.ShaderGraph.Drawing;

public class RoomSpawnerV2 : MonoBehaviour
{
    
    private int rand;
    public bool colliderVierge;
    public Direction direction;
    public GameObject spawnpoint;
    public bool isAlternativeDoor;

    public int porteQuiFautDetruire;
    
    
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
                porteQuiFautDetruire = 1; // TOP

                /*if (spawnpoint == null)
                {
                    spawnpoint = GameObject.FindGameObjectWithTag("SpawnPointDown");
                }*/
                break;
            
            case Direction.Down :
                spawnpoint = GameObject.FindGameObjectWithTag("SpawnPointDown");
                porteQuiFautDetruire = 0; // Down
                /*if (spawnpoint == null)
                {
                    spawnpoint = GameObject.FindGameObjectWithTag("SpawnPointTop");
                }*/
                break;
            
            case Direction.Right :
                spawnpoint = GameObject.FindGameObjectWithTag("SpawnPointRight");
                porteQuiFautDetruire = 3; // RIGHT
                /*if (spawnpoint == null)
                {
                    spawnpoint = GameObject.FindGameObjectWithTag("SpawnPointLeft");
                }*/
                break;
            
            case Direction.Left :
                spawnpoint = GameObject.FindGameObjectWithTag("SpawnPointLeft");    
                porteQuiFautDetruire = 2; //LEFT
                /*if (spawnpoint == null)
                {
                    spawnpoint = GameObject.FindGameObjectWithTag("SpawnPointRight");
                }*/
                break;
            
        }
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            ProceduralGeneration();
        } 
    }
    
    //----------------------- Generate Room Golden Path-----------------

    void ProceduralGeneration()
    {
        if (!RoomManager.instance.isBossRoom)
        {
            if (!colliderVierge && !isAlternativeDoor)
            {
                KeepMemoryDirection();
                InstatiateNewRoom();
                Tag();
                TeleportPlayerToNextRoom(); 
                Debug.Log("Nouvelle Room");
                colliderVierge = true;
            }
            else if(!isAlternativeDoor)
            {
                if (direction == RoomManager.instance.roomMemoryDirection[^1])
                {
                    //KeepMemoryDirection();
                    Return();
                    TeleportPlayerToNextRoom();
                    Debug.Log("On Revient en arrire");
                }
                else
                {
                    Debug.Log("Ok jsuis perdo");
                    ChangeDavis();
                    TeleportPlayerToNextRoom();
                }
            }

            if (isAlternativeDoor)
            {
                KeepMemoryDirection();
                Debug.Log("HEHO MAIS C4EST UN PASSAGE FDERMER");
                InstatiateNewAlternativePath();
                TeleportPlayerToNextRoom();
            }
        }
        else
        {
            Debug.Log("UN Boss apparait");
            InstatiateBossRoom();
            TeleportPlayerToNextRoom();
        }
        AstarPath.active.Scan();
    }
    #region Generate Room
    public void TeleportPlayerToNextRoom()
    {
        PlayerController.instance.transform.position = spawnpoint.transform.position;
    }

    public void InstatiateNewRoom()
    {
        RoomManager.instance.roomMemoryIndex++;
        RoomManager.instance.roomMemoryDirectionIndex++;
        transform.parent.gameObject.SetActive(false);

        switch (direction)
        {
            case Direction.Top :
                rand = Random.Range(0, RoomManager.instance.roomTemplateDown.Count);
                Instantiate(RoomManager.instance.roomTemplateDown[rand], new Vector3(0,0,0),
                    transform.rotation).transform.GetChild(0).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
                SpawnEnnemy();
                break;  
            
            case Direction.Down :
                rand = Random.Range(0, RoomManager.instance.roomTemplateTop.Count);
                Instantiate(RoomManager.instance.roomTemplateTop[rand], new Vector3(0,0,0), 
                    transform.rotation).transform.GetChild(1).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
                SpawnEnnemy();
                break;
            
            case Direction.Right :
                rand = Random.Range(0, RoomManager.instance.roomTemplateLeft.Count);
                Instantiate(RoomManager.instance.roomTemplateLeft[rand], new Vector3(0,0,0), 
                    transform.rotation).transform.GetChild(2).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
                SpawnEnnemy();
                break;
            
            case Direction.Left :
                rand = Random.Range(0, RoomManager.instance.roomTemplateRight.Count);
                Instantiate(RoomManager.instance.roomTemplateRight[rand], new Vector3(0,0,0), 
                    transform.rotation).transform.GetChild(3).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
                SpawnEnnemy();
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
        TeleportPlayerToNextRoom();
        KeepMemoryDirection();
        RoomManager.instance.roomMemoryDirectionIndex++;
        
        RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex+1].SetActive(true);
        RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].SetActive(false);
        RoomManager.instance.roomMemoryIndex++;
    }
    

    #endregion

    //----------------------- Spawn Ennemy Region-----------------
    void SpawnEnnemy()
    {
        
    }
    
    //----------------------- Alternative Path Region-----------------

    #region Alternative Path
    public void InstatiateNewAlternativePath()
    {
        //RoomManager.instance.roomMemoryIndex++;
        //RoomManager.instance.roomMemoryDirectionIndex++;
        transform.parent.gameObject.SetActive(false);

        switch (direction)
        {
            case Direction.Top :
                rand = Random.Range(0, RoomManager.instance.roomTemplateDownEND.Count);
                Instantiate(RoomManager.instance.roomTemplateDownEND[rand], new Vector3(0,0,0),
                    transform.rotation).transform.GetChild(0).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
                break;  
            
            case Direction.Down :
                rand = Random.Range(0, RoomManager.instance.roomTemplateTopEND.Count);
                Instantiate(RoomManager.instance.roomTemplateTopEND[rand], new Vector3(0,0,0), 
                    transform.rotation).transform.GetChild(1).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
                break;
            
            case Direction.Right :
                rand = Random.Range(0, RoomManager.instance.roomTemplateLeftEND.Count);
                Instantiate(RoomManager.instance.roomTemplateLeftEND[rand], new Vector3(0,0,0), 
                    transform.rotation).transform.GetChild(2).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
                break;
            
            case Direction.Left :
                rand = Random.Range(0, RoomManager.instance.roomTemplateRightEND.Count);
                Instantiate(RoomManager.instance.roomTemplateRightEND[rand], new Vector3(0,0,0), 
                    transform.rotation).transform.GetChild(3).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;;
                break;
        }
    }
    #endregion
    
    //----------------------- Boss Room Region-----------------

    #region Boss Room
    public void InstatiateBossRoom()
    {
        //RoomManager.instance.roomMemoryIndex++;
        //RoomManager.instance.roomMemoryDirectionIndex++;
        transform.parent.gameObject.SetActive(false);
        
        rand = Random.Range(0, RoomManager.instance.bossRoom.Count);
        Instantiate(RoomManager.instance.bossRoom[rand], new Vector3(0,0,0),
            transform.rotation).transform.GetChild(0).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;

        /*switch (direction)
        {
            case Direction.Top :
                rand = Random.Range(0, RoomManager.instance.bossRoom.Count);
                Instantiate(RoomManager.instance.bossRoom[rand], new Vector3(0,0,0),
                    transform.rotation).transform.GetChild(0).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
                break;  
            
            case Direction.Down :
                rand = Random.Range(0, RoomManager.instance.bossRoom.Count);
                Instantiate(RoomManager.instance.bossRoom[rand], new Vector3(0,0,0), 
                    transform.rotation).transform.GetChild(0).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
                break;
            
            case Direction.Right :
                rand = Random.Range(0, RoomManager.instance.bossRoom.Count);
                Instantiate(RoomManager.instance.bossRoom[rand], new Vector3(0,0,0), 
                    transform.rotation).transform.GetChild(0).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
                break;
            
            case Direction.Left :
                rand = Random.Range(0, RoomManager.instance.bossRoom.Count);
                Instantiate(RoomManager.instance.bossRoom[rand], new Vector3(0,0,0), 
                    transform.rotation).transform.GetChild(0).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;;
                break;
        }*/
    }
    #endregion
    
}
