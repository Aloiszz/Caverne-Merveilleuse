
using System;
using System.Collections;
using System.Net.Cache;
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
            if (life > lifeDepart * 2 / 3)
            {
                lifeBarre.GetComponent<SpriteRenderer>().color = Color.green;
            }
            else if (life <= lifeDepart * 2 / 3 && life > lifeDepart / 3)
            {
                lifeBarre.GetComponent<SpriteRenderer>().color = Color.yellow;
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
            if (!invokeByBoss || !CompareTag("Boss"))
            {
                for (int i = 0; i < Random.Range(1, maxDentDrop + itemManager.dropSupp + 1); i++)
                {
                    Instantiate(dent, gameObject.transform.position + new Vector3(Random.Range(-3, 4), Random.Range(-3, 4)), Quaternion.identity);
                }

                if (Random.Range(1, 101) >= 100 - (pourcentageDropOr + itemManager.dropOrSupp) && pourcentageDropOr != 0)
                {
                    Instantiate(goldenDent, gameObject.transform.position + new Vector3(Random.Range(-3, 4), Random.Range(-3, 4)), Quaternion.identity);
                }
                
                if (Random.Range(1, 101) >= 100 - (pourcentageDropCoeur + itemManager.dropCoeurSupp) && pourcentageDropCoeur != 0)
                {
                    Instantiate(coeur, gameObject.transform.position + new Vector3(Random.Range(-3, 4), Random.Range(-3, 4)), Quaternion.identity);
                }
            }

            if (CompareTag("Gros"))
            {
                for (int i = 0; i < gameObject.GetComponent<GrosEnnemiScript>().projoList.Count; i++)
                {
                    Destroy(gameObject.GetComponent<GrosEnnemiScript>().projoList[i]);
                }
                
            }
            Destroy(gameObject);
        }
    }
    public void ReceiveLightDamage()
    {
        DebutHitStop();
        
        if (PlayerLightAttack.instance.countInput == PlayerLightAttack.instance.playerLightAttack.combo)
        {
            buffByDash = PlayerLightAttack.instance.lastLightDamage[PlayerLightAttack.instance.lastLightDamageIndex] * itemManager.dashBuff;
            buffAtk = PlayerLightAttack.instance.lastLightDamage[PlayerLightAttack.instance.lastLightDamageIndex] * itemManager.buffATK;
            buffCritique = PlayerLightAttack.instance.lastLightDamage[PlayerLightAttack.instance.lastLightDamageIndex] * itemManager.buffATKCritique;
            life -= PlayerLightAttack.instance.lastLightDamage[PlayerLightAttack.instance.lastLightDamageIndex] + buffAtk + buffCritique + buffByDash;
            player.life += itemManager.regenVie;
            forcelightDamage += 400 + itemManager.puissancePush;
        }
        else if (PlayerLightAttack.instance.countInput != checkIfSameHitBox)
        {
            buffByDash = PlayerLightAttack.instance.lightDamage[PlayerLightAttack.instance.lightDamageIndex] * itemManager.dashBuff;
            buffAtk = PlayerLightAttack.instance.lightDamage[PlayerLightAttack.instance.lightDamageIndex] * itemManager.buffATK;
            buffCritique = PlayerLightAttack.instance.lightDamage[PlayerLightAttack.instance.lightDamageIndex] * itemManager.buffATKCritique;
            life -= PlayerLightAttack.instance.lightDamage[PlayerLightAttack.instance.lightDamageIndex] + buffAtk + buffCritique + buffByDash;
            player.life += itemManager.regenVie;
            checkIfSameHitBox = PlayerLightAttack.instance.countInput;
        }
        
        rb.AddForce((transform.position - player.transform.position) * forcelightDamage);
        forcelightDamage = initialforcelightDamage;

        StartCoroutine(FinHitStop());
    }

    public void ReceiveAOEDamage()
    {
        DebutHitStop();
        life -= PlayerHeavyAttack.instance.heavyDamage[PlayerHeavyAttack.instance.heavyDamageIndex];
        forcelightDamage += 1000 * itemManager.buffPushAB;
        rb.AddForce((transform.position - player.transform.position) * forcelightDamage);
        StartCoroutine(FinHitStop());
    }

    public void ReceiveThrowDamage()
    {
        DebutHitStop();
        life -= PlayerThrowAttack.instance.ThrowDamage[PlayerThrowAttack.instance.ThrowDamageIndex];
        StartCoroutine(FinHitStop());
    }

    public void OtherHit()
    {
        life -= PlayerLightAttack.instance.lightDamage[PlayerLightAttack.instance.lightDamageIndex] / 2;
    }

    void DebutHitStop()
    {
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

    IEnumerator FinHitStop()
    {
        yield return new WaitForSeconds(0.2f);
        switch (gameObject.tag)
        {
            case "CAC":
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
