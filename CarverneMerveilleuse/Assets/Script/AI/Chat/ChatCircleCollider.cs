using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatCircleCollider : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.CompareTag("Player"))
        {
            ChatMarchand.instance.see = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        //ChatMarchand chat = col.GetComponent<ChatMarchand>();
        if (col.CompareTag("Player"))
        {
            ChatMarchand.instance.see = false;
            ChatMarchand.instance.isStillActive = false;
            //chat.see = false;
        }
    }
}
