using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using Unity.VisualScripting;

public class ThrowCollision : MonoBehaviour
{
    
    public SpriteRenderer sprite;
    public Collider2D coll;
    public Rigidbody2D rb;

    public GameObject laFaux;

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
        //rb = GetComponent<Rigidbody2D>();
        
        sprite.enabled = false;
        coll.enabled = false;
        laFaux.SetActive(false);
    }

    private void Update()
    {
        if (PlayerThrowAttack.instance.points[PointCollission.instance.bounceInt] == PlayerThrowAttack.instance.points[^1])
        {
            Debug.Log("ici");
            laFaux.transform.Rotate (0, 0, 0);
        }
        else
        {
            Debug.Log("par la");
            laFaux.transform.Rotate (0, 0, 1050*Time.deltaTime);
        }

        if (PlayerThrowAttack.instance.is_F_Pressed)
        {
            laFaux.transform.Rotate (0, 0, -1050*Time.deltaTime);
        }
        
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("CAC") | col.CompareTag("Boss") | col.CompareTag("Dist") | col.CompareTag("Gros"))
        {
            PlayerLightAttack.instance.playerLightAttack.isStriking = true;
            
            col.GetComponent<Mechant>().ReceiveThrowDamage();
    // Mettre pour les degat  ==>  PlayerThrowAttack.instance.ThrowDamage[PlayerThrowAttack.instance.ThrowDamageIndex];
            if (ItemManager.instance.isExplosfALFGet)
            {
                StartCoroutine(ExplosionColl());
            }
        }

        if (col.CompareTag("Player"))
        {
            if(PointCollission.instance.bounceInt > 1) PlayerThrowAttack.instance.ReturnWeapon();
        }
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
