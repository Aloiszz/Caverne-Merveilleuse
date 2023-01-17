using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.VFX;

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
    [HideInInspector] public int heavyDamageIndex;

    [Header("Tourne ")]
    public List<float> loadingCoolDown; // Préparation de l'attaque
    public int loadingCoolDownIndex;
    public List<float> coolDown; // temps entre chaque coups
    public int coolDownIndex;
    public float timerRemaining; // temps restant entre chaque coup pour arriver au combo
    [HideInInspector] public List<int> numberOfTurn; //nombre de tour que peut faire l'attaque
    [HideInInspector] public int numberOfTurnIndex;

    [Header("Cinemachine Schake")] 
    private float intensityLightCloseDamage;
    private float frequencyLightCloseDamage;
    private float timerLightCloseDamage;

    private bool isAlreadyScaled;

    [Header("Animator")] 
    public bool isCharge;
    
    [Header("VFX")] 
    public VisualEffect Slash;
    
    [Header("Audio")]
    public List<AudioClip> audioSlashNoHit;
    public List<AudioClip> audioSlashHit;
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
        /*if (Input.GetKeyUp(KeyCode.Mouse0) && isKeyUp) // annule l'attaque lourde
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
        }*/
        
        
        if (!isCoolDown)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartCoroutine(WaitPrep());
                isCharge = true;
            }

            if (activate)
            {
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    StartCoroutine(Turn());
                    isCharge = false;
                }
            }
            else
            {
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    activate = false;
                    StopAllCoroutines();
                    isCharge = true;
                }
            }
        }
        
    }

    IEnumerator WaitPrep()
    {
        //PlayerController.instance.speedMovement = 50;
        yield return new WaitForSeconds(loadingCoolDown[loadingCoolDownIndex]);
        activate = true;
        AudioManager.instance.PlayCloche();
    }
    
    IEnumerator Turn()
    {
        /*PrepTrourne();
        
        yield return new WaitForSeconds(loadingCoolDown[loadingCoolDownIndex]);*/
        
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
        Slash.Play();
        PlayerLightAttack.instance.enabled = false;
        PlayerThrowAttack.instance.enabled = false;
        
        if (!ItemManager.instance.canMoveWhileBeyblade)
        {
            //PlayerController.instance.enabled = false;
            PlayerController.instance.speedMovement = 50;
        }

        if (ItemManager.instance.beybladeInvinsible)
        {
            Physics2D.IgnoreLayerCollision(0,6, true);
            Physics2D.IgnoreLayerCollision(0,7, true);
        }
        
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
        PlayerLightAttack.instance.enabled = true;
        PlayerThrowAttack.instance.enabled = true;
        //²PlayerController.instance.enabled = true;
        
        isCoolDown = false;
        
        HeavyAttackCollision.instance.sprite.enabled = false;
        HeavyAttackCollision.instance.coll.enabled = false;

        activate = false;
        isKeyUp = true;
        Physics2D.IgnoreLayerCollision(0,6, false);
        Physics2D.IgnoreLayerCollision(0,7, false);
        Physics2D.IgnoreLayerCollision(0,7, false);

        PlayerController.instance.speedMovement = 90;
    }
}
