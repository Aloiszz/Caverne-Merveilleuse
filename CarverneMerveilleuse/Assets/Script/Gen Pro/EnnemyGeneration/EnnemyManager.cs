using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyManager : MonoBehaviour
{
    
    [Header("Ennemies")]
    public GameObject spider;
    public GameObject bat;
    public GameObject petrol;

    public float timeBeforeFighting = 2; // temps avant le combats
    public float timeToAppearEnnemy = 0.7f; // temps pour activer les ennemis (d'abors le cercle puis activer les ennemies)
    
    [Header("Door")]
    public GameObject Door;
    public float timeBeforeClosingDoor = 0.7f; // temps avant de fermer les portes
    public float timeToOpenDoor = 2; // temps pour ouvrir les portes
    public float timeToCloseDoor = 1; // temps pour fermer les portes
    
    
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
