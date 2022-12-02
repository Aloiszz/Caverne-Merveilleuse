using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Roommanager", menuName ="ScriptableObjects/new RoomManager", order = 1)]

public class SO_RoomManager : ScriptableObject
{
    [Header("----------Golden Path----------")]

    public List<GameObject> roomTemplateTop;
    public List<GameObject> roomTemplateDown;
    public List<GameObject> roomTemplateRight;
    public List<GameObject> roomTemplateLeft;
}

