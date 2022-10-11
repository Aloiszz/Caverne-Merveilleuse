using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class Room : MonoBehaviour
{

    public List<GameObject> AlternativeDoor;
    public GameObject goldenDoor;
    private int result;
    public bool isGoldenPath;
    
    //[SerializeField] public List<Room.Direction> roomGoldenPath = new ();
    
    public enum Direction
    {
        Top,
        Down,
        Right,
        Left
    }

    private void Start()
    {
        RoomManager.instance.roomMemory.Add(this.gameObject);
        CreateGoldenPath();
        CreateAlternativePath();
    }
    
    
    public void CreateGoldenPath()
    {
        AlternativeDoor = GameObject.FindGameObjectsWithTag("Door").ToList();

        isGoldenPath = true;

        if (RoomManager.instance.goldenPathCount == 0)
        {
            result = Random.Range(0, AlternativeDoor.Count);
            goldenDoor = AlternativeDoor[result];
            AlternativeDoor.Remove(AlternativeDoor[result]);
            goldenDoor.gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(1,0.96f,0.016f, 0.8f),0.2f);

            RoomManager.instance.goldenPathCount++;
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
        for (int j = 0; j < AlternativeDoor.Count; j++)
        {
            //Debug.Log(AlternativeDoor[j]);
        }
        
    }
}
