using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class DentScript : MonoBehaviour
{
    private PlayerController player; 
    private UIManager ui;
    public float distanceToGet;
    public float speed = 4;
    [SerializeField] public bool golden;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        ui = GameObject.Find("UIManager").GetComponent<UIManager>();
        
        gameObject.transform.DOMove(new Vector3(Random.Range(-3, 4), Random.Range(-3, 4)), 0.1f);
    }

    private void Update()
    {
        if ((player.transform.position - transform.position).magnitude <= distanceToGet)
        {
            transform.position = Vector2.MoveTowards(gameObject.transform.position, player.transform.position, speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (golden)
            {
                ui.goldenMoney += 1;
            }
            else
            {
                ui.money += 1;
            }
            
            Destroy(gameObject);
        }
    }
}
