using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }

        foreach (var str in lastMovement)
        {
            Debug.Log(str);
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
        if (Input.GetMouseButtonDown(0))
        {
            playerSO.isStriking = true;
            StartCoroutine(AttackTime());
        }
        playerSO.isStriking = false;
    }

    IEnumerator AttackTime()
    {
        PlayerAttackCollision.instance.sprite.enabled = true;
        PlayerAttackCollision.instance.coll.enabled = true;
        yield return new WaitForSeconds(0.2f);
        PlayerAttackCollision.instance.sprite.enabled = false;
        PlayerAttackCollision.instance.coll.enabled = false;
    }
}
