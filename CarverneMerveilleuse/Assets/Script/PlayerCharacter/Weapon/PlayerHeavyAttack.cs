using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeavyAttack : MonoBehaviour
{
    [Header("Player Heavy Attack config")]
    public SO_PlayerHeavyAttack playerHeavyAttack;

    public int countInput = 0;
    public bool activate = false;
    public GameObject AttackCollision;

    public static PlayerHeavyAttack instance;
    
    [Header("Secure data Light Attack")]
    
    [Header("Verification")] 
    private bool isStriking;
    private bool isCoolDown = false;
    
    [Header("Damage")]
    [HideInInspector] public List<float> heavyDamage;
    [HideInInspector]public int heavyDamageIndex;

    [Header("Combo")]
    private List<float> coolDown; // temps entre chaque coups
    private int coolDownIndex;
    private float timerRemaining; // temps restant entre chaque coup pour arriver au combo
    private List<int> numberOfTurn; //nombre de tour que peut faire l'attaque
    private int numberOfTurnIndex;

    [Header("Cinemachine Schake")] 
    private float intensityLightCloseDamage;
    private float frequencyLightCloseDamage;
    private float timerLightCloseDamage;
    
    private void Awake()
    {
        if (instance != null && instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            instance = this; 
        }
    }
    void Start()
    {
        AttackCollision = GameObject.FindGameObjectWithTag("AttackCollision");
        SecureSO();
    }
    private void Update()
    {
        HeavyAttack();
        TimeRemaining(activate);
    }
    void SecureSO()
    {
        isStriking = playerHeavyAttack.isStriking;
        isCoolDown = playerHeavyAttack.isCoolDown;

        heavyDamage = playerHeavyAttack.heavyDamage;
        
        coolDown = playerHeavyAttack.coolDown;
        timerRemaining = playerHeavyAttack.timerRemaining;
        numberOfTurn = playerHeavyAttack.numberOfTurn;
        
        intensityLightCloseDamage = playerHeavyAttack.intensityLightCloseDamage;
        frequencyLightCloseDamage = playerHeavyAttack.frequencyLightCloseDamage;
        timerLightCloseDamage = playerHeavyAttack.timerLightCloseDamage;
    }

    public void HeavyAttack()
    {
        if (!isCoolDown)
        {
            if (Input.GetMouseButtonDown(1))
            {
                activate = false;
                countInput++;
                if (countInput <= numberOfTurn[numberOfTurnIndex])
                {
                    StartCoroutine(Turn());
                    if (countInput == numberOfTurn[numberOfTurnIndex])
                    {
                        CinemachineCameraZoom.instance.CameraZoom(8f, 0.05f, 0.6f);
                        CinemachineShake.instance.ShakeCamera(3,3,0.5f);
                    }
                }
                else
                {
                    StartCoroutine(CoolDown());
                    countInput = 0;
                }
            }
        }
        if (isStriking)
        {
            isStriking = false;
            CinemachineShake.instance.ShakeCamera(intensityLightCloseDamage, 
                frequencyLightCloseDamage ,timerLightCloseDamage);
        }
    }
    private void TimeRemaining(bool activate)
    {
        if (activate && timerRemaining >= 0)
        {
            timerRemaining -= Time.deltaTime;
        }

        if (timerRemaining <= 0)
        {
            countInput = 0;
        }
    }
    
    IEnumerator CoolDown()
    {
        PlayerAttackCollision.instance.sprite.enabled = false;
        PlayerAttackCollision.instance.coll.enabled = false;
        isCoolDown = true;
        yield return new WaitForSeconds(coolDown[coolDownIndex]); //- ItemManager.instance.endComboSoustracteur
        isCoolDown = false;
    }

    IEnumerator Turn()
    {
        PlayerAttackCollision.instance.sprite.enabled = true;
        PlayerAttackCollision.instance.coll.enabled = true;
        isCoolDown = true;
        yield return new WaitForSeconds(coolDown[coolDownIndex]);
        isCoolDown = false;
        PlayerAttackCollision.instance.sprite.enabled = false;
        PlayerAttackCollision.instance.coll.enabled = false;    
        activate = true;
    }
}
