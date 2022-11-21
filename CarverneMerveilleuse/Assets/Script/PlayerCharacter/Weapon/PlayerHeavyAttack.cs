using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerHeavyAttack : MonoBehaviour
{
    [Header("Player Heavy Attack config")]
    public SO_PlayerHeavyAttack playerHeavyAttack;

    public int countInput = 0;
    public bool activate = false;
    private bool isKeyUp = true;

    [Header("Camera")] 
    public float zoomSize;
    public float timeToArrive;
    public float timeToComeBack;
    
    private GameObject AttackCollision;
    private GameObject Pivot;

    public static PlayerHeavyAttack instance;
    
    [Header("Secure data Light Attack")]
    
    [Header("Verification")] 
    private bool isStriking;
    private bool isCoolDown = false;
    
    [Header("Damage")]
    [HideInInspector] public List<float> heavyDamage;
    [HideInInspector]public int heavyDamageIndex;

    [Header("Combo")]
    private List<float> loadingCoolDown; // Préparation de l'attaque
    private int loadingCoolDownIndex;
    private List<float> coolDown; // temps entre chaque coups
    private int coolDownIndex;
    private float timerRemaining; // temps restant entre chaque coup pour arriver au combo
    [HideInInspector] public List<int> numberOfTurn; //nombre de tour que peut faire l'attaque
    [HideInInspector] public int numberOfTurnIndex;

    [Header("Cinemachine Schake")] 
    private float intensityLightCloseDamage;
    private float frequencyLightCloseDamage;
    private float timerLightCloseDamage;

    private bool isAlreadyScaled;
    
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
        Pivot = GameObject.FindGameObjectWithTag("PivotAttackPoint");
        SecureSO();
    }
    private void Update()
    {
        HeavyAttack();
    }
    void SecureSO()
    {
        isStriking = playerHeavyAttack.isStriking;
        isCoolDown = playerHeavyAttack.isCoolDown;

        heavyDamage = playerHeavyAttack.heavyDamage;
        heavyDamageIndex = playerHeavyAttack.heavyDamageIndex;

        loadingCoolDown = playerHeavyAttack.loadingCoolDown;
        loadingCoolDownIndex = playerHeavyAttack.loadingCoolDownIndex;
        
        coolDown = playerHeavyAttack.coolDown;
        coolDownIndex = playerHeavyAttack.coolDownIndex;
        
        timerRemaining = playerHeavyAttack.timerRemaining;
        numberOfTurn = playerHeavyAttack.numberOfTurn;
        
        intensityLightCloseDamage = playerHeavyAttack.intensityLightCloseDamage;
        frequencyLightCloseDamage = playerHeavyAttack.frequencyLightCloseDamage;
        timerLightCloseDamage = playerHeavyAttack.timerLightCloseDamage;
    }

    public void HeavyAttack()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0) && isKeyUp) // annule l'attaque lourde
        {
            StopAllCoroutines();
            CinemachineCameraZoom.instance.StopZoom(timeToComeBack);
            PlayerController.instance.SecureSO();
        }
        
        if (!isCoolDown) // Attaque lourde
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                CinemachineCameraZoom.instance.CameraZoom(zoomSize, timeToArrive, timeToComeBack);
                StartCoroutine(Turn());
                activate = false;
            }
        }
        if (isStriking)
        {
            isStriking = false;
            CinemachineShake.instance.ShakeCamera(intensityLightCloseDamage, 
                frequencyLightCloseDamage ,timerLightCloseDamage);
        }
    }
    
    IEnumerator Turn()
    {
        PrepTrourne();
        
        yield return new WaitForSeconds(loadingCoolDown[loadingCoolDownIndex]);
        
        Tourne();

        yield return new WaitForSeconds(timerRemaining);
        
        FinTourne();
    }

    void PrepTrourne()
    {
        PlayerController.instance.speedMovement = 50;
    }
    
    void Tourne()
    {
        PlayerLightAttack.instance.enabled = false;
        PlayerThrowAttack.instance.enabled = false;
        
        if (!ItemManager.instance.canMoveWhileBeyblade)
        {
            PlayerController.instance.enabled = false;
        }

        /*if (ItemManager.instance.beybladeInvinsible)
        {
            Physics2D.IgnoreLayerCollision(0,6, true);
            Physics2D.IgnoreLayerCollision(0,7, true);
        }*/
        
        isCoolDown = true;
        isKeyUp = false;

        
        StartCoroutine(CoolDown());
    }

    IEnumerator CoolDown()
    {
        for (int i = 0; i < numberOfTurn[numberOfTurnIndex]; i++)
        {
            coolDownIndex = i;
            
            yield return new WaitForSeconds(coolDown[coolDownIndex]);
            HeavyAttackCollision.instance.sprite.enabled = true;
            HeavyAttackCollision.instance.coll.enabled = true;
    
            yield return new WaitForSeconds(coolDown[coolDownIndex + 1]);

            HeavyAttackCollision.instance.sprite.enabled = false;
            HeavyAttackCollision.instance.coll.enabled = false;
        }
    }

    void FinTourne()
    {
        PlayerController.instance.SecureSO();
        PlayerLightAttack.instance.enabled = true;
        PlayerThrowAttack.instance.enabled = true;
        PlayerController.instance.enabled = true;
        
        isCoolDown = false;
        
        HeavyAttackCollision.instance.sprite.enabled = false;
        HeavyAttackCollision.instance.coll.enabled = false;
        
        activate = true;
        isKeyUp = true;
        Physics2D.IgnoreLayerCollision(0,6, false);
        Physics2D.IgnoreLayerCollision(0,7, false);
    }
}
