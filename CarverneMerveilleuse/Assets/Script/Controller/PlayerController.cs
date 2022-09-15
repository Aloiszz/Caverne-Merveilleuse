using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player config")]
    public SO_PlayerController playerSO;
    
    
    [Header("projectiles")]
    
    
    private Rigidbody2D rb;
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
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameMove();
        rb.drag = playerSO.linearDragDeceleration * playerSO.linearDragMultiplier;
    }

    private void Update()
    {
        if (playerSO.life == 0)
        {
            Destroy(gameObject);
        }
        
    }

    private void GameMove()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            //transform.Translate(Vector3.up * speed * Time.deltaTime);
            rb.AddForce(Vector2.up * playerSO.speedMovement);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            //transform.Translate(Vector3.left * speed * Time.deltaTime);
            rb.AddForce(Vector2.left * playerSO.speedMovement);
        }

        if (Input.GetKey(KeyCode.S))
        {
            //transform.Translate(Vector3.down * speed * Time.deltaTime);
            rb.AddForce(Vector2.down * playerSO.speedMovement);
        }

        if (Input.GetKey(KeyCode.D))
        {
            //transform.Translate(Vector3.right * speed * Time.deltaTime);
            rb.AddForce(Vector2.right * playerSO.speedMovement);
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
        if (Input.GetKey(KeyCode.Space))
        {
            
        }
    }
}
