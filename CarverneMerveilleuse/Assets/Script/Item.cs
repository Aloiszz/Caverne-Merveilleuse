using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Affichage Item")]
    public TMP_Text tmpNomItem;
    public TMP_Text tmpDescriptionItem;
    public Canvas canvasItem;
    public string nomItem;
    public string descriptionItem;
    public int prix;
    public int goldenPrix;


    [Header("Intern info")] 
    [SerializeField] public bool isMerveilleux;
    
    [Header("External info")]
    public GameObject player;
    public UIManager ui;
    
    

    private void Update()
    {
        
        if ((player.transform.position - transform.position).magnitude <= 2)
        {
            canvasItem.transform.parent = transform;
            canvasItem.transform.position = transform.position;
            canvasItem.GameObject().SetActive(true);
            tmpDescriptionItem.SetText(descriptionItem);
            if (isMerveilleux)
            {
                tmpNomItem.SetText(nomItem+" "+goldenPrix+"G");
                tmpNomItem.color = Color.yellow;
                tmpDescriptionItem.color = Color.yellow;
                if (Input.GetKeyDown(KeyCode.E) && ui.goldenMoney >= goldenPrix)
                {
                    ui.goldenMoney -= goldenPrix;
                    canvasItem.transform.parent = null;
                    canvasItem.GameObject().SetActive(false);
                    Destroy(gameObject);
                    Debug.Log("modifier les stats en fonction de l'objet pris");
                }
            }
            else
            {
                tmpNomItem.SetText(nomItem+" "+prix+"$");
                tmpNomItem.color = Color.white;
                tmpDescriptionItem.color = Color.white;
                if (Input.GetKeyDown(KeyCode.E) && ui.money >= prix)
                {
                    ui.money -= prix;
                    canvasItem.transform.parent = null;
                    canvasItem.GameObject().SetActive(false);
                    Destroy(gameObject);
                    Debug.Log("modifier les stats en fonction de l'objet pris");
                }
            }
            
            
        }
        else if (canvasItem.transform.parent == transform)
        {
            canvasItem.transform.parent = null;
            canvasItem.GameObject().SetActive(false);
        }
    }
}
