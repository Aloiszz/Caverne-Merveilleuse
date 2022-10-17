using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetting : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = pos;*/
        
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
        mouseWorldPosition.z = 0f;
        transform.position = mouseWorldPosition;
    }
}
