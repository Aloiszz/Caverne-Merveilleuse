using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CACRigidbody : MonoBehaviour
{
    // Start is called before the first frame update
    
    public float speed = 7;
    public Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = rb.velocity.x;
        Debug.Log(speed);
    }
}
