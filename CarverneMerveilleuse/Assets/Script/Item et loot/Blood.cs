using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Blood : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]private float speed = 2;
    
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameObject.transform.DOScale(new Vector3(
            Random.Range(.5f,2), Random.Range(.5f,2)), .2f);
        rb.AddForce(new Vector2(Random.Range(-3,4), Random.Range(-3,4)) * speed, ForceMode2D.Impulse);
    }
}
