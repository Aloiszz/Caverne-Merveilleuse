using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatMarchand : MonoBehaviour
{
    public PlayerController player;
    public float speed;
    public MerchantSecretDoor merchantRoom;

    public bool see;
    public static ChatMarchand instance;

    // Start is called before the first frame update
    
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
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        merchantRoom = GameObject.FindGameObjectWithTag("MerchantSecretDoor").GetComponent<MerchantSecretDoor>();
    }

    // Update is called once per frame
    void Update()
    {
        FindMerchantRoom();   
    }
    

    private void FindMerchantRoom()
    {
        if (see)
        {
            //gameObject.transform.position = Vector3.MoveTowards(transform.position, merchantRoom.transform.position, speed * Time.deltaTime);
        }
        else
        {
            gameObject.transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            //Generate Shop Room
        }
    }
}
