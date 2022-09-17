using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoomTop : MonoBehaviour
{
    public int openingDirection;
    
    private RoomTemplates templates;
    private int rand;
    public bool spawned = false;

    // Start is called before the first frame update
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    private void OnTriggerStay2D(Collider2D other)
    {
        
        if (spawned == false)
        {
            if (openingDirection == 1)
            {
                rand = Random.Range(0, templates.topRooms.Length);
                Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
            }
            else if (openingDirection == 2)
            {
                rand = Random.Range(0, templates.bottomRooms.Length);
                Instantiate(templates.bottomRooms[rand], new Vector3(transform.position.x, transform.position.y +5.5f, transform.position.z), templates.bottomRooms[rand].transform.rotation);
            }
            else if (openingDirection == 3)
            {
                rand = Random.Range(0, templates.rightRooms.Length);
                Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
            }
            else if (openingDirection == 4)
            {
                rand = Random.Range(0, templates.leftRooms.Length);
                Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
            }
            spawned = true;
        }
    }
}
