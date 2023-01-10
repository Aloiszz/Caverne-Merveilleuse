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
    [SerializeField] private TextMeshProUGUI dashTxt;
    [SerializeField]private bool verif_dashbar;
    
    [SerializeField] private Image ChargeBar;
    [SerializeField] private TextMeshProUGUI Chargetxt;
    [SerializeField]private bool verif_Chargebar;
    [SerializeField]private float verif_float;


    [Space] [Space] [Space] [Space] [Header("Intro du jeu")]
    public bool playIntro;

    [Space]
    [SerializeField] private GameObject _flecheDirection; // Player viseur
    [SerializeField] private GameObject _ligneViser; // Player viseur

    [Space]
    [SerializeField] private GameObject _archimage; // Player viseur
    [SerializeField] private GameObject _Player; // Player viseur
    
    [Space] 
    public Light2D GlobalLight;
    [SerializeField] private DialogueTrigger _dialogueTrigger;

    [Space] 
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private CinemachineTargetGroup _targetMain;
    [SerializeField] private CinemachineTargetGroup _targetIntro;
    
    [Space]
    [SerializeField] private GameObject _lightEclairage;
    [SerializeField]private float _globalLigthFloat;
    
    
    
    
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

        if (playIntro)
        {
            Introduction();
        }
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
        } ;
    }

    IEnumerator AfficheHealthBar()
    {
        dashbar.DOFillAmount((float)1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        verif_dashbar = true;
    }

    void Introduction()
    {
        //Controller
        PlayerLightAttack.instance.enabled = false;
        PlayerHeavyAttack.instance.enabled = false;
        PlayerThrowAttack.instance.enabled = false;
        PlayerController.instance.enabled = false;
        _flecheDirection.SetActive(false);
        _ligneViser.SetActive(false);
        
        
        //Camera
        _virtualCamera.Follow = _Player.transform;
        //_virtualCamera.m_Lens.OrthographicSize = 5f;

        //Light
        _lightEclairage.SetActive(true);
        GlobalLight.intensity = 0.15f;
        
        
        //dialogue
        _dialogueTrigger.TriggerDialogue();

    }


    public void EndIntro()
    {
        _virtualCamera.Follow = _targetMain.GetComponent<Transform>();
        GlobalLight.intensity = 0.6f;
        
        _flecheDirection.SetActive(true);
        _ligneViser.SetActive(true);
    }
}
