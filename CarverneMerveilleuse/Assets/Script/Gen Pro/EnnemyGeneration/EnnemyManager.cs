using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyManager : MonoBehaviour
{
    public GameObject spider;
    public GameObject bat;
    public GameObject petrol;
    public GameObject Door;
    
    
    public static EnnemyManager instance;
    
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

    private void Start()
    {
        //Door.SetActive(false);
    }
}
