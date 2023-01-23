using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpwanCac : MonoBehaviour
{

    private Collider2D coll;
    private Rigidbody2D rb;
    private Cac_Anim anim;
    private Mechant mechant;
    private CaCEnnemiScript ennemi;
    public GameObject sprite;
    public GameObject healthBar;
    public GameObject eye1;
    public GameObject eye2;
    public GameObject BGHealthBar;
    public GameObject shadow;
    

    void Start()
    {

        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Cac_Anim>();
        mechant = GetComponent<Mechant>();
        ennemi = GetComponent<CaCEnnemiScript>();
        
        coll.enabled = false;
        rb.simulated = false;
        anim.enabled = false;
        mechant.enabled = false;
        ennemi.enabled = false;
        
        BGHealthBar.SetActive(false);
        shadow.SetActive(false);
        sprite.SetActive(false);
        healthBar.SetActive(false);
        eye1.SetActive(false);
        eye2.SetActive(false);
        
        Invoke("Activate", EnnemyManager.instance.timeToAppearEnnemy);
    }

    void Activate()
    {
        BGHealthBar.SetActive(true);
        shadow.SetActive(true);
        coll.enabled = true;
        rb.simulated = true;
        anim.enabled = true;
        mechant.enabled = true;
        ennemi.enabled = true;
        sprite.SetActive(true);
        healthBar.SetActive(true);
        eye1.SetActive(true);
        eye2.SetActive(true);
        AudioManager.instance.onePlay = false;
    }
    
    void Update()
    {
        
    }
}
