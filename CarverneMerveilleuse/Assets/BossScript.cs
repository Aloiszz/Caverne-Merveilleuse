using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    [Header("Boss Stats")] 
    public float BossPVStart = 10;
    public float BossPV;
    
    [Header("Attaques Phase 2")] 
    public GameObject vague1;
    public GameObject vague2;
    public GameObject zone;
    public GameObject chute;
    public float vaguesSpeed;

    [Header("Info")] 
    public GameObject player;

    private Vector2 vague1Pos;
    private Vector2 vague2Pos;
    private bool canAttack = true;
    private bool isCAC = false;

    private void Start()
    {
        BossPV = BossPVStart;
        vague1Pos = vague1.transform.position;
        vague2Pos = vague2.transform.position;
    }

    void Update()
    {
        if (BossPV <= BossPVStart / 2 && canAttack && !isCAC)
        {
            StartCoroutine(Vague());
        }
    }

    IEnumerator Vague()
    {
        canAttack = false;
        vague1.SetActive(true);
        vague1.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,-1)*vaguesSpeed);
        yield return new WaitForSeconds(2f);
        vague1.SetActive(false);
        vague1.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        vague1.transform.position = vague1Pos;
        vague2.SetActive(true);
        vague2.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,-1)*vaguesSpeed);
        yield return new WaitForSeconds(2f);
        vague2.SetActive(false);
        vague2.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        vague2.transform.position = vague2Pos;

        if (isCAC)
        {
            canAttack = true;
            StopCoroutine(Vague());
        }
        
        for (int i = 0; i < 5; i++)
        {
            GameObject attackChute = Instantiate(chute, player.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.4f);
            Destroy(attackChute);
        }

        canAttack = true;

    }
    

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            StartCoroutine(CAC());
        }
    }

    IEnumerator CAC()
    {
        isCAC = true;
        zone.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        zone.SetActive(false);
        isCAC = false;
    }
}
