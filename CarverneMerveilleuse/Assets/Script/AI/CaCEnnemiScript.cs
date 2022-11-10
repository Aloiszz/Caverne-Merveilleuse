using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class CaCEnnemiScript : MonoBehaviour
{
    private PlayerController player;

    [Header("AI Config")]
    public float speed;
    private bool canJump = true;
    public float jumpForce;
    public float distToJumpOnPlayer;
    private AIPath _aiPath;

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
        _aiPath = gameObject.GetComponent<AIPath>();
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
            canRandomMove = false;
        }
        OnSeePlayer();
    }


    private void OnSeePlayer()
    {
        if (see)
        {
            StopCoroutine(RandomMove());
            if (canJump)
            {
                _aiPath.enabled = true;
                if (playerDir.magnitude <= distToJumpOnPlayer)
                {
                    StartCoroutine(JumpOnPlayer());
                }
            }
        }
        else if (canRandomMove)
        {
            //StartCoroutine(RandomMove());
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

    IEnumerator JumpOnPlayer()
    {
        canJump = false;
        _aiPath.enabled = false;
        yield return new WaitForSeconds(1f);
        rb.AddForce(playerDir.normalized * jumpForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1);
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1);
        canJump = true;
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

