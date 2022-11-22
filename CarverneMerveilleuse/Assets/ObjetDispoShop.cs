using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjetDispoShop : MonoBehaviour
{
    public GameObject itemCanvas;
    public TMP_Text tmpDescriptionItem;
    public TMP_Text tmpNomItem;
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
