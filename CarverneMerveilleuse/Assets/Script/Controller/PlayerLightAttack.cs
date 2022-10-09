using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerLightAttack : MonoBehaviour
{
    
    [Header("Player Light Attack config")]
    public SO_PlayerLightAttack playerLightAttack;

    public int countInput = 0;
    public bool activate = false;
    public float timerRemainingStored;

    public static PlayerLightAttack instance;

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

        timerRemainingStored = playerLightAttack.timerRemaining;
    }


    private void Update()
    {
        LightAttack();
        
        TimeRemaining(activate);
    }

    private void LightAttack()
    {
        if (!playerLightAttack.isCoolDown)
        {
            if (Input.GetMouseButtonDown(0))
            {
                activate = false;
                countInput++;
                timerRemainingStored = playerLightAttack.timerRemaining ; // recupération du timer
                if (countInput <= playerLightAttack.combo)
                {
                    StartCoroutine(CoolDown());
                }
                else
                {
                    StartCoroutine(CoolDownEndCombo());
                    countInput = 0;
                }
            }
        }
        if (playerLightAttack.isStriking)
        {
            playerLightAttack.isStriking = false;
            CinemachineShake.instance.ShakeCamera(playerLightAttack.intensityLightCloseDamage, playerLightAttack.frequencyLightCloseDamage ,playerLightAttack.timerLightCloseDamage);
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
        playerLightAttack.isCoolDown = true;
        yield return new WaitForSeconds(playerLightAttack.coolDown);
        playerLightAttack.isCoolDown = false;
        PlayerAttackCollision.instance.sprite.enabled = false;
        PlayerAttackCollision.instance.coll.enabled = false;    
        activate = true;
    }
    
    IEnumerator CoolDownEndCombo()
    {
        PlayerAttackCollision.instance.sprite.enabled = false;
        PlayerAttackCollision.instance.coll.enabled = false;
        playerLightAttack.isCoolDown = false;
        yield return new WaitForSeconds(playerLightAttack.coolDownEndCombo);
    }
}
