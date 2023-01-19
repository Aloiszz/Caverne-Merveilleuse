using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Image dashbar;
    [SerializeField] private Image dashbarUI;
    [SerializeField] private TextMeshProUGUI dashTxt;
    [SerializeField]private bool verif_dashbar;
    
    [SerializeField] private Image ChargeBar;
    [SerializeField] private Image ChargeBarUI;
    [SerializeField] private TextMeshProUGUI Chargetxt;
    [SerializeField]private bool verif_Chargebar;
    [SerializeField]private float verif_float;
    
    [SerializeField]private bool GodMode;
    
    public static GameManager instance;
    
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
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (verif_dashbar)
        {
            if (PlayerController.instance.isDashing)
            {
                dashbar.DOFillAmount((float)0,0);
                dashbar.DOFillAmount((float)1,PlayerController.instance.dashReload);
            }
        }

        ChargeBar.DOFillAmount(verif_float / PlayerHeavyAttack.instance.loadingCoolDown[PlayerHeavyAttack.instance.loadingCoolDownIndex], 0);

        if (verif_float / PlayerHeavyAttack.instance.loadingCoolDown[PlayerHeavyAttack.instance.loadingCoolDownIndex] < .9)
        {
            ChargeBar.rectTransform.DOScale(new Vector3(verif_float+1.3536f,verif_float+1.3536f,verif_float+1.3536f), 0.2f);
            ChargeBarUI.rectTransform.DOScale(new Vector3(verif_float+1.3536f,verif_float+1.3536f,verif_float+1.3536f), 0.2f);
        }
        
        if (Input.GetKey(KeyCode.Mouse0))
        {
            verif_float += 1 * Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            verif_float = 0;
        }

        if (GodMode)
        {
            PlayerController.instance.life = PlayerController.instance.lifeDepard;
        }
    }

    IEnumerator AfficheHealthBar()
    {
        dashbar.DOFillAmount((float)1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        verif_dashbar = true;
    }
    
}
