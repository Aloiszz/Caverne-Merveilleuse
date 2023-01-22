using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatCircleMerchantWay : MonoBehaviour
{
    public static ChatCircleMerchantWay instance;

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

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            PlayerController.instance.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            RoomManager.instance.isShopRoom = true;
            
            GetComponent<Collider2D>().enabled = false;
            //GetComponent<SpriteRenderer>().enabled = false;
            
            ChatMarchand.instance.isStillActive = true;
            
            ChatMarchand.instance.Shop();
            
        }
    }
}
