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
        RoomManager.instance.roomMemory.Add(this.gameObject);
        CreateGoldenPath();
        CreateAlternativePath();
    }
    
    
    public void CreateGoldenPath()
    {
        AlternativeDoor = GameObject.FindGameObjectsWithTag("Door").ToList();
        result = Random.Range(0, AlternativeDoor.Count);

        switch (result)
        {
            case 0 :
                direction = Direction.Down;
                break;
            case 1 :
                direction = Direction.Top;
                break;
            case 2 :
                direction = Direction.Left;
                break;
            case 3 :
                direction = Direction.Right;
                break;
        }
        Debug.Log(direction);

        if (RoomManager.instance.goldenPathCount == 0)
        {
            
            goldenDoor = AlternativeDoor[result];
            AlternativeDoor.Remove(AlternativeDoor[result]);
            goldenDoor.gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(1,0.96f,0.016f, 0.8f),0.2f);

            RoomManager.instance.goldenPathCount++;
        }
        else
        {
            var i = RoomManager.instance.roomMemoryDirection[RoomManager.instance.roomMemoryDirectionIndex-1];
            Debug.Log(i);
            
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
