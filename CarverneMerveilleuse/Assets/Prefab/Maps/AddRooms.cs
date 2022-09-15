using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class AddRooms : MonoBehaviour
{
    private RoomTemplates templates;

    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        templates.rooms.Add(this.gameObject);
    }
}
