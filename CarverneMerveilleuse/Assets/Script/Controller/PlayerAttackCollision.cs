using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollision : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Collider2D coll;
    
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
        
        sprite.enabled = false;
        coll.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.playerSO.isStriking)
        {
            //
        }
    }
}
