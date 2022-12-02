using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    
    public SO_RoomManager SO_RoomManager;
    
    [Header("----------Golden Path----------")]
    public List<GameObject> roomMemory;
    public int roomMemoryIndex;
    
    [SerializeField]public List<RoomSpawnerV2.Direction> roomMemoryDirection = new ();
    public int roomMemoryDirectionIndex;

    [HideInInspector]public List<GameObject> roomTemplateTop;    
    [HideInInspector]public int roomTemplateTopIndex;
    
    [HideInInspector]public List<GameObject> roomTemplateDown;
    [HideInInspector]public int roomTemplateDownIndex;
    
    [HideInInspector]public List<GameObject> roomTemplateRight;
    [HideInInspector]public int roomTemplateRightIndex;
    
    [HideInInspector]public List<GameObject> roomTemplateLeft;
    [HideInInspector]public int roomTemplateLeftIndex;

    public int goldenPathCount = 0;

    [Header("----------Alternative Path----------")]
    public List<GameObject> roomMemoryAlternativePath;
    public int roomMemoryAlternativePathIndex;
    
    [SerializeField]public List<RoomSpawnerV2.Direction> roomMemoryDirectionAlternativePath = new ();
    public int roomMemoryDirectionAlternativePathIndex;
    
    [Range(0, 100)]
    public int maxAlternativeRoomDepth; 
    public List<GameObject> roomTemplateTopEND;    
    
    public List<GameObject> roomTemplateDownEND;
    
    public List<GameObject> roomTemplateRightEND;
    
    public List<GameObject> roomTemplateLeftEND;
    
    [Header("----------Boss Room----------")]
    public List<GameObject> bossRoom;
    public int roomLeftToBossRoom; 
    public  bool isBossRoom;


    [Header("----------Shop Room----------")]
    public List<int> roomLeftToshopRoom;
    public int roomLeftToshopRooIndex = 0; 
    public  bool isShopRoom;
    public List<GameObject> shopRoom;
    public GameObject potitChat;
    
    public static RoomManager instance;
    
    public void Awake()
    {
        if (instance != null && instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            instance = this; 
        }
        
        Secure_SO();
    }

    void Secure_SO()
    {
        roomTemplateTop = SO_RoomManager.roomTemplateTop;
        roomTemplateDown = SO_RoomManager.roomTemplateDown;
        roomTemplateRight = SO_RoomManager.roomTemplateRight;
        roomTemplateLeft = SO_RoomManager.roomTemplateLeft;
    }
    

    private bool verif;
    private void Update()
    {
        if (goldenPathCount >= roomLeftToBossRoom)
        {
            isBossRoom = true;
        }
        if (goldenPathCount >= roomLeftToshopRoom[roomLeftToshopRooIndex])
        {
            if (!verif)
            {
                Instantiate(potitChat, Vector3.zero, quaternion.identity, roomMemory[roomMemoryIndex].transform);
                roomLeftToshopRooIndex++;
                //verif = true;
            }
        }
    }
}
