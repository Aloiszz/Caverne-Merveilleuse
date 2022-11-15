using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using DG.Tweening;

public class Room : MonoBehaviour
{

    public List<GameObject> AlternativeDoor;
    public GameObject goldenDoor;
    private int result;
    public bool isGoldenPath;
    public bool isShopRoom;

    private void Start()
    {
        FadeInRoom();
        //FindCameraBorder();
        
        AlternativeDoor = GameObject.FindGameObjectsWithTag("Door").ToList();
        
        if (!isShopRoom)
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
        var i = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (var k in i)
        {
            k.DOFade(0, 0);
            k.DOFade(0.9f, 0.7f);
        }
        
        gameObject.GetComponent<SpriteRenderer>().DOFade(1, 0.7f);
        gameObject.GetComponentInChildren<SpriteRenderer>().DOFade(1, 0.7f);
    }
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
    
}
