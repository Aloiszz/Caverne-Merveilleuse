using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

public class RoomSpawnerV2 : MonoBehaviour
{
    private int rand;
    public bool colliderVierge;
    public Direction direction;
    public GameObject spawnpoint;
    public bool isAlternativeDoor;
    public bool isShopDoor;

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
        SpawnPointLocation();
    }
    

    void SpawnPointLocation()
    {
        switch (direction)
        {
            case Direction.Top :
                spawnpoint = GameObject.FindGameObjectWithTag("SpawnPointTop");
                porteQuiFautDetruire = 1; // TOP
                
                break;
            
            case Direction.Down :
                spawnpoint = GameObject.FindGameObjectWithTag("SpawnPointDown");
                porteQuiFautDetruire = 0; // Down
                break;
            
            case Direction.Right :
                spawnpoint = GameObject.FindGameObjectWithTag("SpawnPointRight");
                porteQuiFautDetruire = 3; // RIGHT
                break;
            
            case Direction.Left :
                spawnpoint = GameObject.FindGameObjectWithTag("SpawnPointLeft");    
                porteQuiFautDetruire = 2; //LEFT
                break;
            
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            ProceduralGeneration(); //Entre en contact avec une porte
            if (PlayerThrowAttack.instance.isThrow)
            {
                PlayerThrowAttack.instance.ReturnWeaponImmediate();
            }
        } 
    }
    
    //----------------------- Room Generation -----------------

    void ProceduralGeneration()
    {
        if (!RoomManager.instance.isBossRoom)
        {
            if (isShopDoor)
            {
                ChatMarchand.instance.PlayerCameBack = true;
                ChatMarchand.instance.StartCoroutine(ChatMarchand.instance.PlayerCameback());
                Debug.Log("Come Back From Shop");
                ReturnShopRoom();
                TeleportPlayerBackToRoom();
            }
            
            if (!colliderVierge && !isAlternativeDoor)
            {

                if(RoomManager.instance.goldenPathCount >= RoomManager.instance.roomLeftToBossRoom && RoomManager.instance.roomMemoryDirection[^1] == Direction.Down)
                {
                    KeepMemoryDirection();
                    InstatiatePreBossRoom();
                    SpawnPointLocation();
                    TeleportPlayerToNextRoom(); 
                    RoomManager.instance.isBossRoom = true;
                }
                else if (RoomManager.instance.goldenPathCount >= RoomManager.instance.roomLeftToBossRoom)
                {
                    KeepMemoryDirection();
                    InstatiateNewRoom();
                    SpawnPointLocation();
                    TeleportPlayerToNextRoom();
                    Debug.Log("PreBossRoom");
                    colliderVierge = true;
                    RoomManager.instance.PreBoss();
                }
                else
                {
                    KeepMemoryDirection();
                    InstatiateNewRoom();
                    SpawnPointLocation();
                    TeleportPlayerToNextRoom(); 
                    Debug.Log("Nouvelle Room");
                    colliderVierge = true;
                }
                
            }
            else if(!isAlternativeDoor && !isShopDoor)
            {
                if (direction == RoomManager.instance.roomMemoryDirection[^1])
                {
                    //KeepMemoryDirection();
                    Return();
                    SpawnPointLocation();
                    TeleportPlayerToNextRoom();
                    Debug.Log("On Revient en arrire");
                }
                else
                {
                    Debug.Log("Ok jsuis perdo");
                    ChangeDavis();
                    SpawnPointLocation();
                    TeleportPlayerToNextRoom();
                }
            }

            if (isAlternativeDoor)
            {
                if(!colliderVierge)
                {
                    KeepMemoryDirectionAlternativePath();
                    Debug.Log("HEHO MAIS C4EST UN PASSAGE FDERMER");
                    InstatiateNewAlternativePath();
                    SpawnPointLocation();
                    TeleportPlayerToNextRoom();
                }
                else
                {
                    if (direction == RoomManager.instance.roomMemoryDirectionAlternativePath[^1])
                    {
                        Debug.Log("On Revient en arrire alternativement bebe");
                        ReturnAlternativePath();
                        SpawnPointLocation();
                        TeleportPlayerToNextRoom();
                    }
                    else
                    {
                        /*Debug.Log("Ok jsuis perdo dans l'alternativement passages secrets");
                        ChangeDavisAlternativePath();
                        SpawnPointLocation();
                        TeleportPlayerToNextRoom();*/
                    }
                }
            }
            
            /*if (RoomManager.instance.isShopRoom)
            {
                Debug.Log("fsdf");
                GenerateShopRoom();
                SpawnPointLocation();
                TeleportPlayerToNextRoom();
            }*/
        }
        else
        {
            if (direction == Direction.Top && RoomManager.instance.roomMemory[^1].gameObject.activeInHierarchy)
            {
                Debug.Log("Une Salle Boss apparait");
                InstatiateBossRoom();
                SpawnPointLocation();
                TeleportPlayerToNextRoom();
                AstarPath.active.Scan();
            }
            else
            {
                if (isShopDoor)
                {
                    ChatMarchand.instance.PlayerCameBack = true;
                    ChatMarchand.instance.StartCoroutine(ChatMarchand.instance.PlayerCameback());
                    Debug.Log("Come Back From Shop");
                    ReturnShopRoom();
                    TeleportPlayerBackToRoom();
                }
                else if(!isAlternativeDoor && !isShopDoor)
                {
                    if (direction == RoomManager.instance.roomMemoryDirection[^1])
                    {
                        //KeepMemoryDirection();
                        Return();
                        SpawnPointLocation();
                        TeleportPlayerToNextRoom();
                        Debug.Log("On Revient en arrire");
                    }
                    else
                    {
                        Debug.Log("Ok jsuis perdo");
                        ChangeDavis();
                        SpawnPointLocation();
                        TeleportPlayerToNextRoom();
                    }
                }
                if (isAlternativeDoor)
                {
                    if(!colliderVierge)
                    {
                        KeepMemoryDirectionAlternativePath();
                        Debug.Log("HEHO MAIS C4EST UN PASSAGE FDERMER");
                        InstatiateNewAlternativePath();
                        SpawnPointLocation();
                        TeleportPlayerToNextRoom();
                    }
                    else
                    {
                        if (direction == RoomManager.instance.roomMemoryDirectionAlternativePath[^1])
                        {
                            Debug.Log("On Revient en arrire alternativement bebe");
                            ReturnAlternativePath();
                            SpawnPointLocation();
                            TeleportPlayerToNextRoom();
                        }
                        else
                        {
                            /*Debug.Log("Ok jsuis perdo dans l'alternativement passages secrets");
                            ChangeDavisAlternativePath();
                            SpawnPointLocation();
                            TeleportPlayerToNextRoom();*/
                        }
                    }
                }
            }
        }
        AstarPath.active.Scan();
    }
    
    #region Generate Room
    public void TeleportPlayerToNextRoom()
    {
        PlayerController.instance.transform.position = spawnpoint.transform.position;
        ItemManager.instance.LightShield();
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
                    Quaternion.identity).transform.GetChild(0).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
                break;  
            
            case Direction.Down :
                rand = Random.Range(0, RoomManager.instance.roomTemplateTop.Count);
                Instantiate(RoomManager.instance.roomTemplateTop[rand], new Vector3(0,0,0), 
                    Quaternion.identity).transform.GetChild(1).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
                break;
            
            case Direction.Right :
                rand = Random.Range(0, RoomManager.instance.roomTemplateLeft.Count);
                Instantiate(RoomManager.instance.roomTemplateLeft[rand], new Vector3(0,0,0), 
                    Quaternion.identity).transform.GetChild(2).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
                break;
            
            case Direction.Left :
                rand = Random.Range(0, RoomManager.instance.roomTemplateRight.Count);
                Instantiate(RoomManager.instance.roomTemplateRight[rand], new Vector3(0,0,0), 
                    Quaternion.identity).transform.GetChild(3).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
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

        if (RoomManager.instance.roomMemoryDirectionIndex != 0)
        {
            RoomManager.instance.roomMemoryDirection.RemoveAt(RoomManager.instance.roomMemoryDirectionIndex);
        }

        RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex+1].SetActive(false);
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
    
    //----------------------- Alternative Path Region-----------------

    #region Alternative Path
    public void InstatiateNewAlternativePath()
    {
        RoomManager.instance.roomMemoryAlternativePathIndex++;
        RoomManager.instance.roomMemoryDirectionAlternativePathIndex++;
        transform.parent.gameObject.SetActive(false);

        switch (direction)
        {
            case Direction.Top :
                rand = Random.Range(0, RoomManager.instance.roomTemplateDownEND.Count);
                Instantiate(RoomManager.instance.roomTemplateDownEND[rand], new Vector3(0,0,0),
                    Quaternion.identity).transform.GetChild(0).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
                break;  
            
            case Direction.Down :
                rand = Random.Range(0, RoomManager.instance.roomTemplateTopEND.Count);
                Instantiate(RoomManager.instance.roomTemplateTopEND[rand], new Vector3(0,0,0), 
                    Quaternion.identity).transform.GetChild(1).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
                break;
            
            case Direction.Right :
                rand = Random.Range(0, RoomManager.instance.roomTemplateLeftEND.Count);
                Instantiate(RoomManager.instance.roomTemplateLeftEND[rand], new Vector3(0,0,0), 
                    Quaternion.identity).transform.GetChild(2).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
                break;
            
            case Direction.Left :
                rand = Random.Range(0, RoomManager.instance.roomTemplateRightEND.Count);
                Instantiate(RoomManager.instance.roomTemplateRightEND[rand], new Vector3(0,0,0), 
                    Quaternion.identity).transform.GetChild(3).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;;
                break;
        }
    }
    public void KeepMemoryDirectionAlternativePath()
    {
        switch (direction)
        {
            case Direction.Top:
                RoomManager.instance.roomMemoryDirectionAlternativePath.Add(Direction.Down);
                break;
            
            case Direction.Down:
                RoomManager.instance.roomMemoryDirectionAlternativePath.Add(Direction.Top);
                break; 
            
            case Direction.Right:
                RoomManager.instance.roomMemoryDirectionAlternativePath.Add(Direction.Left);
                break;
            
            case Direction.Left:
                RoomManager.instance.roomMemoryDirectionAlternativePath.Add(Direction.Right);
                break;
        }
    }
    public void ReturnAlternativePath()
    {
        RoomManager.instance.roomMemoryAlternativePath[RoomManager.instance.roomMemoryAlternativePathIndex].SetActive(false);
        RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].SetActive(true);
        
        RoomManager.instance.roomMemoryAlternativePath.RemoveAt(RoomManager.instance.roomMemoryAlternativePathIndex);
        
        RoomManager.instance.roomMemoryAlternativePathIndex--;
        RoomManager.instance.roomMemoryDirectionAlternativePathIndex--;
    }

    public void ChangeDavisAlternativePath()
    {
        RoomManager.instance.roomMemoryDirectionAlternativePathIndex++;
        RoomManager.instance.roomMemoryAlternativePathIndex++;
        
        RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].SetActive(true);
        RoomManager.instance.roomMemoryAlternativePath[RoomManager.instance.roomMemoryAlternativePathIndex].SetActive(false);
    }
    #endregion
    
    //----------------------- Boss Room Region-----------------

    #region Boss Room
    public void InstatiateBossRoom()
    {
        RoomManager.instance.roomMemoryIndex++;
        RoomManager.instance.roomMemoryDirectionIndex++;
        transform.parent.gameObject.SetActive(false);
        
        rand = Random.Range(0, RoomManager.instance.bossRoom.Count);
        Instantiate(RoomManager.instance.bossRoom[rand], new Vector3(0,0,0),
            Quaternion.identity).transform.GetChild(0).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
    }
    
    public void InstatiatePreBossRoom()
    {
        RoomManager.instance.roomMemoryIndex++;
        RoomManager.instance.roomMemoryDirectionIndex++;
        transform.parent.gameObject.SetActive(false);

        switch (direction)
        {
            case Direction.Top :
                Instantiate(RoomManager.instance.PreBossRoom, new Vector3(0,0,0),
                    Quaternion.identity).transform.GetChild(0).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
                break;  
            
            case Direction.Down :
                Instantiate(RoomManager.instance.PreBossRoom, new Vector3(0,0,0), 
                    Quaternion.identity).transform.GetChild(1).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
                break;
            
            case Direction.Right : ;
                Instantiate(RoomManager.instance.PreBossRoom, new Vector3(0,0,0), 
                    Quaternion.identity).transform.GetChild(2).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
                break;
            
            case Direction.Left :
                Instantiate(RoomManager.instance.PreBossRoom, new Vector3(0,0,0), 
                    Quaternion.identity).transform.GetChild(3).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
                break;
        }
    }
    
    public void KeepMemoryDirectionPreBoss()
    {
        switch (direction)
        {
            case Direction.Top:
                RoomManager.instance.roomMemoryDirection.Add(Direction.Down);
                break;
            
            case Direction.Down:
                RoomManager.instance.roomMemoryDirection.Add(Direction.Down);
                break; 
            
            case Direction.Right:
                RoomManager.instance.roomMemoryDirection.Add(Direction.Down);
                break;
            
            case Direction.Left:
                RoomManager.instance.roomMemoryDirection.Add(Direction.Down);
                break;
        }
    }
    #endregion
    
    
    //----------------------- Shop Room Region -----------------

    #region Shop

    void ReturnShopRoom()
    {
        RoomManager.instance.roomMemoryIndex--;
        RoomManager.instance.roomMemoryDirectionIndex--;
        
        RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex+1].GetComponent<ShopScript>().OnExit();
        RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex+1].SetActive(false);
        RoomManager.instance.roomMemory.RemoveAt(RoomManager.instance.roomMemoryIndex+1);
        RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].SetActive(true);
        
    }
    public void TeleportPlayerBackToRoom()
    {
        PlayerController.instance.transform.position = GameObject.FindGameObjectWithTag("Chat").transform.position;
        ChatMarchand.instance.CatDesapear();
    }
    #endregion
}
