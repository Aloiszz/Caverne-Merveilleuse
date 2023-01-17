using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderInLayerSetter : MonoBehaviour
{
    private SpriteRenderer sprite;
    private int ord;
    public float baseObjet;
    public bool isGroup;
    public List<GameObject> objets;
    [HideInInspector]public List<SpriteRenderer> renList;
    [HideInInspector]public List<int> ordList;

    private void Start()
    {
        
        if (isGroup)
        {
            for (int i = 0; i < objets.Count; i++)
            {
                renList.Add(objets[i].GetComponent<SpriteRenderer>());
            }
            for (int i = 0; i < renList.Count; i++)
            {
                ordList.Add(renList[i].sortingOrder);
            }
        }
        else
        {
            sprite = GetComponent<SpriteRenderer>();
            ord = sprite.sortingOrder;
        }
    }

    private void Update()
    {
        if (isGroup)
        {
            if (transform.position.y - baseObjet >= PlayerController.instance.transform.position.y)
            {
                for (int i = 0; i < renList.Count; i++)
                {
                    renList[i].sortingOrder = ordList[i];
                }
                
            }
            else
            {
                for (int i = 0; i < renList.Count; i++)
                {
                    renList[i].sortingOrder = ordList[i] + 11;
                }
            }
        }
        else
        {
            if (transform.position.y - baseObjet >= PlayerController.instance.transform.position.y)
            {
                sprite.sortingOrder = ord;
            }
            else
            {
                sprite.sortingOrder = ord + 11;
            }
        }
    }
}
