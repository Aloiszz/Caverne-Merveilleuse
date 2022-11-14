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

    [Header("Animator")]
    [SerializeField] private List<Animator>  animator;
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
        foreach (var i in animator)
        {
            i.SetFloat("speedX", rb.velocity.x);
            i.SetFloat("speedY", rb.velocity.y);
            
            if (rb.velocity.x > 0.3f || rb.velocity.x < -0.3f)
            {
                i.SetBool("isProfileRunning", true);
            }
            else
            {
                i.SetBool("isProfileRunning", false);
            }
        
            if (rb.velocity.y > 0.3f || rb.velocity.y < -0.3f)
            {
                i.SetBool("isFaceRunning", true);
            }
            else
            {
                i.SetBool("isFaceRunning", false);
            }
        }
        

        Dash();
        Life();
        GameAnimatinon();


        if (Input.GetKey(KeyCode.Q)) graphProfile.transform.localScale = new Vector3(-0.08f,0.08f,0.08f);
        if (Input.GetKey(KeyCode.D)) graphProfile.transform.localScale = new Vector3(0.08f,0.08f,0.08f);
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

    private void GameAnimatinon()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            graphFace.SetActive(false);
            graphDos.SetActive(true);
            graphProfile.SetActive(false);

            animator[2].SetBool("Face", false);
            animator[2].SetBool("Profile", false);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            graphFace.SetActive(false);
            graphDos.SetActive(false);
            graphProfile.SetActive(true);
            
            animator[1].SetBool("Face", false);
            animator[1].SetBool("Profile", true);
        }
        if (Input.GetKey(KeyCode.S))
        {
            graphFace.SetActive(true);
            graphDos.SetActive(false);
            graphProfile.SetActive(false);

            animator[0].SetBool("Face", true);
            animator[0].SetBool("Profile", false);
        }
        if (Input.GetKey(KeyCode.D))
        {
            graphFace.SetActive(false);
            graphDos.SetActive(false);
            graphProfile.SetActive(true);

            animator[1].SetBool("Face", false);
            animator[1].SetBool("Profile", true);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator[0].SetBool("isLightAttacking", true);
        }
        
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            animator[0].SetBool("isLightAttacking", false);
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
                //Destroy(gameObject);
                animator[0].SetBool("isFaceDeath", true);
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
