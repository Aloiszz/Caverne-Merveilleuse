using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BossScript : MonoBehaviour
{
    [Header("Boss Stats")]
    public Image lifeBarre;

    [Header("Phase 1")] 
    public GameObject spawner1;
    public GameObject spawner2;
    public GameObject spawner3;
    public GameObject spawner4;
    public GameObject TypeEnnemi1;
    public GameObject TypeEnnemi2;
    public GameObject TypeEnnemi3;
    public int nbEnnemi;
    
    
    [Header("Attaques Phase 2")] 
    public GameObject vague1;
    public GameObject vague2;
    public GameObject attaqueAvant1;
    public GameObject attaqueAvant2;
    public GameObject zone;
    public GameObject chute;
    public GameObject transitionPoint;
    public float vaguesSpeed;
    public float tempsPrevention = 0.2f;

    [Header("Info")] 
    public GameObject player;
    public Mechant mechantScript;

    private Vector2 vague1Pos;
    private Vector2 vague2Pos;
    private bool canAttack = true;
    private bool isCAC = false;
    private bool startCAC = false;
    private bool startVague = true;
    private GameObject spawnEnnemi;
    private GameObject Ennemi1;
    private GameObject Ennemi2;
    private GameObject Ennemi3;
    private GameObject Ennemi4;
    private bool ennemi1Vivant;
    private bool ennemi2Vivant;
    private bool ennemi3Vivant;
    private bool ennemi4Vivant;
    private bool phaseTransition = true;

    private void Start()
    {
        vague1Pos = vague1.transform.position;
        vague2Pos = vague2.transform.position;
    }

    void Update()
    {
        lifeBarre.fillAmount = mechantScript.life / mechantScript.lifeDepart;
        if (mechantScript.life > mechantScript.lifeDepart / 2 && startVague)
        {
            StartCoroutine(Vague());
        }

        if (mechantScript.life <= mechantScript.lifeDepart / 2 && phaseTransition)
        {
            Destroy(Ennemi1);
            Destroy(Ennemi2);
            Destroy(Ennemi3);
            Destroy(Ennemi4);
            player.transform.position = transitionPoint.transform.position;
            phaseTransition = false;
        }
        
        if (mechantScript.life <= mechantScript.lifeDepart / 2 && canAttack && !isCAC)
        {
            StartCoroutine(Dist());
        }

        if ((player.transform.position - transform.position).magnitude <= 10 && mechantScript.life <= mechantScript.lifeDepart / 2)
        {
            isCAC = true;
            if (!startCAC)
            {
                StartCoroutine(CAC());
            }
            
        }
        else
        {
            isCAC = false;
        }
    }

    IEnumerator Vague()
    {
        startVague = false;
        if (nbEnnemi == 0)
        {
            yield return new WaitForSeconds(3f);
            if (!canAttack)
            {
                StopCoroutine(Vague());
            }
            else
            {
                int nb = Random.Range(1, 4);
                if (nb == 2)
                {
                    spawnEnnemi = TypeEnnemi2;
                }
                else if (nb == 1)
                {
                    spawnEnnemi = TypeEnnemi1;
                }
                else
                {
                    spawnEnnemi = TypeEnnemi3;
                }

                Ennemi1 = Instantiate(spawnEnnemi, spawner1.transform.position, Quaternion.identity);
                Ennemi1.GetComponent<Mechant>().invokeByBoss = true;
                nbEnnemi += 1;
                ennemi1Vivant = true;
                nb = Random.Range(1, 4);
                if (nb == 2)
                {
                    spawnEnnemi = TypeEnnemi2;
                }
                else if (nb == 1)
                {
                    spawnEnnemi = TypeEnnemi1;
                }
                else
                {
                    spawnEnnemi = TypeEnnemi3;
                }

                Ennemi2 = Instantiate(spawnEnnemi, spawner2.transform.position, Quaternion.identity);
                Ennemi2.GetComponent<Mechant>().invokeByBoss = true;
                nbEnnemi += 1;
                ennemi2Vivant = true;
                nb = Random.Range(1, 4);
                if (nb == 2)
                {
                    spawnEnnemi = TypeEnnemi2;
                }
                else if (nb == 1)
                {
                    spawnEnnemi = TypeEnnemi1;
                }
                else
                {
                    spawnEnnemi = TypeEnnemi3;
                }

                Ennemi3 = Instantiate(spawnEnnemi, spawner3.transform.position, Quaternion.identity);
                Ennemi3.GetComponent<Mechant>().invokeByBoss = true;
                nbEnnemi += 1;
                ennemi3Vivant = true;
                nb = Random.Range(1, 4);
                if (nb == 2)
                {
                    spawnEnnemi = TypeEnnemi2;
                }
                else if (nb == 1)
                {
                    spawnEnnemi = TypeEnnemi1;
                }
                else
                {
                    spawnEnnemi = TypeEnnemi3;
                }

                Ennemi4 = Instantiate(spawnEnnemi, spawner4.transform.position, Quaternion.identity);
                Ennemi4.GetComponent<Mechant>().invokeByBoss = true;
                nbEnnemi += 1;
                ennemi4Vivant = true;
            }
        }

        else
        {
            if (Ennemi1 == null && ennemi1Vivant)
            {
                ennemi1Vivant = false;
                nbEnnemi -= 1;
            }
            if (Ennemi2 == null && ennemi2Vivant)
            {
                ennemi2Vivant = false;
                nbEnnemi -= 1;
            }
            if (Ennemi3 == null && ennemi3Vivant)
            {
                ennemi3Vivant = false;
                nbEnnemi -= 1;
            }

            if (Ennemi4 == null && ennemi4Vivant)
            {
                ennemi4Vivant = false;
                nbEnnemi -= 1;
            }
        }
        
        startVague = true;
    }
    
    IEnumerator Dist()
    {
        canAttack = false;
        yield return new WaitForSeconds(1);
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
            StopCoroutine(Dist());
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject attackChute = Instantiate(chute, player.transform.position, Quaternion.identity);
                attackChute.GameObject().GetComponent<Collider2D>().enabled = false;
                attackChute.GetComponent<SpriteRenderer>().color = Color.green;
                yield return new WaitForSeconds(tempsPrevention);
                attackChute.GameObject().GetComponent<Collider2D>().enabled = true;
                attackChute.GetComponent<SpriteRenderer>().color = Color.red;
                yield return new WaitForSeconds(0.4f);
                Destroy(attackChute);
            }
            yield return new WaitForSeconds(1f);
            canAttack = true;
        }

    }
    
    IEnumerator CAC()
    {
        if (canAttack)
        {
            startCAC = true;
            canAttack = false;
            attaqueAvant1.SetActive(true);
            attaqueAvant1.GameObject().GetComponent<Collider2D>().enabled = false;
            attaqueAvant1.GetComponent<SpriteRenderer>().color = Color.green;
            yield return new WaitForSeconds(tempsPrevention);
            attaqueAvant1.GetComponent<SpriteRenderer>().color = Color.red;
            attaqueAvant1.GameObject().GetComponent<Collider2D>().enabled = true;
            yield return new WaitForSeconds(0.5f);
            attaqueAvant1.SetActive(false);
            yield return new WaitForSeconds(1f);
            attaqueAvant2.SetActive(true);
            attaqueAvant2.GameObject().GetComponent<Collider2D>().enabled = false;
            attaqueAvant2.GetComponent<SpriteRenderer>().color = Color.green;
            yield return new WaitForSeconds(tempsPrevention);
            attaqueAvant2.GetComponent<SpriteRenderer>().color = Color.red;
            attaqueAvant2.GameObject().GetComponent<Collider2D>().enabled = true;
            yield return new WaitForSeconds(0.5f);
            attaqueAvant2.SetActive(false);
            yield return new WaitForSeconds(1f);
            if (isCAC)
            {
                zone.SetActive(true);
                zone.GameObject().GetComponent<Collider2D>().enabled = false;
                zone.GetComponent<SpriteRenderer>().color = Color.green;
                yield return new WaitForSeconds(tempsPrevention + 0.5f);
                zone.GetComponent<SpriteRenderer>().color = Color.red;
                zone.GameObject().GetComponent<Collider2D>().enabled = true;
                yield return new WaitForSeconds(0.2f);
                zone.SetActive(false);
                yield return new WaitForSeconds(1f);
            }
            
            startCAC = false;
            canAttack = true;
        }
    }
}
