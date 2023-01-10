using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Introduction : MonoBehaviour
{
    [Header("Intro du jeu")]
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

    [Space] 
    [Header("Effet Camera")] [SerializeField]
    private List<int> _effetCamera;

    public static Introduction instance;
    
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
        DontDestroyOnLoad(this);

        if (playIntro)
        {
            _Introduction();
        }
    }

    private void Update()
    {
        if (playIntro)
        {
            switch (_dialogueTrigger.indexDialogue)
            {
                case 1 :
                    _virtualCamera.m_Lens.OrthographicSize = 12f;
                    break;
                case 3:
                    _virtualCamera.m_Lens.OrthographicSize = 50f;
                    break;
            }
        }
    }

    void _Introduction()
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
        _virtualCamera.m_Lens.OrthographicSize = 5f;

        //Light
        _lightEclairage.SetActive(true);
        GlobalLight.intensity = 0.15f;
        Debug.Log(_dialogueTrigger.dialogue.Count);
        
        
        //dialogue
        _dialogueTrigger.TriggerDialogue();

    }


    public void EndIntro()
    {
        playIntro = false;
        
        _virtualCamera.Follow = _targetMain.GetComponent<Transform>();
        GlobalLight.intensity = 0.6f;
        
        _flecheDirection.SetActive(true);
        _ligneViser.SetActive(true);
    }
}
