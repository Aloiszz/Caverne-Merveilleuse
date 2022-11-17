using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.PlayerLoop;

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
    

    [Header("AI perception")]
    public bool see;
    public float distanceToSeePlayer = 5f;

    [Header("AI Cinemachine Shake")] 
    public float intensity;
    public float frequency;
    public float timer;
    
    private bool canRandomMove = true;
    private Vector2 playerDir;
    private Vector2 playerPos; //Pour le dash

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
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < 10; i++)
        {
            if (i%2 == 0)
            {
                yield return new WaitForSeconds(0.1f);
                transform.localPosition = new Vector2(transform.localPosition.x + 0.2f, transform.localPosition.y);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
                transform.localPosition = new Vector2(transform.localPosition.x - 0.2f, transform.localPosition.y);
            }

            if (i == 7)
            {
                playerPos = playerDir;
            }
        }


        rb.AddForce(playerPos.normalized * jumpForce, ForceMode2D.Impulse);
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

