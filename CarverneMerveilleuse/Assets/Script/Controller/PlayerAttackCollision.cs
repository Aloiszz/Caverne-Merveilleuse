using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollision : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Collider2D coll;
    public GameObject pivot;
    
    [Header("Singleton")]
    public static PlayerAttackCollision instance;
    
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
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
        
        sprite.enabled = true;
        coll.enabled = false;
    }

    private void FixedUpdate()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        pivot.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
 
        if (rotationZ < -90 || rotationZ > 90)
        {
            if(PlayerController.instance.gameObject.transform.eulerAngles.y == 0)
            {
                pivot.transform.localRotation = Quaternion.Euler(180, 0, -rotationZ);
            } 
            /*else if (PlayerController.instance.gameObject.transform.eulerAngles.y == 180) 
            {
                pivot.transform.localRotation = Quaternion.Euler(180, 180, -rotationZ);
            }*/
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.playerSO.isStriking)
        {
            
        }
    }
}
