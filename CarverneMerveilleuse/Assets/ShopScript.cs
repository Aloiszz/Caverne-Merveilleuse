using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using Update = UnityEngine.PlayerLoop.Update;


public class ShopScript : MonoBehaviour
{
    public Transform proposition1;
    public Transform proposition2;
    public Transform proposition3;
    public Transform posiMerveilleux1;
    public Transform posiMerveilleux2;
    private GameObject merveilleux;
    private GameObject objet;
    private GameObject item1; 
    private GameObject item2; 
    private GameObject item3;
    private GameObject itemMerv1;
    private GameObject itemMerv2; 
    public List<GameObject> stockageObjetShop;
    public List<GameObject> stockageMeveilleuxShop;
    public List<GameObject> listItem;
    public List<GameObject> listMerveilleux;

    public bool test;

    private void Update()
    {
        if (test)
        {
            OnExit();
        }
    }

    private void Start()
    {
        stockageObjetShop.Clear();
        stockageMeveilleuxShop.Clear();
        for (int i = 0; i < 3; i++)
        {
            objet = listItem[Random.Range(0, listItem.Count)];
            listItem.Remove(objet);
            stockageObjetShop.Add(objet);
            if (i < 2)
            {
                merveilleux = listMerveilleux[Random.Range(0, listMerveilleux.Count)];
                listMerveilleux.Remove(merveilleux);
                stockageMeveilleuxShop.Add(merveilleux);
            }
        }

        for (int i = 0; i < stockageObjetShop.Count; i++)
        {
            switch (i)
            {
                case 0:
                    item1 = Instantiate(stockageObjetShop[i], proposition1.position, Quaternion.identity);
                    itemMerv1 = Instantiate(stockageMeveilleuxShop[i], posiMerveilleux1.position, Quaternion.identity);
                    break;
                
                case 1:
                    item2 = Instantiate(stockageObjetShop[i], proposition2.position, Quaternion.identity);
                    itemMerv2 = Instantiate(stockageMeveilleuxShop[i], posiMerveilleux2.position, Quaternion.identity);
                    break;
                
                case 2:
                    item3 = Instantiate(stockageObjetShop[i], proposition3.position, Quaternion.identity);
                    break;
                
            }
        }
    }
    void OnExit()
    {

        if (item1 != null)
        {
            listItem.Add(stockageObjetShop[0]);
        }
        if (item2 != null)
        {
            listItem.Add(stockageObjetShop[1]);
        }
        if (item3 != null)
        {
            listItem.Add(stockageObjetShop[2]);
        }
        if (itemMerv1 != null)
        {
            listItem.Add(stockageMeveilleuxShop[0]);
        }
        if (itemMerv2 != null)
        {
            listItem.Add(stockageMeveilleuxShop[1]);
        }
        
        test = false;
    }
}
