using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Header("----------Golden Path----------")]
    public List<GameObject> roomMemory;
    public int roomMemoryIndex;
    
    [SerializeField]public List<RoomSpawnerV2.Direction> roomMemoryDirection = new ();
    public int roomMemoryDirectionIndex;

    public List<GameObject> roomTemplateTop;    
    public int roomTemplateTopIndex;
    
    public List<GameObject> roomTemplateDown;
    public int roomTemplateDownIndex;
    
    public List<GameObject> roomTemplateRight;
    public int roomTemplateRightIndex;
    
    public List<GameObject> roomTemplateLeft;
    public int roomTemplateLeftIndex;

    public int goldenPathCount = 0;
    public int roomLeftToBossRoom; 
    public  bool isBossRoom;
    
    
    [Header("----------Alternative Path----------")]
    public List<GameObject> roomMemoryAlternativePath;
    public int roomMemoryAlternativePathIndex;
    
    [SerializeField]public List<RoomSpawnerV2.Direction> roomMemoryDirectionAlternativePath = new ();
    public int roomMemoryDirectionAlternativePathIndex;
    
    public List<GameObject> roomTemplateTopEND;    
    
    public List<GameObject> roomTemplateDownEND;
    
    public List<GameObject> roomTemplateRightEND;
    
    public List<GameObject> roomTemplateLeftEND;
    
    [Header("----------Boss Room----------")]
    public List<GameObject> bossRoom;
    
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
        
    }

    private void Update()
    {
        if (goldenPathCount >= roomLeftToBossRoom)
        {
            isBossRoom = true;
        }
    }
}
