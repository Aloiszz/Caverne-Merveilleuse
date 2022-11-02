using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttackCollision : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Collider2D coll;

    [Header("Singleton")]
    public static HeavyAttackCollision instance;
    
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("CAC") | col.CompareTag("Boss") | col.CompareTag("Dist") | col.CompareTag("Gros"))
        {
            PlayerLightAttack.instance.playerLightAttack.isStriking = true;
            col.GetComponent<Mechant>().ReceiveLightDamage();
        }
    }
}
