using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Room : MonoBehaviour
{

    public List<GameObject> AlternativeDoor;
    public GameObject goldenDoor;
    private int result;
    public bool isGoldenPath;
    public bool isHub;
    
    public bool isShopRoom;
    
    public List<GameObject> DoorEnnemyPosition; //Position des portes 
    public List<GameObject> DoorEnnemy; //Porte qui se ferme quand ennemi présent

    [SerializeField] private SpriteRenderer[] sprite;
    [SerializeField] private Tilemap[] tiles;
    public Light2D[] light;

    private void Start()
    {
        FadeInRoom();
        //SpawnPointDoorEnnemy();
        
        
        //FindCameraBorder();
        
        AlternativeDoor = GameObject.FindGameObjectsWithTag("Door").ToList();
        if (isHub)
        {
            CreateGoldenPath();
        }
        
        if (!isShopRoom && !isHub)
        {
            if (AlternativeDoor.Count > 1)
            {
                CreateGoldenPath();
                RoomManager.instance.roomMemory.Add(this.gameObject);
            }
            else
            {
                RoomManager.instance.roomMemoryAlternativePath.Add(this.gameObject);
            }
            CreateAlternativePath();
        }
        else
        {
            RoomManager.instance.roomMemory.Add(this.gameObject);
            StartCoroutine(Mini_cinematic_ShopRoom());
        }
    }

    public void FindCameraBorder()
    {
        Collider2D col = gameObject.transform.Find("CameraCollision").GetComponent<Collider2D>();
        Debug.Log(col);
        GetComponent<CinemachineConfiner>().m_BoundingShape2D = col;
    }
    void FadeInRoom()
    {
        gameObject.GetComponent<SpriteRenderer>().DOFade(0, 0);
        sprite = gameObject.GetComponentsInChildren<SpriteRenderer>();
        
        foreach (var k in sprite)
        {
            k.DOFade(0, 0);
            k.DOFade(0.9f, 0.7f);
        }
        gameObject.GetComponent<SpriteRenderer>().DOFade(1, 0.7f);
        //gameObject.GetComponentInChildren<SpriteRenderer>().DOFade(1, 0.7f);
        
        tiles = gameObject.GetComponentsInChildren<Tilemap>();
        foreach (var k in tiles)
        {
            k.DOTilemapFade(0, 0);
            k.DOTilemapFade(1, 0.7f);
        }
        
        light = gameObject.GetComponentsInChildren<Light2D>();
        foreach (var l in light)
        {
            l.DOLight2DIntensity(0, 0);
            l.DOLight2DIntensity(1, 0.7f);
        }
    }


    #region PathGeneration
    public void CreateGoldenPath()
    {
        isGoldenPath = true;
        RoomManager.instance.goldenPathCount++;
        
        if (RoomManager.instance.goldenPathCount == 0)
        {
            result = Random.Range(0, AlternativeDoor.Count);
            goldenDoor = AlternativeDoor[result];
            AlternativeDoor.Remove(AlternativeDoor[result]);
            goldenDoor.gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(1,0.96f,0.016f, 0.8f),0.2f);
        }
        else
        {
            foreach (GameObject x in AlternativeDoor.ToList())
            {
                if (x.CompareTag("Door"))
                {
                    if (x.GetComponent<RoomSpawnerV2>())
                    {
                        if (x.GetComponent<RoomSpawnerV2>().colliderVierge)
                        {
                            AlternativeDoor.Remove(x);
                        }
                    }
                }
            }
            result = Random.Range(0, AlternativeDoor.Count);

            goldenDoor = AlternativeDoor[result];
            AlternativeDoor.Remove(AlternativeDoor[result]);
            goldenDoor.gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(1,0.96f,0.016f, 0.8f),0.2f);
            
        }
        
    }
    public void CreateAlternativePath()
    {
        
        foreach (GameObject x in AlternativeDoor)
        {
            if (x.CompareTag("Door"))
            {
                x.gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(1,0.2f,0.8f, 0.8f),0.2f);
                x.GetComponent<RoomSpawnerV2>().isAlternativeDoor = true;
            }
        }
    }
    
    
    
    
    
    IEnumerator Mini_cinematic_ShopRoom()
    {
        //Camera effetc
        yield return new WaitForSeconds(0.7f);
        PlayerController.instance.enabled = true;
    }

    

    #endregion

    #region Ennemy
    void SpawnPointDoorEnnemy()
    {
        Debug.Log("Spawn Door");
        foreach (var go in GameObject.FindGameObjectsWithTag("DoorEnnemy"))
        {
            DoorEnnemyPosition.Add(go);
            GameObject door = Instantiate(EnnemyManager.instance.Door, go.transform.position, go.transform.rotation, transform);
            DoorEnnemy.Add(door);
        }
    }
    public void OpenTheDoor()
    {
        Debug.Log("Open the door");
        foreach (var i in DoorEnnemy)
        {
            i.GetComponent<SpriteRenderer>().DOFade(0, 2);
            i.GetComponent<Collider2D>().enabled = false;
        }

        LightOpenDoor();
    }
    public void CloseTheDoor()
    {
        Debug.Log("Close the door");
        foreach (var i in DoorEnnemy)
        {
            i.GetComponent<SpriteRenderer>().DOFade(1, 0.5f);
            i.GetComponent<Collider2D>().enabled = true;
        }

        LightCloseDoor();
    }
    #endregion

    void LightOpenDoor()
    {
        foreach (var i in light)
        {
            i.DOLight2DColor32(new Color32(255, 255, 255, 255), 1);
        }
    }

    void LightCloseDoor()
    {
        foreach (var i in light)
        {
            i.DOLight2DColor32(new Color32(168, 33, 33, 255), 1);
        }
    }

}
