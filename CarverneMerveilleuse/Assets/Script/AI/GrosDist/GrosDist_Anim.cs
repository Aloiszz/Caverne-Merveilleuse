using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrosDist_Anim : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private List<Animator>  animator;
    [SerializeField] private GameObject  graph;

    private Rigidbody2D rb;
    private GrosEnnemiScript GrosDist;
    
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
        GrosDist = GetComponent<GrosEnnemiScript>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        GrosDistAnimation();
    }

    void GrosDistAnimation()
    {
        if (GrosDist.isAttacking)
        {
            animator[0].SetBool("isAttacking", true);
        }
        else
        {
            animator[0].SetBool("isAttacking", false);
            
        }
        
        
        if (GrosDist.isDefending)
        {
            animator[0].SetTrigger("isDefending");
        }
    }
}
