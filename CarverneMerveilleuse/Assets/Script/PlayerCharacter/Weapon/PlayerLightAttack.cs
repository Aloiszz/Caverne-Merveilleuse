using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Mathematics;

public class PlayerLightAttack : MonoBehaviour
{
    
    [Header("Player Light Attack config")]
    public SO_PlayerLightAttack playerLightAttack;

    public int countInput = 0;
    public bool activate = false;
    public float timerRemainingStored;

    public static PlayerLightAttack instance;
    
    [Header("Secure data Light Attack")]
    
    [Header("Verification")] 
    private bool isStriking;
    private bool isCoolDown = false;
    
    [Header("Damage")]
    [HideInInspector] public List<float> lightDamage;
    [HideInInspector]public int lightDamageIndex;
    [HideInInspector]public List<float> lastLightDamage;
    [HideInInspector]public int lastLightDamageIndex;
    
    [Header("Combo")]
    private List<float> coolDown; // temps entre chaque coups
    private int coolDownIndex;
    private List<float> coolDownEndCombo; // temps entre chaque combo
    private int coolDownEndComboIndex;
    private float timerRemaining; // temps restant entre chaque coup pour arriver au combo
    private int combo; //nombre de coup que peut enchainner le joueur avant le coolDown final

    [Header("Cinemachine Schake")] 
    private float intensityLightCloseDamage;
    private float frequencyLightCloseDamage;
    private float timerLightCloseDamage;


    private Vector3 mouseWorldPosition;
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

        //timerRemainingStored = playerLightAttack.timerRemaining;
        //playerLightAttack.isCoolDown = false;
    }
    
    void Start()
    {
        SecureSO();
    }
    
    void SecureSO()
    {
        isStriking = playerLightAttack.isStriking;
        isCoolDown = playerLightAttack.isCoolDown;
        
        lightDamage = playerLightAttack.lightDamage;
        lastLightDamage = playerLightAttack.lastLightDamage;


        coolDown = playerLightAttack.coolDown;
        coolDownEndCombo = playerLightAttack.coolDownEndCombo;
        timerRemaining = playerLightAttack.timerRemaining;
        combo = playerLightAttack.combo;

        
        intensityLightCloseDamage = playerLightAttack.intensityLightCloseDamage;
        frequencyLightCloseDamage = playerLightAttack.frequencyLightCloseDamage;
        timerLightCloseDamage = playerLightAttack.timerLightCloseDamage;
    }


    private void Update()
    {
        LightAttack();
        mouseWorldPosition = Camera.main.WorldToScreenPoint(transform.position);
        TimeRemaining(activate);
    }

    public void LightAttack()
    {
        if (!isCoolDown)
        {
            if (Input.GetMouseButtonDown(0))
            {
                activate = false;
                countInput++;
                timerRemainingStored = timerRemaining ; // recupération du timer
                if (countInput <= combo)
                {
                    StartCoroutine(CoolDown());
                    if (countInput == combo)
                    {
                        CinemachineCameraZoom.instance.CameraZoom(8f, 0.05f, 0.6f);
                        CinemachineShake.instance.ShakeCamera(3,3,0.5f);
                        Vector3 direction = (Vector3)(Input.mousePosition-mouseWorldPosition);
                        direction.Normalize();
                        PlayerController.instance.rb.AddForce(direction * 1200);
                        if (ItemManager.instance.isFenteAPGet)
                        {
                            StartCoroutine(FenteAP());
                        }
                    }
                }
                else
                {
                    StartCoroutine(CoolDownEndCombo());
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
        if (activate && timerRemainingStored >= 0)
        {
            timerRemainingStored -= Time.deltaTime;
        }

        if (timerRemainingStored <= 0)
        {
            countInput = 0;
        }
    }

    IEnumerator CoolDown()
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
    
    IEnumerator CoolDownEndCombo()
    {
        PlayerAttackCollision.instance.sprite.enabled = false;
        PlayerAttackCollision.instance.coll.enabled = false;
        isCoolDown = true;
        yield return new WaitForSeconds(coolDownEndCombo[coolDownEndComboIndex]); //- ItemManager.instance.endComboSoustracteur
        isCoolDown = false;
    }

    IEnumerator FenteAP()
    {
        GameObject fente = Instantiate(ItemManager.instance.fente, PlayerAttackCollision.instance.transform.position, PlayerAttackCollision.instance.transform.rotation);
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < ItemManager.instance.nbCoupFente; i++)
        {
            fente.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            fente.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(fente);
    }
}
