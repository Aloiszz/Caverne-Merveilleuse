using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class progElise : MonoBehaviour
{

    public float vitesse = 3.28f;
    public Rigidbody2D corps;

    public static progElise instance;
    
    // Start is called before the first frame update

    void Awake()
    {
        if (instance != null && instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            instance = this; 
        } 
    }
    void Start()
    {
        corps = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        déplacement();
    }

    void déplacement()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            corps.AddForce(Vector2.right*vitesse);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            corps.AddForce(Vector2.left*vitesse);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            corps.AddForce(Vector2.up*vitesse);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            corps.AddForce(Vector2.down*vitesse);
        }
        
    }
}
