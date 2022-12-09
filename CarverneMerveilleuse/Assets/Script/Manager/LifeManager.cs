using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.Rendering;

public class LifeManager : MonoBehaviour
{
    public SO_PlayerController PlayerControllerSO;
    
    [SerializeField] private Image life_Bar;
    [SerializeField] private TextMeshProUGUI lifeTxt;
    private bool verif;
    [SerializeField] private int maxLife;
    
    
    [SerializeField] private Image life_Bar_Rage;
    [SerializeField] private float timeInRage = 3;
    [SerializeField] private float timeInRageMax;
    public bool isInRage;
    [SerializeField] private Volume globalVolume;
    

    public static LifeManager instance;
    
    private void Awake()
    {
        if (instance != null && instance != this) 
        {
            Destroy(gameObject);
        } 
        else 
        { 
            instance = this; 
        }
    }
    
    private void Start()
    {
        StartCoroutine(AfficheHealthBar());
        
        SecureSO();
        maxLife = PlayerControllerSO.life;
        timeInRageMax = timeInRage;
    }
    public void SecureSO()
    {
        
    }
    
    private void Update()
    {
        if (verif)
        {
            life_Bar.DOFillAmount((float)PlayerController.instance.life / (float)maxLife, 0.15f);
        }

        if (PlayerController.instance.life > maxLife)
        {
            isInRage = true;
            globalVolume.enabled = false;
        }

        if (isInRage)
        {
            timeInRage -= 1 * Time.deltaTime;
            life_Bar_Rage.DOFillAmount(timeInRage / timeInRageMax, 0.15f);
            globalVolume.enabled = true;
            if (timeInRage <= 0)
            {
                timeInRage = 3;
                isInRage = false;
                if (PlayerController.instance.life >= maxLife)
                {
                    PlayerController.instance.life = maxLife;
                    globalVolume.enabled = false;
                }
            }
            
        }

        //lifeTxt.text = current_life + " / " + nextLifeLevel;
        //ChargeBar.DOFillAmount(verif_float / PlayerHeavyAttack.instance.loadingCoolDown[PlayerHeavyAttack.instance.loadingCoolDownIndex], 0);
    }

    IEnumerator AfficheHealthBar()
    {
        life_Bar.DOFillAmount((float)1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        verif = true;
    }
}