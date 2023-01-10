using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class MenuManager : MonoBehaviour
{
    public GameObject sceneProps;
    
    [Header("Panel Main Menu ")]
    public CanvasGroup cg_MenuMainMenu;
    public CanvasGroup cg_btn_Play;
    public CanvasGroup cg_btn_Option;
    public CanvasGroup cg_btn_Quit;
    
    [Header("GameObject Main Menu  ")]
    public GameObject go_MainMenu;
    public GameObject go_btn_Play;
    public GameObject go_btn_Option;
    public GameObject go_btn_Quit;
    
    [Space]
    [Header("Panel Option Menu ")]
    public CanvasGroup cg_OptionMenu;
    public CanvasGroup cg_btn_Score;
    public CanvasGroup cg_btn_Credit;
    public CanvasGroup cg_btn_Advanced;
    public CanvasGroup cg_btn_MainMenu;
    
    [Header("GameObject Option Menu  ")]
    public GameObject go_OptionMenu;
    public GameObject go_btn_Score;
    public GameObject go_btn_Credit;
    public GameObject go_btn_Advanced;
    public GameObject go_btn_MainMenu;
    
    [Space]
    [Header("Panel Credit")]
    public CanvasGroup cg_CreditMenu;
    public CanvasGroup cg_Credit;
    public CanvasGroup cg_btn_BackToOption;
    
    [Header("GameObject Option Menu  ")]
    public GameObject go_CreditMenu;
    public GameObject goCredit;
    public GameObject go_btn_BackToOption;

    [Space]

    public CinemachineVirtualCamera cam;
    public bool verifDutch;

    public Volume globalVolume;
    public GameObject Flaque;

    public bool verif;

    [Header("Animator")]
    public Animator animator;
    public static MenuManager instance;
    
    public void Awake()
    {
        if (instance != null && instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            instance = this; 
        }
        
    }
    
    void Start()
    {
        //sceneProps = GetComponentsInChildren<SpriteRenderer>;
        StartCoroutine(MainMenu());
        go_MainMenu.SetActive(false);
        go_btn_Play.SetActive(false);
        go_btn_Option.SetActive(false);
        go_btn_Quit.SetActive(false);

        cam.m_Lens.OrthographicSize = 100;
        cam.DOCinemachineOrthoSize(10, 10).SetEase(Ease.OutQuint);
        cam.m_Lens.Dutch = 0;
        verifDutch = false;
        
        // ----- option ------

        cg_OptionMenu.DOFade(0, 0);
        cg_btn_Score.DOFade(0, 0);
        cg_btn_Credit.DOFade(0, 0);
        cg_btn_Advanced.DOFade(0, 0);
        cg_btn_MainMenu.DOFade(0, 0);
        
        cg_btn_Score.GetComponent<Button>().interactable = false;
        cg_btn_Credit.GetComponent<Button>().interactable = false;
        cg_btn_Advanced.GetComponent<Button>().interactable = false;
        cg_btn_MainMenu.GetComponent<Button>().interactable = false;
        
        //----- credit -----
        cg_CreditMenu.DOFade(0, 0);
        cg_btn_Credit.DOFade(0, 0);
        cg_btn_BackToOption.DOFade(0, 0);
        
        go_btn_BackToOption.SetActive(false);
        
        cg_btn_BackToOption.GetComponent<Button>().interactable = false;
    }

    private void Update()
    {
        if (verifDutch)
        {
            //cam.m_Lens.Dutch += 25 * Time.deltaTime;
            
        }
    }

    IEnumerator MainMenu() // ouverture du menu
    {
        //cam.m_Lens.Dutch = 5;
        
        yield return new WaitForSeconds(2);
        go_MainMenu.SetActive(true);
        cg_MenuMainMenu.DOFade(1, 2);
            
        yield return new WaitForSeconds(5f);
        go_btn_Play.SetActive(true);
        cg_btn_Play.DOFade(1, 3);
        
        yield return new WaitForSeconds(1.5f);
        go_btn_Option.SetActive(true);
        cg_btn_Option.DOFade(1, 3);
        
        yield return new WaitForSeconds(1.5f);
        go_btn_Quit.SetActive(true);
        cg_btn_Quit.DOFade(1, 3);
    }

    public void Quit()
    {
        StartCoroutine(PlayAnimQuitGame());

        cg_MenuMainMenu.DOFade(0, 2);
        cg_btn_Play.DOFade(0, 2);
        cg_btn_Option.DOFade(0, 2);
        cg_btn_Quit.DOFade(0, 2);
        
    }

    public void Play()
    {
        StartCoroutine(PlayAnimStartGame());
        globalVolume.DOVignetteIntensity(1, 2f);
        Flaque.transform.DOScale(new Vector3(0.5441945f, 1.451186f, 0.5441945f), 0.3f);
    }

    public void VibrateScreen()
    {
        CinemachineShake.instance.ShakeCamera(3,3,0.2f); 
    }

    public void Option()
    {
        cg_MenuMainMenu.DOFade(0, 2);
        cg_btn_Play.DOFade(0, 2);
        cg_btn_Option.DOFade(0, 2);
        cg_btn_Quit.DOFade(0, 2);
        
        go_btn_Play.GetComponent<Button>().interactable = false;
        go_btn_Option.GetComponent<Button>().interactable = false;
        go_btn_Quit.GetComponent<Button>().interactable = false;
        
        
        cg_btn_Score.GetComponent<Button>().interactable = true;
        cg_btn_Credit.GetComponent<Button>().interactable = true;
        cg_btn_Advanced.GetComponent<Button>().interactable = true;
        cg_btn_MainMenu.GetComponent<Button>().interactable = true;
        
        cg_OptionMenu.DOFade(1, 5);
        cg_btn_Score.DOFade(1, 5.5f);
        cg_btn_Credit.DOFade(1, 5.8f);
        cg_btn_Advanced.DOFade(1, 6f);
        cg_btn_MainMenu.DOFade(1, 6.2f);
        
        animator.enabled = true;
        animator.SetBool("IsMain", false);
    }

    public void Main()
    {

        cg_OptionMenu.DOFade(0, 2);
        cg_btn_Score.DOFade(0, 2);
        cg_btn_Credit.DOFade(0, 2);
        cg_btn_Advanced.DOFade(0, 2);
        cg_btn_MainMenu.DOFade(0, 2);
        
        cg_btn_Score.GetComponent<Button>().interactable = false;
        cg_btn_Credit.GetComponent<Button>().interactable = false;
        cg_btn_Advanced.GetComponent<Button>().interactable = false;
        cg_btn_MainMenu.GetComponent<Button>().interactable = false;


        cg_MenuMainMenu.DOFade(1, 5);
        cg_btn_Play.DOFade(1, 6);
        cg_btn_Option.DOFade(1, 6.5f);
        cg_btn_Quit.DOFade(1, 7);
        
        go_btn_Play.GetComponent<Button>().interactable = true;
        go_btn_Option.GetComponent<Button>().interactable = true;
        go_btn_Quit.GetComponent<Button>().interactable = true;
        
        
        animator.SetBool("IsMain", true);
    }

    public void Credit()
    {
        //verif = true;
        
        cg_OptionMenu.DOFade(0, 2);
        cg_btn_Score.DOFade(0, 2);
        cg_btn_Credit.DOFade(0, 2);
        cg_btn_Advanced.DOFade(0, 2);
        cg_btn_MainMenu.DOFade(0, 2).OnComplete(() =>
            go_btn_MainMenu.SetActive(false));
        
        cg_btn_Score.GetComponent<Button>().interactable = false;
        cg_btn_Credit.GetComponent<Button>().interactable = false;
        cg_btn_Advanced.GetComponent<Button>().interactable = false;
        cg_btn_MainMenu.GetComponent<Button>().interactable = false;

        go_btn_BackToOption.SetActive(true);
        cg_CreditMenu.DOFade(1, 2);
        cg_Credit.DOFade(1, 2);
        cg_btn_BackToOption.DOFade(1, 2);
        
        cg_btn_BackToOption.GetComponent<Button>().interactable = true;
        
    }

    public void BackToOption()
    {
        //verif = false;
        
        go_btn_MainMenu.SetActive(true);
        cg_CreditMenu.DOFade(0, 2);
        cg_Credit.DOFade(0, 2);
        cg_btn_BackToOption.DOFade(0, 2).OnComplete(() =>
            go_btn_BackToOption.SetActive(false));
        
        cg_btn_BackToOption.GetComponent<Button>().interactable = false;
        
        cg_OptionMenu.DOFade(1, 2);
        cg_btn_Score.DOFade(1, 2);
        cg_btn_Credit.DOFade(1, 2);
        cg_btn_Advanced.DOFade(1, 2);
        cg_btn_MainMenu.DOFade(1, 2);
        
        cg_btn_Score.GetComponent<Button>().interactable = true;
        cg_btn_Credit.GetComponent<Button>().interactable = true;
        cg_btn_Advanced.GetComponent<Button>().interactable = true;
        cg_btn_MainMenu.GetComponent<Button>().interactable = true;
    }


    IEnumerator PlayAnimStartGame()
    {
        go_btn_Play.GetComponent<Button>().interactable = false;
        go_btn_Option.GetComponent<Button>().interactable = false;
        go_btn_Quit.GetComponent<Button>().interactable = false;
        
        cg_btn_Option.DOFade(0, 2.5F);
        cg_btn_Quit.DOFade(0, 2);
        
        yield return new WaitForSeconds(1.5f);

        cg_btn_Play.DOFade(0, 2);
        cg_MenuMainMenu.DOFade(0, 1.5f);
        //globalVolume
        verif = true;

        animator.enabled = true;
        animator.SetTrigger("Play");
        
        //StartCoroutine(PlayDutch());
        //cam.DOCinemachineOrthoSize(0, 3).SetEase(Ease.InQuint);

        yield return new WaitForSeconds(4);
        
        
        
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    IEnumerator PlayDutch()
    {
        yield return new WaitForSeconds(0.1f);
        verifDutch = true; // Pour le dutch modfication voir dans l'update
    }

    IEnumerator PlayAnimQuitGame()
    {
        yield return new WaitForSeconds(2f);
        
        go_MainMenu.SetActive(false);
        go_btn_Play.SetActive(false);
        go_btn_Option.SetActive(false);
        go_btn_Quit.SetActive(false);

        cam.transform.DOMoveY(-50, 3).SetEase(Ease.InQuart);
        
        
        yield return new WaitForSeconds(3f);
        
        Application.Quit();
    }
}
