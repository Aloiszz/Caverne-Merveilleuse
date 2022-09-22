using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;

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
        playerSO.life = 4;
        playerSO.isDash = true;
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameMove();
        rb.drag = playerSO.linearDragDeceleration * playerSO.linearDragMultiplier;
    }

    private void Update()
    {
        Dash();
        Attack();
        if (playerSO.life == 0)
        {
            Destroy(gameObject);
            Instantiate(deathBloodPS, gameObject.transform.position, quaternion.identity);
        }
    }

    private void GameMove()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            //transform.Translate(Vector3.up * speed * Time.deltaTime);
            rb.AddForce(Vector2.up * playerSO.speedMovement);
            lastMovement.Add(Vector2.up);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            //transform.Translate(Vector3.left * speed * Time.deltaTime);
            rb.AddForce(Vector2.left * playerSO.speedMovement);
            lastMovement.Add(Vector2.left);
        }

        if (Input.GetKey(KeyCode.S))
        {
            //transform.Translate(Vector3.down * speed * Time.deltaTime);
            rb.AddForce(Vector2.down * playerSO.speedMovement);
            lastMovement.Add(Vector2.down);
        }

        if (Input.GetKey(KeyCode.D))
        {
            //transform.Translate(Vector3.right * speed * Time.deltaTime);
            rb.AddForce(Vector2.right * playerSO.speedMovement);
            lastMovement.Add(Vector2.right);
        }
    }

    private void Shoot()
    {
        
    }
    public void LoseLife()
    {
        playerSO.life -= 1;
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


    private void Attack()
    {
        if (!playerSO.isCoolDown)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(LightCloseDamageCoolDown());
            }
        }
        if (playerSO.isStriking)
        {
            playerSO.isStriking = false;
            CinemachineShake.instance.ShakeCamera(playerSO.intensityLightCloseDamage, playerSO.frequencyLightCloseDamage ,playerSO.timerLightCloseDamage);
        }
    }

    IEnumerator LightCloseDamageCoolDown()
    {
        PlayerAttackCollision.instance.sprite.enabled = true;
        PlayerAttackCollision.instance.coll.enabled = true;
        playerSO.isCoolDown = true;
        yield return new WaitForSeconds(playerSO.lightCloseDamageCoolDown);
        playerSO.isCoolDown = false;
        PlayerAttackCollision.instance.sprite.enabled = false;
        PlayerAttackCollision.instance.coll.enabled = false;
    }
}
