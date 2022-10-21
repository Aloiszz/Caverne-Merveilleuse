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


    [Header("Info")] 
    [SerializeField] public bool isMerveilleux;
    
    private GameObject player;
    private UIManager ui;
    private ItemManager itemManager;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        ui = GameObject.Find("UIManager").GetComponent<UIManager>();
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        canvasItem = GameObject.Find("ItemCanvas").GetComponent<Canvas>();
        tmpDescriptionItem = GameObject.Find("Description").GetComponent<TMP_Text>();
        tmpNomItem = GameObject.Find("Text item").GetComponent<TMP_Text>();
    }
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
                    itemManager.OnBuy(gameObject.name);
                    Destroy(gameObject);
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
                    itemManager.OnBuy(gameObject.name);
                    Destroy(gameObject);
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
