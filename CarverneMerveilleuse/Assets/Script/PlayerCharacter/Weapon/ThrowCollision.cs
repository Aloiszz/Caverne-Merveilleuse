using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core.Easing;

public class ThrowCollision : MonoBehaviour
{
    
    public SpriteRenderer sprite;
    public Collider2D coll;
    public Rigidbody2D rb;
    [HideInInspector] public int bounceInt = 2;

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
    
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("CAC") | col.CompareTag("Boss") | col.CompareTag("Dist") | col.CompareTag("Gros"))
        {
            PlayerLightAttack.instance.playerLightAttack.isStriking = true;
            
            //col.GetComponent<Mechant>().ReceiveThrowDamage;     A Faire !!!
            if (ItemManager.instance.isExplosfALFGet)
            {
                StartCoroutine(ExplosionColl());
            }
        }

        if (col.gameObject.layer == 9)
        {
            BounceWeapon();
        }
    }
    
    public void ThrowWeapon()
    {
        rb.AddForce((PlayerAttackCollision.instance.difference) * 20, ForceMode2D.Impulse);

        gameObject.transform.DORotate(new Vector3(0, 0, 2160), 3);
    }

    void BounceWeapon()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;
        rb.AddForce((PlayerThrowAttack.instance.points[bounceInt]), ForceMode2D.Impulse);
        bounceInt++;

        /*for (int i = 2; i < PlayerThrowAttack.instance.points.Count;)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0;
            rb.AddForce((PlayerThrowAttack.instance.points[i]) * 120, ForceMode2D.Impulse);
            i++;
        }*/
    }

    IEnumerator ExplosionColl()
    {
        GameObject expColl = Instantiate(ItemManager.instance.explosion, transform.position, Quaternion.identity);
        expColl.transform.localScale =new Vector2(ItemManager.instance.tailleExplosion, ItemManager.instance.tailleExplosion);
        yield return new WaitForSeconds(0.1f);
        Destroy(expColl);
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
