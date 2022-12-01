using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using Unity.VisualScripting;
using Random = UnityEngine.Random;


public class ThrowCollision : MonoBehaviour
{
    
    public SpriteRenderer sprite;
    public Collider2D coll;
    public Rigidbody2D rb;
    public GameObject bloodPS;

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
        
        if (PlayerThrowAttack.instance.is_F_Pressed)
        {
            laFaux.transform.Rotate (0, 0, 1050*Time.deltaTime);
        }
        else
        {
            if (PlayerThrowAttack.instance.points[PointCollission.instance.bounceInt] == PlayerThrowAttack.instance.points[^1])
            {
                laFaux.transform.Rotate (0, 0, 0);
            }
            else
            {
                laFaux.transform.Rotate (0, 0, -1050*Time.deltaTime);
            }
        }
        
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("CAC") | col.CompareTag("Boss") | col.CompareTag("Dist") | col.CompareTag("Gros"))
        {
            PlayerLightAttack.instance.playerLightAttack.isStriking = true;
            
            col.GetComponent<Mechant>().ReceiveThrowDamage();
            int rand = Random.Range(1, 3);
            for (int i = 0; i < rand; i++)
            {
                Instantiate(bloodPS, col.transform.position, Quaternion.identity, RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].transform);
            }
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
            //sprite.enabled = true;
            coll.enabled = true;
        }
        else
        {
            //sprite.enabled = false;
            coll.enabled = false;
        }
    }
}
