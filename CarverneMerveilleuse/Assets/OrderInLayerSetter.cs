using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderInLayerSetter : MonoBehaviour
{
    private SpriteRenderer sprite;
    private int ord;
    public float baseObjet;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        ord = sprite.sortingOrder;
    }

    private void Update()
    {
        if (transform.position.y-baseObjet >= PlayerController.instance.transform.position.y)
        {
            sprite.sortingOrder = ord;
        }
        else
        {
            sprite.sortingOrder = ord + 11;
        }
    }
}
