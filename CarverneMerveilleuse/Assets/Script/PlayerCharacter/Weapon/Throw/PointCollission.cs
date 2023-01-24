using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class PointCollission : MonoBehaviour
{
    public Collider2D coll;
    public Rigidbody2D rb;
    public int bounceInt = 1;

    private int randAudio;
    public static PointCollission instance;
    
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
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        
        coll.enabled = false;
    }

    private void Update()
    {
        if (verif)
        {
            BounceWeapon();
        }

        if (transform.position == PlayerThrowAttack.instance.points[^1])
        {
            verif = false;
        }

    }

    private bool verif;
    [HideInInspector] public bool verifPremierTouch;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 9)
        {
            verif = true;
            verifPremierTouch = true;
            bounceInt++;
            PlayerThrowAttack.instance.multiplicateur++;

            Instantiate(PlayerThrowAttack.instance.PS_eclatDeFaux, transform.position, quaternion.identity,
                RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].transform);

            randAudio = Random.Range(0, PlayerThrowAttack.instance.audioRebond.Count);
            PlayerController.instance.Source.PlayOneShot(PlayerThrowAttack.instance.audioRebond[randAudio], 0.15f);
        }
        
    }
    
    void BounceWeapon()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;
        //rb.AddForce((PlayerThrowAttack.instance.points[bounceInt]), ForceMode2D.Impulse);
        
        transform.position = Vector3.MoveTowards(transform.position,
            PlayerThrowAttack.instance.points[bounceInt], Time.deltaTime * 
                                                          PlayerThrowAttack.instance.ThrowSpeed[PlayerThrowAttack.instance.ThrowSpeedIndex] );
    }
    
    public void ThrowWeapon()
    {
        //rb.AddForce((PlayerAttackCollision.instance.difference) * 20, ForceMode2D.Impulse);
        
        
        transform.position = Vector3.MoveTowards(transform.position,
            PlayerThrowAttack.instance.points[1], Time.deltaTime * 
                                                  PlayerThrowAttack.instance.ThrowSpeed[PlayerThrowAttack.instance.ThrowSpeedIndex]);
        
        /*transform.position = Vector3.MoveTowards(transform.position,
            PlayerAttackCollision.instance.difference, Time.deltaTime * 40);*/
        
        //gameObject.transform.DORotate(new Vector3(0, 0, 2160), 3);
    }
    
    public void IsWeaponActive(bool verif)
    {
        if (verif)
        {
            coll.enabled = true;
        }
        else
        {
            coll.enabled = false;
        }
    }
}
