using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjoCollision : MonoBehaviour
{
    public bool isProjoGros;
    
    public bool mode2;
    private float initialSpeed ;
    private float speed;
    private GameObject player;
    private GameObject newProjo;
    private Vector2 playerDir;
    [HideInInspector] public GrosEnnemiScript origine;
    [HideInInspector] public GameObject shooter;
    private bool check;
    public GameObject projo;

    private void Start()
    {
        initialSpeed = PlayerController.instance.playerSO.speedMovement;
        speed = initialSpeed / 2;
    }

    private void Update()
    {
        if (mode2)
        {
            if(check)
            {
                StartCoroutine(NewProjoOrDestroy());
                check = false;
            }
        }

        if (shooter.activeInHierarchy == false)
        {
            if (isProjoGros)
            {
                origine.projoList.Clear();
                origine.canShoot = true;
            }
            Destroy(gameObject);   
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
                for (int i = 0; i < origine.projoList.Count; i++)
                {
                    Destroy(origine.projoList[i]);
                }
                origine.projoList.Clear();
                origine.canShoot = true;
                PlayerController.instance.speedMovement = initialSpeed;
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
                PlayerController.instance.speedMovement = speed;
                PlayerThrowAttack.instance.isInGrosProjo = true;
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
                PlayerThrowAttack.instance.isInGrosProjo = false;

            }
        }
    }

    public void Grossissement(GameObject player)
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        gameObject.transform.localScale = new Vector3(5, 5);
        mode2 = true;
        check = true;
    }
    IEnumerator NewProjoOrDestroy()
    {
        yield return new WaitForSeconds(origine.TimeBeforeLinkedShoot);
        if (origine.projoList.Count < origine.maxProjo)
        {
            newProjo = Instantiate(projo, transform.position, Quaternion.identity);
            newProjo.GetComponent<ProjoCollision>().mode2 = false;
            newProjo.transform.localScale = new Vector2(1, 1);
            origine.projoList.Add(newProjo);
            newProjo.GetComponent<Rigidbody2D>().velocity = playerDir.normalized * origine.grosForce;
            newProjo.GetComponent<ProjoCollision>().origine = origine;
            newProjo.GetComponent<ProjoCollision>().shooter = shooter;
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