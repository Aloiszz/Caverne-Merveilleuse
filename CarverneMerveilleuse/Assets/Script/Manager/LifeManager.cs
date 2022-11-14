using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class LifeManager : MonoBehaviour
{
    public SO_PlayerController PlayerControllerSO;
    
    [SerializeField] private Image life_Bar;
    [SerializeField] private TextMeshProUGUI lifeTxt;
    
    [SerializeField] private int maxLife;

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
        SecureSO();
        maxLife = PlayerControllerSO.life;
    }
    public void SecureSO()
    {
        
    }
    
    private void Update()
    {
        life_Bar.DOFillAmount((float)PlayerController.instance.life / (float)maxLife, 0.15f);
        
        //lifeTxt.text = current_life + " / " + nextLifeLevel;

       
    }
}