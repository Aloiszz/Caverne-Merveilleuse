using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoomTop : MonoBehaviour
{
    public int openingDirection;
    
    private RoomTemplates templates;
    private int rand;
    public bool spawned = false;

    public Direction direction;
    public enum Direction
    {
        Top, Down, Right, Left
    };

    // Start is called before the first frame update
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Direction myDirection;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    private void OnTriggerStay2D(Collider2D other)
    {
        ChoixDeSalle(direction);
    }
    
    Direction ChoixDeSalle (Direction dir)
    {
        if (!spawned)
        {
            if (dir == Direction.Down)
            {
                rand = Random.Range(0, templates.topRooms.Length);
                Instantiate(templates.topRooms[rand], new Vector3(transform.position.x, transform.position.y -5.5f, transform.position.z), templates.topRooms[rand].transform.rotation);
            }
            else if (dir == Direction.Top)
            {
                rand = Random.Range(0, templates.bottomRooms.Length);
                Instantiate(templates.bottomRooms[rand], new Vector3(transform.position.x, transform.position.y +5.5f, transform.position.z), templates.bottomRooms[rand].transform.rotation);
            }
            
            else if (dir == Direction.Left)
            {
                rand = Random.Range(0, templates.rightRooms.Length);
                Instantiate(templates.rightRooms[rand], new Vector3(transform.position.x -5.5f, transform.position.y, transform.position.z), templates.rightRooms[rand].transform.rotation);
            }
            
            else if (dir == Direction.Right)
            {
                rand = Random.Range(0, templates.leftRooms.Length);
                Instantiate(templates.leftRooms[rand], new Vector3(transform.position.x+5.5f, transform.position.y, transform.position.z), templates.leftRooms[rand].transform.rotation);
            }
            spawned = true;
            
        }
        return dir; 
    }
}
