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
    
    private void Start()
    {
        OnEnter();
    }

    private void OnEnter()
    {
        for (int i = 0; i < 3; i++)
        {
            objet = ObjetDispoShop.instance.listItem[Random.Range(0, ObjetDispoShop.instance.listItem.Count)];
            stockageObjetShop.Add(objet);
            ObjetDispoShop.instance.listItem.Remove(objet);
            
            if (i < 2)
            {
                merveilleux = ObjetDispoShop.instance.listMerveilleux[Random.Range(0, ObjetDispoShop.instance.listMerveilleux.Count)];
                ObjetDispoShop.instance.listMerveilleux.Remove(merveilleux);
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
    public void OnExit()
    {

        if (item1 != null)
        {
            ObjetDispoShop.instance.listItem.Add(stockageObjetShop[0]);
            Destroy(item1);
        }
        if (item2 != null)
        {
            ObjetDispoShop.instance.listItem.Add(stockageObjetShop[1]);
            Destroy(item2);
        }
        if (item3 != null)
        {
            ObjetDispoShop.instance.listItem.Add(stockageObjetShop[2]);
            Destroy(item3);
        }
        if (itemMerv1 != null)
        {
            ObjetDispoShop.instance.listMerveilleux.Add(stockageMeveilleuxShop[0]);
            Destroy(itemMerv1);
        }
        if (itemMerv2 != null)
        {
            ObjetDispoShop.instance.listMerveilleux.Add(stockageMeveilleuxShop[1]);
            Destroy(itemMerv2);
        }
        
        stockageObjetShop.Clear();
        stockageMeveilleuxShop.Clear();
    }
}
