using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;

public class GrosEnnemiScript : MonoBehaviour
{
    private PlayerController player;

    [Header("AI Config")]
    public float spaceFromPlayer = 5;
    public float distForCAC = 3;
    public GameObject grosProjo;
    public float grosForce = 2f;
    public float maxRangeProjo;
    public int maxProjo = 3;
    public float TimeBeforeLinkedShoot = 2;

    [Header("AI Physics")]
    public Rigidbody2D rb;


    [Header("AI perception")]
    public bool see;
    public float distanceToSeePlayer = 5f;

    [Header("AI Cinemachine Shake")] 
    public float intensity;
    public float frequency;
    public float timer;
    
    
    public bool canShoot = true;
    public bool canCaC = true;
    private bool canRandomMove = true;
    private Vector2 playerDir;
    private bool firstTimeShoot = true;
    [HideInInspector] public AIPath AI;
    private GameObject projectile;
    public GameObject cacHitBox;
    [HideInInspector] public List<GameObject> projoList;

    [Header("Animator")] 
    [HideInInspector] public bool isDefending;
    [HideInInspector] public bool isAttacking;
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
        if (rb.velocity.magnitude > 10)
        {
            rb.velocity = Vector2.zero;
        }
    }


    private void OnSeePlayer()
    {
        if (see)
        {
            StopCoroutine(RandomMove());
            if (canShoot)
            {
                AI.enabled = true;
                isAttacking = false;
                if (playerDir.magnitude < spaceFromPlayer)
                {
                    StartCoroutine(GrosShoot());
                }
            }

            if (playerDir.magnitude <= distForCAC && canCaC)
            {
                StartCoroutine(CaC());
            }
        }
        else if (canRandomMove)
        {
            //StartCoroutine(RandomMove());
        }
    }

    IEnumerator GrosShoot()
    {
        canShoot = false;
        if (firstTimeShoot)
        {
            yield return new WaitForSeconds(0.5f);
            firstTimeShoot = false;
        }
        AI.enabled = false;
        rb.velocity = Vector2.zero;
        isAttacking = true;
        projectile = Instantiate(grosProjo, transform.position, Quaternion.identity);
        projectile.GetComponent<ProjoCollision>().shooter = gameObject;
        projoList.Add(projectile);
        projectile.GetComponent<Rigidbody2D>().velocity = playerDir.normalized * grosForce;
        projectile.GetComponent<ProjoCollision>().origine = this;
        yield return new WaitUntil(() => (projectile.transform.position - transform.position).magnitude >= maxRangeProjo);
        projectile.GetComponent<ProjoCollision>().Grossissement(player.gameObject);
        isAttacking = false;
    }

    public IEnumerator CaC()
    {
        canCaC = false;
        for (int i = 0; i < 8; i++)
        {
            if (i % 2 == 0)
            {
                yield return new WaitForSeconds(0.1f);
                transform.position = new Vector2(transform.position.x + 0.2f, transform.localPosition.y);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
                transform.position = new Vector2(transform.position.x - 0.2f, transform.localPosition.y);
            }
        }
        isDefending = true;
        cacHitBox.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        isDefending = false;
        cacHitBox.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        canCaC = true;
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
            if (PlayerController.instance.isDashing)
            {
                if (ItemManager.instance.isPushDashGet)
                {
                    rb.AddForce(-playerDir.normalized * ItemManager.instance.puissancePushDash, ForceMode2D.Impulse);
                }

                if (ItemManager.instance.isDegatDashGet)
                {
                    GetComponent<Mechant>().OtherHit();
                }
            }
            else
            {
                col.gameObject.GetComponent<Rigidbody2D>().AddForce(playerDir * 2000);
                PlayerController.instance.LoseLife();
                CinemachineShake.instance.ShakeCamera(intensity, frequency ,timer);
            }
            
        }
        if (col.gameObject.layer == 4)
        {
            rb.AddForce(new Vector2(-playerDir.normalized.x,-playerDir.normalized.y) * 400);
        }
    }
}

