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
    [HideInInspector]public int life;
    [HideInInspector]public float linearDragDeceleration;
    [HideInInspector]public float linearDragMultiplier;
    [HideInInspector] public int lifeDepard;

    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject graphFace;
    [SerializeField] private GameObject graphProfile;
    [SerializeField] private GameObject graphDos;

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

    public void SecureSO()
    {
        speedMovement = playerSO.speedMovement;
        dashForce = playerSO.dashForce;
        dashReload = playerSO.dashReload;
        dashInvinsibleTime = playerSO.dashInvinsibleTime;
        life = playerSO.life;
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
        //rb.velocity = new Vector2((speedMovement), rb.velocity.y);
        
          Debug.Log(rb.velocity.x);

          if (rb.velocity.x > 0.03f || rb.velocity.x < -0.03f)
          {
              graphFace.SetActive(false);
              graphDos.SetActive(false);
              graphProfile.SetActive(true);
              animator.SetBool("isProfileRunning", true);
          }
          else
          {
              animator.SetBool("isProfileRunning", false);
          }

          if (!animator.GetBool("isProfileRunning") && !animator.GetBool("Profile"))
          {
              graphProfile.SetActive(false);
          }

          animator.SetFloat("speedY", rb.velocity.y);
        
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
            //animator.Play("Dos_Idle");
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //animator.Play("profil idle");
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            //animator.Play("Face_run");
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            //animator.Play("profil idle");
        }

        if (Input.GetKey(KeyCode.Q)) graphProfile.transform.localScale = new Vector3(-1,1,1);
        if (Input.GetKey(KeyCode.D)) graphProfile.transform.localScale = new Vector3(1,1,1);
    }

    private void GameMove()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            //transform.Translate(Vector3.up * speed * Time.deltaTime);
            rb.AddForce(Vector2.up * speedMovement);
            lastMovement.Add(Vector2.up);
            graphFace.SetActive(false);
            graphDos.SetActive(true);
            animator.SetBool("Face", false);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            //transform.Translate(Vector3.left * speed * Time.deltaTime);
            rb.AddForce(Vector2.left * speedMovement);
            lastMovement.Add(Vector2.left);
            animator.SetBool("Profile", true);
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            //transform.Translate(Vector3.down * speed * Time.deltaTime);
            rb.AddForce(Vector2.down * speedMovement);
            lastMovement.Add(Vector2.down);
            graphFace.SetActive(true);
            graphDos.SetActive(false);
            animator.SetBool("Face", true);
        }

        if (Input.GetKey(KeyCode.D))
        {
            //transform.Translate(Vector3.right * speed * Time.deltaTime);
            rb.AddForce(Vector2.right * speedMovement);
            lastMovement.Add(Vector2.right);
            animator.SetBool("Profile", true);
        }

        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.S))
        {
            if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.Q))
            {
                animator.SetBool("Profile", false);
            }
        }
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
