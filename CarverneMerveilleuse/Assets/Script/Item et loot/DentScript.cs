using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class DentScript : MonoBehaviour
{
    private PlayerController player; 
    private UIManager ui;
    private Rigidbody2D rb;
    private Collider2D coll;
    public float distanceToGet;
    public float speed = 4;
    [SerializeField] public bool golden;

    [Header("Audio")] 
    private AudioSource source;
    public List<AudioClip> DentClips;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        coll = GetComponent<Collider2D>();
        ui = GameObject.Find("UIManager").GetComponent<UIManager>();
        rb = GetComponent<Rigidbody2D>();
        
        rb.AddForce(new Vector2(Random.Range(-3,4), Random.Range(-3,4)) * 2, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        rb.drag = 2.3f;
    }

    private void Update()
    {
        if ((PlayerController.instance.gameObject.transform.position - transform.position).magnitude <= distanceToGet)
        {
            coll.isTrigger = true;
            transform.position = Vector2.MoveTowards(gameObject.transform.position, PlayerController.instance.gameObject.transform.position, speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            int rand = Random.Range(0, DentClips.Count);
            if (golden)
            {
                ui.goldenMoney += 1;
                
            }
            else
            {
                ui.money += 1;
                source.PlayOneShot(DentClips[rand]);
            }
            
            Destroy(gameObject,0.1f);
        }
    }
}
