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
    [SerializeField] private CinemachineVirtualCamera _virtualCameraEnsemble;
    [SerializeField] private CinemachineVirtualCamera _virtualCameraElue;
    [SerializeField] private CinemachineTargetGroup _targetMain;
    [SerializeField] private CinemachineTargetGroup _targetIntro;
    
    [SerializeField] private GameObject archimage;
    [SerializeField] private GameObject _localisationOfArchimage;
    [Space]
    [SerializeField] private GameObject _lightEclairage;
    [SerializeField]private float _globalLigthFloat;

    [Space] 
    [SerializeField] private CanvasGroup _IntroBD;
    [SerializeField] private CanvasGroup _MoneyPanel;

    [Space] 
    [Header("Effet Camera")] [SerializeField]
    private List<int> _effetCamera;

    [Header("Chara Archimage")] 
    [SerializeField]
    public GameObject CHara1;
    [SerializeField]
    public GameObject CHara2;
    [SerializeField]
    public GameObject eye1;
    [SerializeField]
    public GameObject eye2;
    

    [Space] 
    [Header("Animator")] 
    [SerializeField]private Animator _animator;
    [SerializeField]private Animator _animatorIllustration;
    [SerializeField]private Animator _animatorPlayer;
    
    [Space] 
    [Header("Audio")] 
    [SerializeField]private AudioSource source;
    [SerializeField]private AudioClip ArchimageAudioEtouffe;
    [SerializeField]private AudioClip ArchimageAudioFier;
    [SerializeField]private AudioClip ArchimageAudioNormal;
    [SerializeField]private AudioClip ArchimageAudioNormalBis;
    [SerializeField]private AudioClip MaryTombe;
    
    public static Introduction instance;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance != null && instance != this) 
        {
            //Destroy(gameObject);
        } 
        else 
        { 
            instance = this; 
        }
    }
    
    
    private void Start()
    {
        

        if (playIntro)
        {
            //_Introduction();
            Tuto();
        }
        else
        {
            AudioManager.instance.PlayCave();
            _animatorPlayer.enabled = false;
            archimage.transform.position = _localisationOfArchimage.transform.position;
            //archimage.GetComponent<Collider2D>().enabled = false;
            CHara1.SetActive(false);
            CHara2.SetActive(true);
        }
    }

    private bool verifIllu;
    
    public void Dialogue()
    {
        if (playIntro)
        {
            switch (_dialogueTrigger.indexDialogue)
            {
                case 1 :
                    /*_virtualCamera.Follow = _Player.transform;
                    _virtualCamera.m_Lens.OrthographicSize = 2f;*/
                    _virtualCamera.Priority = 8;
                    _virtualCameraPlayer.Priority = 10;
                    PlayerController.instance.Source.PlayOneShot(PlayerController.instance.audioSoupir);
                    break;
                case 4:
                    _lightEclairage.GetComponent<Light2D>().enabled = true;
                    _lightEclairage.GetComponent<LightAnimationCurve>().enabled = true;
                    /*_virtualCamera.Follow = _archimage.transform;
                    _virtualCamera.m_Lens.OrthographicSize = 2f;*/
                    _virtualCameraPlayer.Priority = 8;
                    _virtualCameraArchimage.Priority = 10;
                    source.PlayOneShot(ArchimageAudioEtouffe, 0.5f);
                    break;
                case 5 : 
                    /*_virtualCamera.Follow = _Player.transform;
                    _virtualCamera.m_Lens.OrthographicSize = 2f;*/
                    _virtualCameraPlayer.Priority = 10;
                    _virtualCameraArchimage.Priority = 8;
                    PlayerController.instance.Source.PlayOneShot(PlayerController.instance.audioSurpris);
                    break;
                case 6 :
                    /*_virtualCamera.Follow = _archimage.transform;
                    _virtualCamera.m_Lens.OrthographicSize = 2f;*/
                    _virtualCameraPlayer.Priority = 8;
                    _virtualCameraArchimage.Priority = 10;
                    source.PlayOneShot(ArchimageAudioNormal, 0.5f);
                    break;
                case 7 : 
                    /*_virtualCamera.Follow = _Player.transform;
                    _virtualCamera.m_Lens.OrthographicSize = 2f;*/
                    _virtualCameraPlayer.Priority = 10;
                    _virtualCameraArchimage.Priority = 8;
                    PlayerController.instance.Source.PlayOneShot(PlayerController.instance.audioSurpris);
                    break;
                case 8 :
                    /*_virtualCamera.Follow = _targetIntro.transform;
                    _virtualCamera.m_Lens.OrthographicSize = 7f;*/
                    _virtualCameraPlayer.Priority = 8;
                    _virtualCameraEnsemble.Priority = 10;
                    source.PlayOneShot(ArchimageAudioEtouffe, 0.5f);
                    break;
                case 9:
                    /*_virtualCamera.Follow = _Player.transform;
                    _virtualCamera.m_Lens.OrthographicSize = 2f;*/
                    _virtualCameraPlayer.Priority = 10;
                    _virtualCameraEnsemble.Priority = 8;
                    PlayerController.instance.Source.PlayOneShot(PlayerController.instance.audioSoupir);
                    break;
                case 10 : 
                    /*_virtualCamera.Follow = _archimage.transform;
                    _virtualCamera.m_Lens.OrthographicSize = 2f;*/
                    _virtualCameraPlayer.Priority = 8;
                    _virtualCameraArchimage.Priority = 10;
                    source.PlayOneShot(ArchimageAudioNormalBis, 0.5f);
                    break;
                case 11 : //Elu !!
                    /*_virtualCamera.Follow = _targetIntro.transform;
                    if (_virtualCamera.m_Lens.OrthographicSize < 15)
                    {
                        _virtualCamera.m_Lens.OrthographicSize += 3.5f * Time.deltaTime;
                    }*/
                    _virtualCameraArchimage.Priority = 8;
                    _virtualCameraElue.Priority = 10;
                    source.PlayOneShot(ArchimageAudioFier, 0.5f);
                    break;
                case 13 :
                    /*_virtualCamera.m_Lens.OrthographicSize = 2f;
                    _virtualCamera.Follow = _archimage.transform;*/
                    _virtualCameraArchimage.Priority = 10;
                    _virtualCameraElue.Priority = 8;
                    source.PlayOneShot(ArchimageAudioEtouffe, 0.5f);
                    break;
                case 14 : 
                    /*_virtualCamera.Follow = _Player.transform;
                    _virtualCamera.m_Lens.OrthographicSize = 2f;*/
                    _virtualCameraPlayer.Priority = 10;
                    _virtualCameraEnsemble.Priority = 8;
                    PlayerController.instance.Source.PlayOneShot(PlayerController.instance.audioNanana);
                    break;
                case 15 :
                    /*_virtualCamera.Follow = _archimage.transform;
                    _virtualCamera.m_Lens.OrthographicSize = 2f;*/
                    _virtualCameraPlayer.Priority = 8;
                    _virtualCameraArchimage.Priority = 10;
                    source.PlayOneShot(ArchimageAudioNormal, 0.5f);
                    break;
                case 16 :
                    _IntroBD.DOFade(1, 4);
                    _animator.SetBool("isOpen", false);
                    _animatorIllustration.SetTrigger("Play");
                    source.PlayOneShot(ArchimageAudioNormalBis, 0.5f);
                    PlayerController.instance.Source.PlayOneShot(PlayerController.instance.audioSurpris);
                    eye1.transform.DOScale(3, 2f);
                    eye2.transform.DOScale(3, 2f);
                    if (!verifIllu)
                    {
                        verifIllu = true;
                        StartCoroutine(AfterIllustration());
                    }
                    break;
                
                case 17:
                    CHara1.SetActive(false);
                    CHara2.SetActive(true);
                    _IntroBD.DOFade(0, 2);
                    _animator.SetBool("isOpen", true);
                    _virtualCameraPlayer.Priority = 8;
                    _virtualCameraArchimage.Priority = 10;
                    source.PlayOneShot(ArchimageAudioFier, 0.5f);
                    break;
                case 18:
                    _virtualCameraPlayer.Priority = 10; 
                    _virtualCameraArchimage.Priority = 8; 
                    PlayerController.instance.Source.PlayOneShot(PlayerController.instance.audioSoupir);
                    break;
                case 19 : 
                    _virtualCameraPlayer.Priority = 8;
                    _virtualCameraArchimage.Priority = 10;
                    source.PlayOneShot(ArchimageAudioNormal, 0.5f);
                    break;
                case 20 : 
                    _virtualCameraPlayer.Priority = 10;
                    _virtualCameraEnsemble.Priority = 8;
                    PlayerController.instance.Source.PlayOneShot(PlayerController.instance.audioSoupir);
                    break;
                case 21 :
                    _virtualCameraPlayer.Priority = 8;
                    _virtualCameraArchimage.Priority = 10;
                    source.PlayOneShot(ArchimageAudioNormal, 0.5f);
                    break;
                case 22 :
                    _virtualCameraPlayer.Priority = 8;
                    _virtualCameraArchimage.Priority = 8;
                    _virtualCamera.Priority = 10;
                    break;
            }
        }
    }

    IEnumerator AfterIllustration()
    {
        yield return new WaitForSeconds(22);
        _dialogueTrigger.TriggerDialogue();
    }

    void _Introduction()
    {
        CHara1.SetActive(true);
        CHara2.SetActive(false);
        
        //AudioManager.instance.PlayNoCombatMusic();
        _animatorPlayer.enabled = true;
        SceneManager.instance.playModeCG_.DOFade(0, 0);
        _MoneyPanel.DOFade(0,0);
        
        //Controller
        /*PlayerController.instance.transform.position = transform.position;
        PlayerController.instance.transform.DORotate(new Vector3(0,0,-1170), 10);
        PlayerController.instance.transform.DOMove(new Vector3(-2.52f, 22.77f, 0), 10);*/
        
        PlayerController.instance.enabled = false;
        PlayerLightAttack.instance.enabled = false;
        PlayerHeavyAttack.instance.enabled = false;
        PlayerThrowAttack.instance.enabled = false;
        _flecheDirection.SetActive(false);
        _ligneViser.SetActive(false);
        
        
        //Camera
        //_virtualCamera.Follow = _Player.transform;
        _virtualCamera.m_Lens.OrthographicSize = 5f;

        //Light
        _lightEclairage.GetComponent<Light2D>().enabled = false;
        _lightEclairage.GetComponent<LightAnimationCurve>().enabled = false;
        GlobalLight.intensity = 0.15f;

        //dialogue
        StartCoroutine(ScreenShake());
        StartCoroutine(StartDialogue());
        //Dialogue();
    }
    
    
    IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(7);
        _dialogueTrigger.TriggerDialogue();
        _virtualCamera.Follow = _Player.transform;
    }
    IEnumerator ScreenShake()
    {
        yield return new WaitForSeconds(3.47f);
        CinemachineShake.instance.ShakeCamera(2f,2f,.3f);
        source.PlayOneShot(MaryTombe);
    }

    public void Tuto()
    {
        Debug.Log("AHHAHAHAHAH");
        archimage.transform.DOMove(_localisationOfArchimage.transform.position, 5);
        playIntro = false;
    }


    public void EndIntro()
    {
        _MoneyPanel.DOFade(1,2);
        _animatorPlayer.enabled = false;
        playIntro = false;
        
        _virtualCamera.Follow = _targetMain.GetComponent<Transform>();
        GlobalLight.intensity = 0.6f;
        
        _flecheDirection.SetActive(true);
        _ligneViser.SetActive(true);
        _ligneViser.GetComponent<SpriteRenderer>().DOFade(0, 0);

        archimage.transform.DOMove(_localisationOfArchimage.transform.position, 5);
        //archimage.GetComponent<Collider2D>().enabled = false;
    }
}
