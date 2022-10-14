using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class felche : MonoBehaviour
{
    public Rigidbody2D rigidbody;
    public EdgeCollider2D BoxCollider;

    public float force = 300f;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        BoxCollider = GetComponent<EdgeCollider2D>();
        
        rigidbody.AddForce(new Vector2(-1,0) * force);
    }

    // Update is called once per frame
    void Update()   
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("CUBEELISE"))
        {
            col.GetComponent<progElise>().Destroy();
        }
    }
}
