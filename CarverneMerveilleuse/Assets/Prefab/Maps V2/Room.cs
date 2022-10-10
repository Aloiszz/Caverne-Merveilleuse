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
            var i = RoomManager.instance.roomMemoryDirection[RoomManager.instance.roomMemoryDirectionIndex-1];
            bool cbon = false;
            //Debug.Log(i);
            
            //AlternativeDoor.Remove(GameObject.FindWithTag("Door").GetComponent<RoomSpawnerV2>().colliderVierge);

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
            Debug.Log(result);
            //Debug.Log(GameObject.FindWithTag("Door").GetComponent<RoomSpawnerV2>().porteQuiFautDetruire);

            goldenDoor = AlternativeDoor[result];
            AlternativeDoor.Remove(AlternativeDoor[result]);
            goldenDoor.gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(1,0.96f,0.016f, 0.8f),0.2f);
            
            
            /*if (roomGoldenPath[0] == RoomManager.instance.roomMemoryDirection[^1])
            {
                
            }*/
        }
        
        /*switch (result)
        {
            case 0 :
                roomGoldenPath.Add(Direction.Down);
                break;
            case 1 :
                roomGoldenPath.Add(Direction.Top);
                break;
            case 2 :
                roomGoldenPath.Add(Direction.Left);
                break;
            case 3 :
                roomGoldenPath.Add(Direction.Right);
                break;
        }*

        if (roomGoldenPath[0] == Direction.Down)
        {
            
        }*/
    }
    
    public void CreateAlternativePath()
    {
        for (int j = 0; j < AlternativeDoor.Count; j++)
        {
            //Debug.Log(AlternativeDoor[j]);
        }
        
    }
}
