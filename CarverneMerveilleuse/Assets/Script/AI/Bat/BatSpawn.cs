using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BatSpawn : MonoBehaviour
{

    private Collider2D coll;
    private Rigidbody2D rb;
    //private Bat anim;
    private Mechant mechant;
    private DistScript ennemi;
    public GameObject sprite;
    public GameObject healthBar;
    public GameObject eye1;
    public GameObject BGHealthBar;
    public GameObject shadow;

    void Start()
    {

        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Cac_Anim>();
        mechant = GetComponent<Mechant>();
        ennemi = GetComponent<DistScript>();
        
        coll.enabled = false;
        rb.simulated = false;
        //anim.enabled = false;
        mechant.enabled = false;
        ennemi.enabled = false;
        
        BGHealthBar.SetActive(false);
        shadow.SetActive(false);
        sprite.SetActive(false);
        healthBar.SetActive(false);
        eye1.SetActive(false);

        Invoke("Activate", EnnemyManager.instance.timeToAppearEnnemy);
    }

    void Activate()
    {
        coll.enabled = true;
        rb.simulated = true;
        //anim.enabled = true;
        mechant.enabled = true;
        ennemi.enabled = true;
        
        BGHealthBar.SetActive(true);
        shadow.SetActive(true);
        sprite.SetActive(true);
        healthBar.SetActive(true);
        eye1.SetActive(true);
        AudioManager.instance.onePlay = false;
    }
    
    void Update()
    {
        
    }
}