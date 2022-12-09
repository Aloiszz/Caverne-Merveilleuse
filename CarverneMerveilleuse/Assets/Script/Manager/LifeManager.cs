using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class LifeManager : MonoBehaviour
{
    public SO_PlayerController PlayerControllerSO;
    
    [Header("Life")]
    [SerializeField] private Image life_Bar;
    [SerializeField] private TextMeshProUGUI lifeTxt;
    private bool verif;
    [SerializeField] private int maxLife;
    
    [Space]
    [Header("Rage")]
    [SerializeField] private Image life_Bar_Rage;
    [SerializeField] private float timeInRage = 3;
    [SerializeField] private float timeInRageMax;
    public bool isInRage;
    [SerializeField] private Volume globalVolume;
    
    [SerializeField] private Image r_key_img;
    [SerializeField] private int listScoreRageIndex = 0;

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
        r_key_img.DOFade(0, 0);
        
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
            life_Bar.DOFillAmount((float)PlayerController.instance.life / (float)PlayerController.instance.lifeDepard, 0.15f);
        }

        /*if (PlayerController.instance.life > PlayerController.instance.lifeDepard)
        {
            isInRage = true;
        }*/

        life_Bar_Rage.DOFillAmount((float)Score.instance.score / (float)Score.instance.listScoreRage[listScoreRageIndex], 0.15f);
        if (Score.instance.score > Score.instance.listScoreRage[listScoreRageIndex] || PlayerController.instance.life > PlayerController.instance.lifeDepard)
        {
            r_key_img.DOFade(1, .2f);
            if (Input.GetKeyDown(KeyCode.R))
            {
                listScoreRageIndex++;
                r_key_img.DOFade(0, .2f);
                isInRage = true;
                //PlayerController.instance.life++;
            }
        }
        

        if (isInRage)
        {
            timeInRage -= 1 * Time.deltaTime;
            life_Bar_Rage.DOFillAmount(timeInRage / timeInRageMax, 0.15f);

            if (globalVolume.weight <= 1)
            {
                globalVolume.weight += 2 * Time.deltaTime;
            }
            if (timeInRage <= 0)
            {
                timeInRage = 3;
                isInRage = false;

                if (PlayerController.instance.life >= PlayerController.instance.lifeDepard)
                {
                    PlayerController.instance.life = PlayerController.instance.lifeDepard;
                }
            }
        }
        else
        {
            if (globalVolume.weight >= 0)
            {
                globalVolume.weight -= 2 * Time.deltaTime;
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