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
    private bool verif;
    

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
    }

    private void Update()
    {
        if (verif)
        {
            if (PlayerController.instance.isDashing)
            {
                dashbar.DOFillAmount((float)0,0);
                dashbar.DOFillAmount((float)1,PlayerController.instance.playerSO.dashReload).SetEase(Ease.Linear);
            }
        }
    }

    IEnumerator AfficheHealthBar()
    {
        dashbar.DOFillAmount((float)1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        verif = true;
    }
}
