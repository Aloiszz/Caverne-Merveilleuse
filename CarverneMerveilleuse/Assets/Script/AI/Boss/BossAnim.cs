using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class BossAnim : MonoBehaviour
{

    [Header("Camera")] 
    public CinemachineVirtualCamera vcamMain;
    public CinemachineVirtualCamera vcamBoss;

    public CinemachineTargetGroup TargetGroup;
    public GameObject PointCameraBoss;
    public GameObject CameraPoint;
    
    [Header("Graph")] 
    public GameObject BossPhase1;
    public GameObject BossPhase2;
    
    [Header("animator")] 
    public List<Animator> BossAnimator;
    public int BossAnimatorIndex;

    public bool BossZoneCac; // Attaque de zone
    public bool BossCacLeft; // Attaque de zone Left
    public bool BossCacRight; // Attaque de zone Right

    public Animator animatorEnding;
    public static BossAnim instance;

    public GameObject QuitToMenu;

    public CanvasGroup AngeText;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        QuitToMenu.SetActive(false);
        vcamMain = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        vcamBoss = GameObject.Find("CM vcam Boss").GetComponent<CinemachineVirtualCamera>();
        TargetGroup = GameObject.Find("CM TargetBoss").GetComponent<CinemachineTargetGroup>();
        PointCameraBoss = GameObject.Find("PointCameraBoss");
        CameraPoint = GameObject.Find("CameraPoint");
        
        BossPhase2.SetActive(false);
        BossAnimatorIndex = 0;

        vcamMain.Priority = 8;
        vcamBoss.Priority = 10;

        TargetGroup.m_Targets[1].target = CameraPoint.transform;
        
        CinemachineShake.instance.cinemachineVirtualCamera = vcamBoss;
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            TargetGroup.m_Targets[0].radius = 0.7f;
            PointCameraBoss.transform.localPosition = new Vector3(0,8,0);
            CameraPoint.transform.localPosition = new Vector3(.14f, -1.27f, 0);
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            TargetGroup.m_Targets[0].radius = 0.7f;
            PointCameraBoss.transform.localPosition = new Vector3(0,-8,0);
            CameraPoint.transform.localPosition = new Vector3(.14f, 1.27f, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        BossPhaseChange();
        ZoneCac();

        if (canReturn)
        {
            if (Input.anyKeyDown)
            {
                QuitToMenu.SetActive(true);
                QuitToMenu.GetComponent<CanvasGroup>().DOFade(1, 1.25f);
            }
        }
        //TargetGroup.m_Targets[0].radius.DOFloat(10, .5f);
    }

    private bool _isVierge = false;
    private bool _isVierge2 = false;
    private bool isThrow = false;
    void BossPhaseChange()
    {
        if (BossScript.instance.is2Phase)
        {
            if (!_isVierge)
            {
                _isVierge = true;
                BossAnimatorIndex++;
                BossPhase1.SetActive(false);
                BossPhase2.SetActive(true);
            }
        }
        
        if (BossScript.instance.mechantScript.life <= 0)
        {
            if (!_isVierge2)
            {
                _isVierge2 = true;
                DeathBoss();
            }
        }

        if (PlayerThrowAttack.instance.isThrow)
        {
            if (!isThrow)
            {
                isThrow = true;
                BossAnimator[BossAnimatorIndex].SetTrigger("Protection");
            }
        }
        else
        {
            isThrow = false;
        }
    }


    void ZoneCac()
    {
        if (BossZoneCac)
        {
            BossAnimator[BossAnimatorIndex].SetTrigger("AttaqueDeuxMains");
        }

        if (BossCacLeft)
        {
            BossAnimator[BossAnimatorIndex].SetTrigger("AttaqueLeft");
        }
        
        if (BossCacRight)
        {
            BossAnimator[BossAnimatorIndex].SetTrigger("AttaqueRight");
        }
    }

    private bool canReturn;
    void DeathBoss()
    {
        vcamMain.Priority = 10;
        vcamBoss.Priority = 8;
        
        SceneManager.instance.playModeCG_.DOFade(0, 1.25f);
        Introduction.instance._MoneyPanel.DOFade(0, 1.25f);
        AngeText.DOFade(0, 1.25f);

        GameManager.instance.GodMode = true;
        animatorEnding.SetTrigger("Ending");
        StartCoroutine(Ending());
        BossScript.instance.enabled = false;
        AudioManager.instance.PlayTheEnd();
        StartCoroutine(ReturnMenu());
        StartCoroutine(ReturnMenu2());
        
    }

    IEnumerator ReturnMenu()
    {
        yield return new WaitForSeconds(51);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    IEnumerator ReturnMenu2()
    {
        yield return new WaitForSeconds(11);
        canReturn = true;
    }

    IEnumerator Ending()
    {
        SceneManager.instance.ProtectFromJacques = true;
        yield return new WaitForSeconds(5);
        PlayerController.instance.enabled = false;
        PlayerLightAttack.instance.enabled = false;
        PlayerHeavyAttack.instance.enabled = false;
        PlayerThrowAttack.instance.enabled = false;
        SceneManager.instance.ProtectFromJacques = false;
    }
}
