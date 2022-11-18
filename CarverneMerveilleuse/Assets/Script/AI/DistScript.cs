using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class DistScript : MonoBehaviour
{
    private PlayerController player;

    [Header("AI Config")]
    public float speed;

    public float spaceFromPlayer = 5;
    public GameObject projo;
    public float shootForce = 3f;
    public float TimeBeforeShoot = 3f;
    
    [Header("AI Physics")]
    public Rigidbody2D rb;

    [Header("AI perception")]
    public bool see;
    public float distanceToSeePlayer = 5f;

    [Header("AI Cinemachine Shake")] 
    public float intensity;
    public float frequency;
    public float timer;
    
    
    private bool canShoot = true;
    
    private bool canRandomMove = true;
    private Vector2 playerDir;
    private AIPath AI;

    void Start()
    {
        if (gameObject.GetComponent<Mechant>().invokeByBoss)
        {
            see = true;
        }
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        AI = gameObject.GetComponent<AIPath>();
        gameObject.GetComponent<AIDestinationSetter>().target = player.transform;
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
            StopCoroutine(RandomMove());
            AI.enabled = true;
            if (playerDir.magnitude < spaceFromPlayer)
            {
                transform.position =Vector2.MoveTowards(transform.position, player.gameObject.transform.position, -speed * Time.deltaTime);
            }
            if (canShoot)
            {
                StartCoroutine(Shoot());
            }
        }
        else if (canRandomMove)
        {
            StartCoroutine(RandomMove());
        }
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(0.2f);
        
        GameObject projectile = Instantiate(projo, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().AddForce(playerDir.normalized * shootForce);
        yield return new WaitForSeconds(TimeBeforeShoot);
        canShoot = true;
        
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
        if (PlayerController.instance.isDashing)
        {
            if (ItemManager.instance.isPushDashGet)
            {
                rb.AddForce(-playerDir.normalized * ItemManager.instance.puissancePushDash, ForceMode2D.Impulse);
            }
        }
        else
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

