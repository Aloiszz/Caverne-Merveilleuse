using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Player config")]
    public SO_PlayerController playerSO;

    [HideInInspector] public Rigidbody2D rb;
    private Collider2D coll;

    [HideInInspector]public List<Vector2> lastMovement;
    private string strMovement;
    
    [Header("Singleton")]
    public static PlayerController instance;

    public GameObject deathBloodPS;
    public GameObject bloodPS;
    public Image healthBar;
    
    public float speedMovement;
    [HideInInspector]public float dashForce;
    [HideInInspector]public float dashReload;
    [HideInInspector]public float dashInvinsibleTime;
    public int life;
    [HideInInspector]public float linearDragDeceleration;
    [HideInInspector]public float linearDragMultiplier;
    public int lifeDepard;
    
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
        playerSO.isDash = true;
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        SecureSO();
        life = lifeDepard;
    }

    public void SecureSO()
    {
        speedMovement = playerSO.speedMovement;
        dashForce = playerSO.dashForce;
        dashReload = playerSO.dashReload;
        dashInvinsibleTime = playerSO.dashInvinsibleTime;
        lifeDepard = playerSO.life;
        linearDragDeceleration = playerSO.linearDragDeceleration;
        linearDragMultiplier = playerSO.linearDragMultiplier;
    }

    
    void FixedUpdate()
    {
        GameMove(); // Physics movements
        
        rb.drag = linearDragDeceleration * linearDragMultiplier;
    }

    private void Update()
    {
        Dash();
        Life();
    }

    private void GameMove()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            //transform.Translate(Vector3.up * speed * Time.deltaTime);
            rb.AddForce(Vector2.up * speedMovement);
            lastMovement.Add(Vector2.up);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            rb.AddForce(Vector2.left * speedMovement);
            lastMovement.Add(Vector2.left);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(Vector2.down * speedMovement);
            lastMovement.Add(Vector2.down);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(Vector2.right * speedMovement);
            lastMovement.Add(Vector2.right);
        }
    }

    

    void Life()
    {
        if (life == 0)
        {
            if (ItemManager.instance.nbVieEnPlus >= 1)
            {
                life = lifeDepard / 2;
                ItemManager.instance.nbVieEnPlus -= 1;
            }
            else
            {
                Destroy(gameObject);
                Instantiate(deathBloodPS, gameObject.transform.position, quaternion.identity); 
            }
        }
    }
    public void LoseLife()
    {
        life -= 1;
        Debug.Log("lose");
        Instantiate(bloodPS, gameObject.transform.position, quaternion.identity);
    }

    private void Dash()
    {
        if (Input.GetKey(KeyCode.Space) && playerSO.isDash)
        {
            rb.AddForce(new Vector2(lastMovement[^1].x, lastMovement[^1].y) * dashForce);
            StartCoroutine(DashReload());
            StartCoroutine(DashInvinsibleTimer());
            lastMovement.Clear();
        }
    }
    IEnumerator DashReload()
    {
        playerSO.isDash = false;
        yield return new WaitForSeconds(dashReload);
        playerSO.isDash = true;
    }
    IEnumerator DashInvinsibleTimer()
    {
        Physics2D.IgnoreLayerCollision(0,6, true);
        Physics2D.IgnoreLayerCollision(0,7, true);
        yield return new WaitForSeconds(dashInvinsibleTime);
        Physics2D.IgnoreLayerCollision(0,6, false);
        Physics2D.IgnoreLayerCollision(0,7, false);
    }

    
    
}
