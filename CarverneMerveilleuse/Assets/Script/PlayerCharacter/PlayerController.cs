using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    
    [HideInInspector] public Rigidbody2D rb;
    private Collider2D coll;

    [HideInInspector]public List<Vector2> lastMovement;
    private string strMovement;

    public GameObject dashTrail;
    
    
    [Header("Singleton")]
    public static PlayerController instance;

    public GameObject deathBloodPS;
    public GameObject bloodPS;
    public Image healthBar;
    
    [Header("Player config")]
    public SO_PlayerController playerSO;
    public float speedMovement;
    public float dashForce;
    [HideInInspector]public float dashReload;
    [HideInInspector]public float dashInvinsibleTime;
    public int life;
    [HideInInspector]public float linearDragDeceleration;
    [HideInInspector]public float linearDragMultiplier;
    public int lifeDepard;
    public bool isDashing;
    [HideInInspector] public int nbPossibleDash = 1;
    [HideInInspector] public bool isInHole;
    [HideInInspector] public Vector2 actualVelocity;
    
    [Header("Camera Zoom")] 
    public float zoomSize;
    public float timeToArrive;
    public float timeToComeBack;
    
    [Header("Audio")] 
    public AudioSource Source;
    [Space]
    public AudioClip audioWalk;
    public AudioClip audioRage;
    public AudioClip audioRageHeart;
    
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
        
        
        dashTrail.SetActive(false);
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
        Life();
        Dash();
        Rage();
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
        if (life <= 0)
        {
            if (ItemManager.instance.nbVieEnPlus >= 1)
            {
                life = lifeDepard / 2;
                ItemManager.instance.nbVieEnPlus -= 1;
            }
            else
            {
                if (Score.instance.activateScore)
                {
                    Score.instance.AddScore();
                }
                SceneManager.instance.Death();
                /*PlayerController.instance.enabled = false;
                PlayerLightAttack.instance.enabled = false;
                PlayerHeavyAttack.instance.enabled = false;
                PlayerThrowAttack.instance.enabled = false;*/
                //Destroy(gameObject);
                gameObject.SetActive(false);
                Instantiate(deathBloodPS, gameObject.transform.position, quaternion.identity,
                    RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].transform);
                GameObject.FindGameObjectWithTag("Respawn").GetComponent<CinemachineVirtualCamera>().Follow = null;
            }
        }
    }
    public void LoseLife(int damage)
    {
        life -= damage;
        StartCoroutine(InvinsibleTime());
        Instantiate(bloodPS, gameObject.transform.position, quaternion.identity,
            RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].transform);
        
        CinemachineCameraZoom.instance.CameraZoom(zoomSize, timeToArrive, timeToComeBack);
    }

    private void Dash()
    {
        if (Input.GetButtonDown("Dash") && playerSO.isDash && !isInHole)
        {
            rb.AddForce(new Vector2(lastMovement[^1].x, lastMovement[^1].y) * dashForce);
            StartCoroutine(DashReload());
            StartCoroutine(DashInvinsibleTimer());
            StartCoroutine(PetrolDash());
            lastMovement.Clear();
        }
    }
    IEnumerator DashReload()
    {
        nbPossibleDash -= 1;
        if (nbPossibleDash == 0)
        {
            playerSO.isDash = false;
        }
        yield return new WaitForSeconds(dashReload);
        nbPossibleDash += 1;
        playerSO.isDash = true;
    }
    IEnumerator DashInvinsibleTimer()
    {
        isDashing = true;
        if (!ItemManager.instance.isPushDashGet && !ItemManager.instance.isDegatDashGet)
        {
            Physics2D.IgnoreLayerCollision(0,6, true);
            Physics2D.IgnoreLayerCollision(0,7, true);
        }
        dashTrail.SetActive(true);
        yield return new WaitForSeconds(dashInvinsibleTime);
        Physics2D.IgnoreLayerCollision(0,6, false);
        Physics2D.IgnoreLayerCollision(0,7, false);
        if (!isInHole)
        {
            isDashing = false;
            dashTrail.SetActive(false);
        }
        
    }

    [HideInInspector]public bool isPlayed;
    public void Rage()
    {
        
        if (LifeManager.instance.isInRage)
        {
            if (PlayerThrowAttack.instance.isThrow)
            {
                PlayerThrowAttack.instance.ReturnWeaponImmediate();
            }

            if (!isPlayed)
            {
                Source.PlayOneShot(audioRage, 1);
                StartCoroutine(HeartBeat());
                isPlayed = true;
            }
            
            
            speedMovement = 110;
            dashForce = 5000;
            dashReload = .8f;
            //linearDragDeceleration = 4.2f;
            
            PlayerLightAttack.instance.coolDownEndComboIndex = 1;
        }
        else
        {
            PlayerLightAttack.instance.coolDownEndComboIndex = 0;
            SecureSO();
            isPlayed = false;
        }
    }

    IEnumerator HeartBeat()
    {
        for (int i = 0; i < 5; i++)
        {
            Source.PlayOneShot(audioRageHeart, 0.5f);
            yield return new WaitForSeconds(1);
        }
        
    }

    IEnumerator InvinsibleTime()
    {
        Physics2D.IgnoreLayerCollision(0,6, true);
        Physics2D.IgnoreLayerCollision(0,7, true);
        yield return new WaitForSeconds(playerSO.invinsibleTimer);
        Physics2D.IgnoreLayerCollision(0,6, false);
        Physics2D.IgnoreLayerCollision(0,7, false);
    }
    IEnumerator PetrolDash()
    {
        
        if (ItemManager.instance.isPetroleDashGet)
        {
            List<GameObject> petrole = new List<GameObject>();
            for (int i = 0; i < ItemManager.instance.nbTachePetrole; i++)
            {
                petrole.Add(Instantiate(ItemManager.instance.petrole, transform.position, Quaternion.identity));
                yield return new WaitForSeconds(dashInvinsibleTime / ItemManager.instance.nbTachePetrole);
            }

            yield return new WaitForSeconds(ItemManager.instance.secondAvantDisparitionPetrole);
            for (int i = 0; i < petrole.Count; i++)
            {
                Destroy(petrole[i]);
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == 9 && isInHole)
        {
            actualVelocity = -actualVelocity;
        }
    }
}
