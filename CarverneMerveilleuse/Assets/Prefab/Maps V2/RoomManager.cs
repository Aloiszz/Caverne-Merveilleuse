using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<GameObject> roomMemory;
    public int roomMemoryIndex;
    
    [SerializeField] public List<RoomSpawnerV2.Direction> roomMemoryDirection = new ();
    public int roomMemoryDirectionIndex;

    public List<GameObject> roomTemplateTop;    
    public int roomTemplateTopIndex;
    
    public List<GameObject> roomTemplateDown;
    public int roomTemplateDownIndex;
    
    public List<GameObject> roomTemplateRight;
    public int roomTemplateRightIndex;
    
    public List<GameObject> roomTemplateLeft;
    public int roomTemplateLeftIndex;

    
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
    
    
}
