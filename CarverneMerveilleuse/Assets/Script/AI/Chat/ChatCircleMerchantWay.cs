using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatCircleMerchantWay : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            RoomManager.instance.isShopRoom = true;
            
            //GetComponent<Collider2D>().enabled = false;
            //GetComponent<SpriteRenderer>().enabled = false;
            
            ChatMarchand.instance.isStillActive = true;
            
            ChatMarchand.instance.Shop();
            
        }
    }
}
