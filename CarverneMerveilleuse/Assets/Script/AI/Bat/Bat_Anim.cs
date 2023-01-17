using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat_Anim : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private List<Animator>  animator;
    [SerializeField] private GameObject  graph;
    [SerializeField] private GameObject  eye1;
    [SerializeField] private GameObject  eye2;
    
    
    private Rigidbody2D rb;
    private DistScript bat;
    
    
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
        bat = GetComponent<DistScript>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        DistAnimation();
    }

    void DistAnimation()
    {
        
    }
}
