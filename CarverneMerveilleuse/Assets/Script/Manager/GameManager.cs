using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Image dashbar;
    [SerializeField] private TextMeshProUGUI dashTxt;
    [SerializeField]private bool verif_dashbar;
    
    [SerializeField] private Image ChargeBar;
    [SerializeField] private TextMeshProUGUI Chargetxt;
    [SerializeField]private bool verif_Chargebar;
    [SerializeField]private float verif_float;
    

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

        if (Input.GetKey(KeyCode.Mouse0))
        {
            verif_float += 1 * Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            verif_float = 0;
        }

        
    }

    IEnumerator AfficheHealthBar()
    {
        dashbar.DOFillAmount((float)1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        verif_dashbar = true;
    }

    
}
