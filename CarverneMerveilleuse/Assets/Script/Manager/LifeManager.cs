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
    [SerializeField] private Image life_Bar_RageScore; // dépend du scoreRage
    [SerializeField] private Image life_Bar_RageLife; // Dépend de la vie
    [SerializeField] private float timeInRage = 3;
    private float timeInRageMax;
    public bool isInRage;
    [SerializeField]private bool rageBarScore; // savoir juste quelle bar utilisé entre bar life et bar score
    [SerializeField]private bool rageBarLife;
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
        
        maxLife = PlayerControllerSO.life;
        timeInRageMax = timeInRage;
    }

    private void Update()
    {
        
        if (verif)
        {
            life_Bar.DOFillAmount((float)PlayerController.instance.life / (float)PlayerController.instance.lifeDepard, 0.3f);
        }
        
        life_Bar_RageScore.DOFillAmount((float)Score.instance.scoreRage / (float)Score.instance.listScoreRage[listScoreRageIndex], 0.3f);

        RageDueToLife();
        RageDueToScoreRage();

        if (isInRage)
        {
            timeInRage -= 1 * Time.deltaTime;
            if (rageBarScore)
            {
                life_Bar_RageScore.DOFillAmount(timeInRage / timeInRageMax, 0);
                StartCoroutine(WaitForRageScoreBar());
            }
            if(rageBarLife)
            {
                life_Bar_RageLife.DOFillAmount(timeInRage / timeInRageMax, 0);
            }

            if (globalVolume.weight <= 1)
            {
                globalVolume.weight += 4 * Time.deltaTime;
            }
            if (timeInRage <= 0)
            {
                timeInRage = 3;
                isInRage = false;
                Score.instance.scoreRage = 0;
                r_key_img.DOFade(0, 0);
            }
        }
        else
        {
            if (globalVolume.weight >= 0)
            {
                globalVolume.weight -= 2 * Time.deltaTime;
            }
        }
    }



    void RageDueToLife()
    {
        if (PlayerController.instance.life > PlayerController.instance.lifeDepard) // rage quand surplus de vie
        {
            life_Bar_RageLife.DOFillAmount((float)1, 1);
            r_key_img.DOFade(1, .2f);
            if (Input.GetKeyDown(KeyCode.R))
            {
                r_key_img.DOFade(0, .2f);
                isInRage = true;
                rageBarLife = true;
                if (PlayerController.instance.life >= PlayerController.instance.lifeDepard)
                {
                    PlayerController.instance.life = PlayerController.instance.lifeDepard;
                }
                else
                {
                    PlayerController.instance.life++;
                }
            }
        }
        else
        {
            r_key_img.DOFade(0, .2f);
        }

    }

    void RageDueToScoreRage()
    {
        if (Score.instance.scoreRage > Score.instance.listScoreRage[listScoreRageIndex]) // rage quand score de rage atteint
        {
            r_key_img.DOFade(1, .2f);
            if (Input.GetKeyDown(KeyCode.R))
            {
                listScoreRageIndex++;
                r_key_img.DOFade(0, .2f);
                isInRage = true;
                rageBarScore = true;
                if (PlayerController.instance.life >= PlayerController.instance.lifeDepard)
                {
                    PlayerController.instance.life = PlayerController.instance.lifeDepard;
                }
                else
                {
                    PlayerController.instance.life++;
                }
            }
        }
        else
        {
            r_key_img.DOFade(0, .2f);
        }
    }

    IEnumerator WaitForRageScoreBar()
    {
        yield return new WaitForSeconds(3);
        rageBarScore = false;
        rageBarLife = false;
    }


    IEnumerator AfficheHealthBar()
    {
        life_Bar.DOFillAmount((float)1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        verif = true;
    }
}