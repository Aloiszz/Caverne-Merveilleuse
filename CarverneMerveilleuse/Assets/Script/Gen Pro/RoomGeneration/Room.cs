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
        Invoke("SpawnPointDoorEnnemy", EnnemyManager.instance.timeBeforeClosingDoor);
        
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
        
        FindCameraBorder();
    }

    public void FindCameraBorder()
    {
        //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineConfiner>().m_BoundingShape2D = null;
        Debug.Log("qsd");
        //PolygonCollider2D col = gameObject.transform.Find("CameraBorder").GetComponent<PolygonCollider2D>();
        Collider2D col = gameObject.transform.Find("CameraCollision").GetComponentInChildren<Collider2D>();
        Debug.Log(col);
        GameObject.FindGameObjectWithTag("Respawn").GetComponent<CinemachineConfiner>().m_BoundingShape2D = col;
        
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
            i.GetComponent<SpriteRenderer>().DOFade(0, EnnemyManager.instance.timeToOpenDoor);
            i.GetComponent<Collider2D>().enabled = false;
        }

        Light2D_OpenDoor();
    }
    public void CloseTheDoor()
    {
        Debug.Log("Close the door");
        foreach (var i in DoorEnnemy)
        {
            i.GetComponent<SpriteRenderer>().DOFade(1, EnnemyManager.instance.timeToCloseDoor);
            i.GetComponent<Collider2D>().enabled = true;
        }

        Light2D_CloseDoor();
    }
    #endregion

    
    void Light2D_OpenDoor()
    {
        foreach (var i in light)
        {
            i.DOLight2DColor32(new Color32(255, 255, 255, 255), 3);
            i.DOLight2DIntensity(1, 0.5f);
        }
    }

    void Light2D_CloseDoor()
    {
        foreach (var i in light)
        {
            i.DOLight2DColor32(new Color32(255, 15, 0, 255), 3);
            i.DOLight2DIntensity(3, 2f);
        }
    }

}
