using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
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
    [SerializeField] private CinemachineVirtualCamera _virtualCameraPlayer;
    [SerializeField] private CinemachineVirtualCamera _virtualCameraArchimage;
    [SerializeField] private CinemachineTargetGroup _targetMain;
    [SerializeField] private CinemachineTargetGroup _targetIntro;
    
    [Space]
    [SerializeField] private GameObject _lightEclairage;
    [SerializeField]private float _globalLigthFloat;

    [Space] 
    [SerializeField] private CanvasGroup _IntroBD;

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
                    _virtualCamera.Follow = _Player.transform;
                    _virtualCamera.m_Lens.OrthographicSize = 2f;
                    break;
                case 4:
                    _lightEclairage.GetComponent<Light2D>().enabled = true;
                    _lightEclairage.GetComponent<LightAnimationCurve>().enabled = true;
                    _virtualCamera.Follow = _archimage.transform;
                    _virtualCamera.m_Lens.OrthographicSize = 2f;
                    break;
                case 5 : 
                    _virtualCamera.Follow = _Player.transform;
                    _virtualCamera.m_Lens.OrthographicSize = 2f;
                    break;
                case 6 :
                    _virtualCamera.Follow = _archimage.transform;
                    _virtualCamera.m_Lens.OrthographicSize = 2f;
                    break;
                case 7 : 
                    _virtualCamera.Follow = _Player.transform;
                    _virtualCamera.m_Lens.OrthographicSize = 2f;
                    break;
                case 8 :
                    _virtualCamera.Follow = _targetIntro.transform;
                    _virtualCamera.m_Lens.OrthographicSize = 7f;
                    break;
                case 9:
                    _virtualCamera.Follow = _Player.transform;
                    _virtualCamera.m_Lens.OrthographicSize = 2f;
                    break;
                case 10 : 
                    _virtualCamera.Follow = _archimage.transform;
                    _virtualCamera.m_Lens.OrthographicSize = 2f;
                    break;
                case 11 : //Elu !!
                    _virtualCamera.Follow = _targetIntro.transform;
                    if (_virtualCamera.m_Lens.OrthographicSize < 15)
                    {
                        _virtualCamera.m_Lens.OrthographicSize += 3.5f * Time.deltaTime;
                    }
                    
                    break;
                case 13 :
                    _virtualCamera.m_Lens.OrthographicSize = 2f;
                    _virtualCamera.Follow = _archimage.transform;
                    break;
                case 14 : 
                    _virtualCamera.Follow = _Player.transform;
                    _virtualCamera.m_Lens.OrthographicSize = 2f;
                    break;
                case 15 :
                    _virtualCamera.Follow = _archimage.transform;
                    _virtualCamera.m_Lens.OrthographicSize = 2f;
                    break;
                case 16 :
                    _IntroBD.DOFade(1, 4);
                    break;
                case 22 : 
                    _IntroBD.DOFade(0, 4);
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
        _lightEclairage.GetComponent<Light2D>().enabled = false;
        _lightEclairage.GetComponent<LightAnimationCurve>().enabled = false;
        GlobalLight.intensity = 0.15f;

        Debug.Log(_dialogueTrigger.indexDialogue);

        //dialogue
        StartCoroutine(StartDialogue());
    }

    IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(0);
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
