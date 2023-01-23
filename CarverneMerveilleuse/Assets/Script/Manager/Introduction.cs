using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

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
    [SerializeField] private DiscussionTrigger _discussionTrigger;

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
    [SerializeField] private GameObject _localisationOfArchimageDash;
    [Space]
    [SerializeField] private GameObject _lightEclairage;
    [SerializeField]private float _globalLigthFloat;

    [Space] 
    [SerializeField] private CanvasGroup _IntroBD;
    public CanvasGroup _MoneyPanel;
    public CanvasGroup canvasAfficage;

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

    [Space] 
    [Header("TUTO Combat 1")] 
    public List<GameObject> ennemyAlive;
    public GameObject[] posEnnemy1;
    public GameObject[] posEnnemy2;

    public GameObject TUTODash;
    public GameObject DoorTuto;
    
    private int random;
    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        if (instance != null && instance != this) 
        {
            Destroy(gameObject);
        } 
        else 
        { 
            instance = this; 
        }

        if (VerifIntro.instance.compte != 0)
        {
            VerifIntro.instance.isis = false;
            random = Random.Range(16, 23 + 1);
            _discussionTrigger.indexDialogue = random;
        }
        
    }

    private bool dialogueSecondaire;
    
    public void Start2()
    {
        DoorTuto.SetActive(false);
        DoorTuto.GetComponentInChildren<SpriteRenderer>().DOFade(0, EnnemyManager.instance.timeToCloseDoor);
        DoorTuto.GetComponentInChildren<Collider2D>().enabled = false;
        AudioManager.instance.PlayCave();
        _animatorPlayer.enabled = false;
        archimage.transform.position = _localisationOfArchimage.transform.position;
        //archimage.GetComponent<Collider2D>().enabled = false;
        CHara1.SetActive(false);
        CHara2.SetActive(true);
        canvasAfficage.DOFade(1, 1.25f);
        Destroy(TUTODash);
        dialogueSecondaire = true;
        
        
        StartCoroutine(Tutotime2(0.5f));
    }

    public void Start1()
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
            canvasAfficage.DOFade(1, 1.25f);
        }
    }
    
    private void Start()
    {
        /*if (playIntro)
        {
            _Introduction();
            //Tuto();
        }
        else
        {
            AudioManager.instance.PlayCave();
            _animatorPlayer.enabled = false;
            archimage.transform.position = _localisationOfArchimage.transform.position;
            //archimage.GetComponent<Collider2D>().enabled = false;
            CHara1.SetActive(false);
            CHara2.SetActive(true);
        }*/
        canvasAfficage.DOFade(0, 0);
    }

    private bool isCombat1began = false;
    private bool isCombat2began = false;
    private void Update()
    {
        if (ennemyAlive.Count <= 0)
        {
            if (isCombat1began)
            {
                isCombat1began = false;
                StartCoroutine(Tutotime(1.5f));
            }

            if (isCombat2began)
            {
                isCombat2began = false;
                StartCoroutine(Tutotime(2f));
            }
        }

        if (lancerDeFaux)
        {
            if (PlayerThrowAttack.instance.isThrow)
            {
                lancerDeFaux = false;
                StartCoroutine(Tutotime(2));
            }
        }

        if (Tourne)
        {
            if (PlayerHeavyAttack.instance.isTourne)
            {
                Tourne = false;
                StartCoroutine(Tutotime(2));
            }
        }
        
        if (dialogueSecondaire)
        {
            //dialogueSecondaire = false;
            
        }
    }
    private void LateUpdate()
    {
        for(var i = ennemyAlive.Count - 1; i > -1; i--)
        {
            if (ennemyAlive[i] == null)
                ennemyAlive.RemoveAt(i);
        }
    }

    private bool verifIllu;
    private bool deplacement;
    private bool lancerDeFaux;
    private bool Tourne;
    
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

            switch (_discussionTrigger.indexDialogue)
            {
                case 2 :
                    if (!deplacement)
                    {
                        
                        archimage.transform.DOMove(_localisationOfArchimage.transform.position, 3);
                        deplacement = true;
                    }
                    StartCoroutine(Tutotime(1.5f));

                    break;
                case 3:
                    StartCoroutine(Tutotime(1.5f));
                    
                    break;
                case 4 :
                    StartCoroutine(Tutotime(1.25f));
                    break;
                case 5:
                    StartCoroutine(Tutotime(2f));
                    TutoCombat1();
                    break;
                case 7 :
                    StartCoroutine(Tutotime(2f));
                    break;
                case 8 :
                    StartCoroutine(Tutotime(2f));
                    break;
                case 9 :
                    StartCoroutine(Tutotime(2f));
                    lancerDeFaux = true;
                    break;
                case 11 :
                    StartCoroutine(Tutotime(2f));
                    Tourne = true;
                    break;
                case 13 :
                    StartCoroutine(Tutotime(2f));
                    break;
                case 14 :
                    StartCoroutine(Tutotime(3f));
                    TutoCombat2();
                    break;
                case 15:
                    EndTUTO();
                    break;
            }
        }
    }

    IEnumerator AfterIllustration()
    {
        yield return new WaitForSeconds(22);
        _dialogueTrigger.TriggerDialogue();
    }

    public void _Introduction()
    {
        DoorTuto.SetActive(true);
        DoorTuto.GetComponentInChildren<SpriteRenderer>().DOFade(0, EnnemyManager.instance.timeToCloseDoor);
        DoorTuto.GetComponentInChildren<Collider2D>().enabled = true;
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
        DoorTuto.GetComponentInChildren<Collider2D>().enabled = false;
        archimage.transform.DOMove(_localisationOfArchimageDash.transform.position, 3);
        //playIntro = false;
    }

    IEnumerator Tutotime(float time)
    {
        
        yield return new WaitForSeconds(time);
        _discussionTrigger.TriggerTuto();
    }
    
    IEnumerator Tutotime2(float time)
    {
        
        yield return new WaitForSeconds(time);
        _discussionTrigger.TriggerTuto2(random);
    }
    
    void LookForEnnemyAlive()
    {
        addEnnemyToList("CAC");
        addEnnemyToList("Dist");
        addEnnemyToList("Gros");
    }
    void addEnnemyToList(string tag)
    {
        foreach (var go in GameObject.FindGameObjectsWithTag(tag))
        {
            ennemyAlive.Add(go);
        }
    }

    void TutoCombat1()
    {
        foreach (var i in posEnnemy1)
        {
            Instantiate(EnnemyManager.instance.SpawningVFX, i.transform.position, Quaternion.identity, transform);
            GameObject mechant = Instantiate(EnnemyManager.instance.spider, i.transform.position, Quaternion.identity, transform);
            mechant.GetComponent<CaCEnnemiScript>().isInvokeByArch = true;
            AudioManager.instance.PlaySpawn();
        }
        LookForEnnemyAlive();
        isCombat1began = true;
    }
    
    void TutoCombat2()
    {
        foreach (var i in posEnnemy2)
        {
            Instantiate(EnnemyManager.instance.SpawningVFX, i.transform.position, Quaternion.identity, transform);
            GameObject mechant = Instantiate(EnnemyManager.instance.spider, i.transform.position, Quaternion.identity, transform);
            mechant.GetComponent<CaCEnnemiScript>().isInvokeByArch = true;
            AudioManager.instance.PlaySpawn();
        }
        LookForEnnemyAlive();
        isCombat2began = true;
    }

    private bool yakari;
    public void EndIntro()
    {
        _MoneyPanel.DOFade(1,2);
        _animatorPlayer.enabled = false;
        //playIntro = false;
        
        _virtualCamera.Follow = _targetMain.GetComponent<Transform>();
        _virtualCamera.m_Lens.OrthographicSize = 10;
        GlobalLight.intensity = 0.6f;
        
        _flecheDirection.SetActive(true);
        _ligneViser.SetActive(true);
        _ligneViser.GetComponent<SpriteRenderer>().DOFade(0, 0);
        canvasAfficage.DOFade(1, 1.25f);
        Tuto();
        /*if (!yakari)
        {
            yakari = true;
            _discussionTrigger.TriggerTuto();
        }*/
        
        //archimage.transform.DOMove(_localisationOfArchimage.transform.position, 5);
        //archimage.GetComponent<Collider2D>().enabled = false;
    }

    void EndTUTO()
    {
        //playIntro = false;
        VerifIntro.instance.compte++;
        DoorTuto.SetActive(false);
        DoorTuto.GetComponentInChildren<SpriteRenderer>().DOFade(0, EnnemyManager.instance.timeToCloseDoor);
        DoorTuto.GetComponentInChildren<Collider2D>().enabled = false;
    }
}
