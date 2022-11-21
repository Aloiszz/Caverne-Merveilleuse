using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetDispoShop : MonoBehaviour
{
    public List<GameObject> listItem;
    public List<GameObject> listMerveilleux;

    public static ObjetDispoShop instance;

    private void Awake()
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
