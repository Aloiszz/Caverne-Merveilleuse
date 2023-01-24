
using System;
using System.Collections;
using System.Net.Cache;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Mechant : MonoBehaviour
{
    private PlayerController player;

    [Header("AI Config")]
    public float life;
    public float lifeDepart;
    public GameObject lifeBarre;


    [Header("AI Physics")]
    public Rigidbody2D rb;
    public float linearDragDeceleration;
    public float linearDragMultiplier;
    public float forcelightDamage;
    private float initialforcelightDamage;

    [Header("Loot")] 
    public int pointScore;
    public GameObject dent;
    public GameObject goldenDent;
    public GameObject coeur;
    public int maxDentDrop;
    public int pourcentageDropOr;
    public int pourcentageDropCoeur;
    
    [HideInInspector] public bool invokeByBoss;
    [HideInInspector] public bool isInPetrole;
    private ItemManager itemManager;
    private float buffAtk;
    private float buffCritique;
    private float buffByDash;
    private int checkIfSameHitBox;
    private bool killedWithLightAtk;
    [HideInInspector] public bool canTakeDamage = true;
    private int zizou = 1;


    void Start()
    {
        initialforcelightDamage = forcelightDamage;
        buffAtk = 0;
        lifeDepart = life;
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
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
        
        if (!CompareTag("Boss"))
        {
            lifeBarre.transform.localScale = new Vector2(life / lifeDepart, 0.1f);
            if (life > lifeDepart / 2)
            {
                lifeBarre.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                lifeBarre.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }

        if (PlayerLightAttack.instance.countInput == 0)
        {
            checkIfSameHitBox = 0;
        }
        Death();
       
    }
    void Death()
    {
        if (life <= 0)
        {
            if (!CompareTag("Boss"))
            {
                if (!invokeByBoss)
                {
                    for (int i = 0; i < maxDentDrop + itemManager.dropSupp; i++)
                    {
                        if (zizou == 1)
                        {
                            if (Random.Range(0,5) >= 2)
                            {
                                gameObject.transform.DOMove(new Vector3(Random.Range(-3, 4), Random.Range(-3, 4)), 0.1f);
                                Instantiate(dent, gameObject.transform.position, Quaternion.identity,
                                    RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].transform);
                            }
                            zizou -= 1;
                            
                        }
                        else if (Random.Range(0, 5) == 1)
                        {
                            gameObject.transform.DOMove(new Vector3(Random.Range(-3, 4), Random.Range(-3, 4)), 0.1f);
                            Instantiate(dent, gameObject.transform.position, Quaternion.identity,
                                RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].transform);
                            
                        }
                    }

                    if (Random.Range(1, 101) >= 100 - (pourcentageDropOr + itemManager.dropOrSupp) &&
                        pourcentageDropOr != 0)
                    {
                        Instantiate(goldenDent, gameObject.transform.position, Quaternion.identity,
                            RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].transform);
                    }

                    if (Random.Range(1, 101) >= 100 - (pourcentageDropCoeur + itemManager.dropCoeurSupp) &&
                        pourcentageDropCoeur != 0)
                    {
                        Instantiate(coeur, gameObject.transform.position, Quaternion.identity,
                            RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].transform);
                    }

                    if (CompareTag("CAC"))
                    {
                        AudioManager.instance.PlaySpiderDeath(GetComponent<AudioSource>());
                    }

                    if (CompareTag("Dist"))
                    {
                        AudioManager.instance.PlayBatDeath(GetComponent<AudioSource>());
                    }
                    
                    Score.instance.score += pointScore * PlayerThrowAttack.instance.multiplicateur;
                }

                Destroy(gameObject);
            }

            if (CompareTag("Gros"))
            {
                for (int i = 0; i < gameObject.GetComponent<GrosEnnemiScript>().projoList.Count; i++)
                {
                    Destroy(gameObject.GetComponent<GrosEnnemiScript>().projoList[i]);
                }
                AudioManager.instance.PlayGrosDeath(GetComponent<AudioSource>());
                Destroy(gameObject);
                
            }

            if (killedWithLightAtk)
            {
                PlayerController.instance.life += ItemManager.instance.regenVie;
            }
            
            //Score.instance.scoreRage += pointScore;
            if (CompareTag("Boss") && Score.instance.activateScore)
            {
                Score.instance.score += pointScore;
                Score.instance.AddScore();
            }
            
        }
    }
    public void ReceiveLightDamage()
    {
        killedWithLightAtk = true;
        if (PlayerLightAttack.instance.countInput == PlayerLightAttack.instance.playerLightAttack.combo)
        {
            DebutHitStop();
            buffByDash = PlayerLightAttack.instance.lastLightDamage[PlayerLightAttack.instance.lastLightDamageIndex] * itemManager.dashBuff;
            buffAtk = PlayerLightAttack.instance.lastLightDamage[PlayerLightAttack.instance.lastLightDamageIndex] * itemManager.buffATK;
            buffCritique = PlayerLightAttack.instance.lastLightDamage[PlayerLightAttack.instance.lastLightDamageIndex] * itemManager.buffATKCritique;
            life -= PlayerLightAttack.instance.lastLightDamage[PlayerLightAttack.instance.lastLightDamageIndex] + buffAtk + buffCritique + buffByDash;
            forcelightDamage += 400 + itemManager.puissancePush;
            StartCoroutine(FinHitStop());
        }
        else if (PlayerLightAttack.instance.countInput != checkIfSameHitBox)
        {
            buffByDash = PlayerLightAttack.instance.lightDamage[PlayerLightAttack.instance.lightDamageIndex] * itemManager.dashBuff;
            buffAtk = PlayerLightAttack.instance.lightDamage[PlayerLightAttack.instance.lightDamageIndex] * itemManager.buffATK;
            buffCritique = PlayerLightAttack.instance.lightDamage[PlayerLightAttack.instance.lightDamageIndex] * itemManager.buffATKCritique;
            life -= PlayerLightAttack.instance.lightDamage[PlayerLightAttack.instance.lightDamageIndex] + buffAtk + buffCritique + buffByDash;
            checkIfSameHitBox = PlayerLightAttack.instance.countInput;
        }

        if (!CompareTag("Boss"))
        {
            rb.AddForce((transform.position - player.transform.position) * forcelightDamage);
        }

        if (CompareTag("Boss"))
        {
            int rand = Random.Range(0, AudioManager.instance.BossHit.Count);
            AudioManager.instance.SFXSource.PlayOneShot(AudioManager.instance.BossHit[rand]);
        }
        
        forcelightDamage = initialforcelightDamage;

        
    }

    public void ReceiveAOEDamage()
    {
        killedWithLightAtk = false;
        DebutHitStop();
        buffByDash = PlayerHeavyAttack.instance.heavyDamage[PlayerHeavyAttack.instance.heavyDamageIndex] * itemManager.dashBuff;
        buffAtk = PlayerHeavyAttack.instance.heavyDamage[PlayerHeavyAttack.instance.heavyDamageIndex] * itemManager.buffATK;
        buffCritique = PlayerHeavyAttack.instance.heavyDamage[PlayerHeavyAttack.instance.heavyDamageIndex] * itemManager.buffATKCritique;
        life -= PlayerHeavyAttack.instance.heavyDamage[PlayerHeavyAttack.instance.heavyDamageIndex] + buffAtk + buffCritique + buffByDash;
        if (PlayerHeavyAttack.instance.coolDownIndex == 2)
        {
            forcelightDamage += 100 * itemManager.buffPushAB * 2;
        }
        else
        {
            forcelightDamage += 100 * itemManager.buffPushAB;
        }
        
        rb.AddForce((transform.position - player.transform.position) * forcelightDamage);
        forcelightDamage = initialforcelightDamage;
        StartCoroutine(FinHitStop());
        
        if (CompareTag("Boss"))
        {
            int rand = Random.Range(0, AudioManager.instance.BossHit.Count);
            AudioManager.instance.SFXSource.PlayOneShot(AudioManager.instance.BossHit[rand]);
        }
    }

    public void ReceiveThrowDamage()
    {
        if (canTakeDamage)
        {
            killedWithLightAtk = false;
            DebutHitStop();
            buffByDash = PlayerThrowAttack.instance.ThrowDamage[PlayerThrowAttack.instance.ThrowDamageIndex] *
                         itemManager.dashBuff;
            buffAtk = PlayerThrowAttack.instance.ThrowDamage[PlayerThrowAttack.instance.ThrowDamageIndex] *
                      itemManager.buffATK;
            buffCritique = PlayerThrowAttack.instance.ThrowDamage[PlayerThrowAttack.instance.ThrowDamageIndex] *
                           itemManager.buffATKCritique;
            life -= PlayerThrowAttack.instance.ThrowDamage[PlayerThrowAttack.instance.ThrowDamageIndex] + buffAtk +
                    buffCritique + buffByDash;
            StartCoroutine(FinHitStop());
        }
        
        if (CompareTag("Boss"))
        {
            int rand = Random.Range(0, AudioManager.instance.BossHit.Count);
            AudioManager.instance.SFXSource.PlayOneShot(AudioManager.instance.BossHit[rand]);
        }
    }

    public void OtherHit()
    {
        killedWithLightAtk = false;
        buffByDash = PlayerLightAttack.instance.lightDamage[PlayerLightAttack.instance.lightDamageIndex] * itemManager.dashBuff;
        buffAtk = PlayerLightAttack.instance.lightDamage[PlayerLightAttack.instance.lightDamageIndex] * itemManager.buffATK;
        buffCritique = PlayerLightAttack.instance.lightDamage[PlayerLightAttack.instance.lightDamageIndex] * itemManager.buffATKCritique;
        life -= (PlayerLightAttack.instance.lightDamage[PlayerLightAttack.instance.lightDamageIndex] + buffAtk + buffCritique + buffByDash) / 2;
    }

    public void RageArea()
    {
        rb.AddForce((transform.position - player.transform.position) * 500);
    }

    public void DebutHitStop()
    {
        StopCoroutine(FinHitStop());
        switch (gameObject.tag)
        {
            case "CAC":
                CaCEnnemiScript cacScript = GetComponent<CaCEnnemiScript>();
                cacScript.StopAllCoroutines();
                if (cacScript.canJump)
                {
                    cacScript.canJump = false;
                    rb.velocity = Vector2.zero;
                }
                cacScript._aiPath.enabled = false;
                break;
            
            case "Dist":
                DistScript distScript = GetComponent<DistScript>();
                distScript.StopAllCoroutines();
                distScript.canShoot = false;
                distScript.AI.enabled = false;
                distScript.isHit = true;
                rb.velocity = Vector2.zero;
                break;
            
            case "Gros":
                GrosEnnemiScript grosScript = GetComponent<GrosEnnemiScript>();
                grosScript.StopCoroutine(grosScript.CaC());
                break;
        }
    }

    public IEnumerator FinHitStop()
    {
        yield return new WaitForSeconds(0.2f);
        switch (gameObject.tag)
        {
            case "CAC":
                yield return new WaitForSeconds(0.2f);
                CaCEnnemiScript cacScript = GetComponent<CaCEnnemiScript>();
                cacScript.canJump = true;
                cacScript.see = true;
                cacScript._aiPath.enabled = true;
                break;
            
            case "Dist":
                DistScript distScript = GetComponent<DistScript>();
                distScript.canShoot = true;
                distScript.see = true;
                distScript.AI.enabled = true;
                distScript.isHit = false;
                break;
            
            case "Gros":
                GrosEnnemiScript grosScript = GetComponent<GrosEnnemiScript>();
                grosScript.see = true;
                break;
        }
    }
}
