using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Cac_Anim : MonoBehaviour
{
    
    [Header("Animator")]
    [SerializeField] private List<Animator>  animator;
    [SerializeField] private GameObject  graph;

    private Rigidbody2D rb;
    private CaCEnnemiScript spyder;
    
    private void Update()
    {
        /*foreach (var i in animator)
        {
            i.SetFloat("speedX", GetComponent<CaCEnnemiScript>().rb.velocity.x);
            i.SetFloat("speedY", GetComponent<CaCEnnemiScript>().rb.velocity.y);
        }*/
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spyder = GetComponent<CaCEnnemiScript>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CacAnimation();
    }

    void CacAnimation()
    {
        if (spyder._aiPath.enabled)
        {
            animator[0].SetBool("isWalking", true);
            
            
        }
        else
        {
            animator[0].SetBool("isWalking", false);

            /*if (!spyder.canJump) // attack
            {
                animator[0].SetBool("isAttacking", true);
            }
            else
            {
                animator[0].SetBool("isAttacking", false);
            }*/
            
            
            if (spyder.speed >= 0.3f)
            {
                graph.transform.localScale = new Vector3(0.102338f,0.102338f,102338f);
            }
            else if (spyder.speed <= -0.3f)
            {
                Debug.Log("qsd");
                graph.transform.localScale = new Vector3(-0.102338f,0.102338f,102338f);
            }
        }
        
    }
}
