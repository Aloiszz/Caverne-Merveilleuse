using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjoCollision : MonoBehaviour
{
    public bool isProjoGros;
    private float speed;
    public bool mode2;
    private float initialSpeed;
    private GameObject player;
    [HideInInspector] public GrosEnnemiScript origine;

    private void Update()
    {
        if (mode2)
        {
            speed = initialSpeed / 2;
            StartCoroutine(TimeBeforeDestroy());
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!isProjoGros)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                col.gameObject.GetComponent<Rigidbody2D>().AddForce((col.transform.position - transform.position) * 500);
                Destroy(gameObject);
                PlayerController.instance.LoseLife();
            }
            if (col.gameObject.layer == 4 || col.gameObject.layer == 9)
            {
                Destroy(gameObject);
            }
        }
        else if (!mode2)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                Grossissement(col.gameObject);
                PlayerController.instance.LoseLife();
            }
            else if (col.gameObject.layer == 4 || col.gameObject.layer == 9)
            {
                origine.canShoot = true;
                Destroy(gameObject);
            }
        }
        
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (isProjoGros)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                col.gameObject.GetComponent<PlayerController>().speedMovement = speed;

            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (isProjoGros)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                
                col.gameObject.GetComponent<PlayerController>().speedMovement = initialSpeed;

            }
        }
    }

    public void Grossissement(GameObject player)
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        gameObject.transform.localScale = new Vector3(5, 5);
        initialSpeed = player.gameObject.GetComponent<PlayerController>().speedMovement;
        mode2 = true;
    }
    IEnumerator TimeBeforeDestroy()
    {
        yield return new WaitForSeconds(2f);
        origine.canShoot = true;
        PlayerController.instance.speedMovement = initialSpeed;
        Destroy(gameObject);
    }
}