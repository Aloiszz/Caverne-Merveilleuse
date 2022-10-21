
using System;
using System.Collections;
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
    private ItemManager itemManager;
    private float buffAtk;
    private float buffCritique;
    private float buffByDash;
    


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

            Destroy(gameObject);
        }
    }

    public void ReceiveLightDamage()
    {
        if (PlayerLightAttack.instance.countInput == PlayerLightAttack.instance.playerLightAttack.combo)
        {
            buffByDash = PlayerLightAttack.instance.playerLightAttack.lastLightDamage * itemManager.dashBuff;
            buffAtk = PlayerLightAttack.instance.playerLightAttack.lastLightDamage * itemManager.buffATK;
            buffCritique = PlayerLightAttack.instance.playerLightAttack.lastLightDamage * itemManager.buffATKCritique;
            life -= PlayerLightAttack.instance.playerLightAttack.lastLightDamage + buffAtk + buffCritique + buffByDash;
            player.life += itemManager.regenVie;
            forcelightDamage += itemManager.puissancePush;
        }
        else
        {
            buffByDash = PlayerLightAttack.instance.playerLightAttack.lightDamage * itemManager.dashBuff;
            buffAtk = PlayerLightAttack.instance.playerLightAttack.lightDamage * itemManager.buffATK;
            buffCritique = PlayerLightAttack.instance.playerLightAttack.lightDamage * itemManager.buffATKCritique;
            life -= PlayerLightAttack.instance.playerLightAttack.lightDamage + buffAtk + buffCritique + buffByDash;
            player.life += itemManager.regenVie;
        }
        
        rb.AddForce((transform.position - player.transform.position) * forcelightDamage);
        forcelightDamage = initialforcelightDamage;
    }

}
