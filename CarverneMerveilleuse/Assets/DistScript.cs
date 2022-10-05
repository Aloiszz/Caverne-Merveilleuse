using System.Collections;
using System.Collections.Generic;
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
    public float linearDragDeceleration;
    public float linearDragMultiplier;

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

    void Start()
    {
        if (gameObject.GetComponent<Mechant>().invokeByBoss)
        {
            see = true;
        }
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
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
            StopCoroutine(RandomMove());
            if (playerDir.magnitude < spaceFromPlayer)
            {
                    transform.position = Vector2.MoveTowards(transform.position, player.gameObject.transform.position,
                        -speed * Time.deltaTime);
            }
            else
            {
                    transform.position =Vector2.MoveTowards(transform.position, player.gameObject.transform.position, speed * Time.deltaTime);
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
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Rigidbody2D>().AddForce(playerDir * 2000);
            PlayerController.instance.LoseLife();
            CinemachineShake.instance.ShakeCamera(intensity, frequency ,timer);
        }
    }
}

