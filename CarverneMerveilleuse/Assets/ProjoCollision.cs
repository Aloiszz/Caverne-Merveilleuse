using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjoCollision : MonoBehaviour
{
    public bool isProjoGros;
    private float speed;
    private bool mode2;
    private float initialSpeed;

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
            else if (!col.gameObject.CompareTag("CAC") | !col.gameObject.CompareTag("Boss") | !col.gameObject.CompareTag("Dist") | !col.gameObject.CompareTag("Gros"))
            {
                Destroy(gameObject);
            }
        }
        else if (!mode2)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                gameObject.transform.localScale = new Vector3(5, 5);
                initialSpeed = col.gameObject.GetComponent<PlayerController>().speedMovement;
                mode2 = true;
                PlayerController.instance.LoseLife();
            }
            else if (!col.gameObject.CompareTag("CAC") | !col.gameObject.CompareTag("Boss") | !col.gameObject.CompareTag("Dist") | !col.gameObject.CompareTag("Gros"))
            {
                Destroy(gameObject);
            }
        }
        
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerController>().speedMovement = speed;
            
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerController>().speedMovement = initialSpeed;
            
        }
    }

    IEnumerator TimeBeforeDestroy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}