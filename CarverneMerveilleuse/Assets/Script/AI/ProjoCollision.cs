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
    private GameObject newProjo;
    private Vector2 playerDir;
    [HideInInspector] public GrosEnnemiScript origine;
    private bool check;
    public GameObject projo;
    

    private void Update()
    {
        if (mode2)
        {
            speed = initialSpeed / 2;
            if(check)
            {
                StartCoroutine(TimeBeforeDestroy());
                check = false;
            }
        }
        playerDir = PlayerController.instance.transform.position - transform.position;
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
        check = true;
    }
    IEnumerator TimeBeforeDestroy()
    {
        yield return new WaitForSeconds(2f);
        if (origine.projoList.Count < 3)
        {
            newProjo = Instantiate(projo, transform.position, Quaternion.identity);
            newProjo.GetComponent<ProjoCollision>().mode2 = false;
            newProjo.transform.localScale = new Vector2(1, 1);
            origine.projoList.Add(newProjo);
            newProjo.GetComponent<Rigidbody2D>().velocity = playerDir.normalized * origine.grosForce;
            newProjo.GetComponent<ProjoCollision>().origine = origine;
            yield return new WaitUntil(() => (newProjo.transform.position - transform.position).magnitude >= origine.maxRangeProjo);
            newProjo.GetComponent<ProjoCollision>().Grossissement(PlayerController.instance.gameObject);
        }
        else
        {
            for (int i = 0; i < origine.projoList.Count; i++)
            {
                Destroy(origine.projoList[i]);
            }
            origine.projoList.Clear();
            origine.canShoot = true;
            PlayerController.instance.speedMovement = initialSpeed;

        }
    }
}