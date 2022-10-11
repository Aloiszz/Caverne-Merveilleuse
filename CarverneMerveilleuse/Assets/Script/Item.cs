using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    public TMP_Text tmpNomItem;
    public TMP_Text tmpDescriptionItem;
    public Canvas canvasItem;
    public GameObject player;
    public string nomItem;
    public string descriptionItem;
    public int prix;
    public UIManager ui;
    
    

    private void Update()
    {
        
        if ((player.transform.position - transform.position).magnitude <= 2)
        {
            canvasItem.transform.parent = transform;
            canvasItem.transform.position = transform.position;
            canvasItem.GameObject().SetActive(true);
            tmpNomItem.SetText(nomItem+prix+"$");
            tmpDescriptionItem.SetText(descriptionItem);
            if (Input.GetKeyDown(KeyCode.E) && ui.money >= prix)
            {
                ui.money -= prix;
                canvasItem.transform.parent = null;
                canvasItem.GameObject().SetActive(false);
                Destroy(gameObject);
                Debug.Log("modifier les stats en fonction de l'objet pris");
            }
        }
        else if (canvasItem.transform.parent == transform)
        {
            canvasItem.transform.parent = null;
            canvasItem.GameObject().SetActive(false);
        }
    }
}
