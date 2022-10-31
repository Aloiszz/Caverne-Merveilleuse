using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ThrowCollision : MonoBehaviour
{
    
    public SpriteRenderer sprite;
    public Collider2D coll;
    public Rigidbody2D rb;
    

    public static ThrowCollision instance;
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
    
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        
        sprite.enabled = false;
        coll.enabled = false;
    }

    public void ThrowWeapon()
    {
        rb.AddForce((PlayerAttackCollision.instance.difference), ForceMode2D.Force);
        gameObject.transform.DORotate(new Vector3(0, 0, 2160), 3);
    }
    
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("CAC") | col.CompareTag("Boss") | col.CompareTag("Dist") | col.CompareTag("Gros"))
        {
            PlayerLightAttack.instance.playerLightAttack.isStriking = true;
            
            //col.GetComponent<Mechant>().ReceiveThrowDamage;     A Faire !!!
        }
    }


    public void IsWeaponActive(bool verif)
    {
        if (verif)
        {
            sprite.enabled = true;
            coll.enabled = true;
        }
        else
        {
            sprite.enabled = false;
            coll.enabled = false;
        }
    }
}
