using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightPath : MonoBehaviour
{
    
    public List<Light2D> colliders = new List<Light2D>();

    public List<Light2D> GetColliders()
    {
        return colliders; 
        
    }

    private Collider2D col;
    private void Start()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D (Collider2D col) 
    {
        
        if (col.GetComponentInChildren<Light2D>())
        {
            
            colliders.Add(col.GetComponentInChildren<Light2D>());
        }
        /*if (col.GetComponent<Light2D>())
        {
            Debug.Log("LIght");
            //colliders.Add(col.GetComponent<Light2D>().GetComponentInChildren<Light2D>());
            colliders.Add(col.transform.GetChild(0).transform.GetChild(0).GetComponent<Light2D>());
            colliders.Add(col.transform.GetChild(0).transform.GetChild(1).GetComponent<Light2D>());
            colliders.Add(col.transform.GetChild(0).transform.GetChild(2).GetComponent<Light2D>());
        }*/
    }

    private void OnTriggerExit2D (Collider2D col) 
    {
        
    }
 
    
}
