using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class CaCEnnemiScript : MonoBehaviour
{
    private PlayerController player;

    [Header("AI Config")]
    public float speed;
    private float lifeDepart;

    [Header("AI Physics")]
    public Rigidbody2D rb;
    public float linearDragDeceleration;
    public float linearDragMultiplier;

    [Header("AI perception")]
    public bool see;
    public float distanceToSeePlayer = 5f;

    [Header("AI Cinemachine Shake")] 
    public float intensity;
    public float frequency;
    public float timer;
    
    private bool canRandomMove = true;
    private Vector2 playerDir;

    void Start()
    {
        if (gameObject.GetComponent<Mechant>().invokeByBoss)
        {
            see = true;
        }
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        gameObject.GetComponent<AIDestinationSetter>().target = player.transform;
    }
    
    void FixedUpdate()
    {
        rb.drag = linearDragDeceleration * linearDragMultiplier; 
    }

    // Update is called once per frame
    void Update()
    {
        playerDir = player.transform.position - transform.position;
        if (playerDir.magnitude <= distanceToSeePlayer)
        {
            see = true;
        }
        OnSeePlayer();
    }


    private void OnSeePlayer()
    {
        if (see)
        {
            gameObject.GetComponent<AIDestinationSetter>().enabled = true;
            
        }
        else if (canRandomMove)
        {
            StartCoroutine(RandomMove());
        }
    }
    

    IEnumerator RandomMove()
    {
        canRandomMove = false;
        rb.AddForce(new Vector2(Random.Range(-9f, 10f),Random.Range(-9f,10f)) * 25);
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(3f);
        canRandomMove = true;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Rigidbody2D>().AddForce(playerDir * 2000);
            PlayerController.instance.LoseLife();
            CinemachineShake.instance.ShakeCamera(intensity, frequency ,timer);
        }

        if (col.gameObject.layer == 4)
        {
            rb.AddForce(new Vector2(-playerDir.normalized.x,-playerDir.normalized.y) * 400);
        }
    }
}

