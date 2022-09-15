using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mechant : MonoBehaviour
{
    public GameObject player;
    public float speed;

    public bool see = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Attack();   
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        //PlayerController.instance.LoseLife();   
        see = true;
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        //PlayerController.instance.LoseLife();   
        see = false;
    }

    private void Attack()
    {
        if (see)
        {
            gameObject.transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }
    
}
