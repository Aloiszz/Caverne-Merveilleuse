using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Player config")]
    public SO_PlayerController playerSO;

    private Rigidbody2D rb;
    private Collider2D coll;

    public List<Vector2> lastMovement;
    private string strMovement;
    
    [Header("Singleton")]
    public static PlayerController instance;

    public GameObject deathBloodPS;
    public GameObject bloodPS;
    public Image healthBar;
    public float speedMovement;
    private float dashForce;
    public float dashReload;
    public float dashInvinsibleTime;
    public int life;
    public float linearDragDeceleration;
    public float linearDragMultiplier;
    [HideInInspector] public int lifeDepard;

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
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
        lifeDepard = life;
    }

    void SecureSO()
    {
        speedMovement = playerSO.speedMovement;
        dashForce = playerSO.dashForce;
        dashReload = playerSO.dashReload;
        dashInvinsibleTime = playerSO.dashInvinsibleTime;
        life = playerSO.life;
        linearDragDeceleration = playerSO.linearDragDeceleration;
        linearDragMultiplier = playerSO.linearDragMultiplier;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameMove(); // Physics movements
        
        rb.drag = playerSO.linearDragDeceleration * playerSO.linearDragMultiplier;
    }

    private void Update()
    {
        /*animator.SetFloat("speedX", rb.velocity.x);
        animator.SetFloat("speedY", rb.velocity.y);*/

        if (life > lifeDepard)
        {
            life = lifeDepard;
        }
        Dash();

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
        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.Play("Dos_Idle");
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.Play("profil idle");
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.Play("Face_run");
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            animator.Play("profil idle");
        }

        if (Input.GetKey(KeyCode.Q)) spriteRenderer.flipX = true;
        else if (Input.GetKey(KeyCode.Q)) spriteRenderer.flipX = true;
        
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
            //transform.Translate(Vector3.left * speed * Time.deltaTime);
            rb.AddForce(Vector2.left * speedMovement);
            lastMovement.Add(Vector2.left);
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            //transform.Translate(Vector3.down * speed * Time.deltaTime);
            rb.AddForce(Vector2.down * speedMovement);
            lastMovement.Add(Vector2.down);
        }

        if (Input.GetKey(KeyCode.D))
        {
            //transform.Translate(Vector3.right * speed * Time.deltaTime);
            rb.AddForce(Vector2.right * speedMovement);
            lastMovement.Add(Vector2.right);
        }
    }

    private void Shoot()
    {
        
    }
    public void LoseLife()
    {
        life -= 1;
        Instantiate(bloodPS, gameObject.transform.position, quaternion.identity);
    }

    private void Dash()
    {
        if (Input.GetKey(KeyCode.Space) && playerSO.isDash)
        {
            rb.AddForce(new Vector2(lastMovement[^1].x, lastMovement[^1].y) * playerSO.dashForce);
            StartCoroutine(DashReload());
            StartCoroutine(DashInvinsibleTimer());
            lastMovement.Clear();
        }
    }
    IEnumerator DashReload()
    {
        playerSO.isDash = false;
        yield return new WaitForSeconds(playerSO.dashReload);
        playerSO.isDash = true;
    }
    IEnumerator DashInvinsibleTimer()
    {
        coll.enabled = false;
        yield return new WaitForSeconds(playerSO.dashInvinsibleTime);
        coll.enabled = true;
    }
    
}
