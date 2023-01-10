using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [Header("Affichage Item")]
    public ObjetDispoShop canvasItem;
    public string nomItem;
    public string descriptionItem;
    public int prix;
    public int goldenPrix;
    


    [Header("Info")] 
    public int ID;
    [SerializeField] public bool isMerveilleux;
    
    private GameObject player;
    private UIManager ui;
    private ItemManager itemManager;
    private Vector3 spawnPos;
    private Vector3 newPos;

    private void Start()
    {
        spawnPos = transform.position;
        newPos = transform.position + new Vector3(0, 0.4f, 0);
        player = GameObject.FindWithTag("Player");
        ui = GameObject.Find("UIManager").GetComponent<UIManager>();
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        canvasItem = GameObject.Find("ItemManager").GetComponent<ObjetDispoShop>();
    }
    private void Update()
    {
        if ((player.transform.position - spawnPos).magnitude <= 2)
        {
            canvasItem.itemCanvas.transform.parent = transform;
            canvasItem.itemCanvas.transform.position = spawnPos;
            canvasItem.itemCanvas.SetActive(true); 
            canvasItem.tmpDescriptionItem.SetText(descriptionItem);
            if (isMerveilleux)
            {
                canvasItem.tmpNomItem.SetText(nomItem+" "+goldenPrix+"G");
                canvasItem.tmpNomItem.color = Color.yellow;
                canvasItem.tmpDescriptionItem.color = Color.yellow;
                if (Input.GetKeyDown(KeyCode.E) && ui.goldenMoney >= goldenPrix)
                {
                    
                    ui.goldenMoney -= goldenPrix;
                    canvasItem.itemCanvas.transform.parent = null;
                    canvasItem.itemCanvas.SetActive(false);
                    itemManager.OnBuy(ID);
                    Destroy(gameObject);
                }
            }
            else
            {
                canvasItem.tmpNomItem.SetText(nomItem+" "+prix+"$");
                canvasItem.tmpNomItem.color = Color.white;
                canvasItem.tmpDescriptionItem.color = Color.white;
                if (Input.GetKeyDown(KeyCode.E) && ui.money >= prix)
                {
                    ui.money -= prix;
                    canvasItem.itemCanvas.transform.parent = null;
                    canvasItem.itemCanvas.SetActive(false);
                    itemManager.OnBuy(ID);
                    Destroy(gameObject);
                }
            }

            if (transform.position.y <= newPos.y)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + 0.05f);
            }
            
        }
        else if (canvasItem.itemCanvas.transform.parent == transform)
        {
            canvasItem.itemCanvas.transform.parent = null;
            canvasItem.itemCanvas.SetActive(false);
            
        }
        else if (transform.position.y >= spawnPos.y)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - 0.05f);
        }
    }
}
