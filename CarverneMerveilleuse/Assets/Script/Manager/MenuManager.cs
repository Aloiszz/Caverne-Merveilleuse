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

public class MenuManager : MonoBehaviour
{
    public GameObject sceneProps;
    
    [Header("Panel")]
    public CanvasGroup cg_MenuMainMenu;
    public CanvasGroup cg_btn_Play;
    public CanvasGroup cg_btn_Option;
    public CanvasGroup cg_btn_Quit;
    
    [Header("GameObject")]
    public GameObject go_MainMenu;
    public GameObject go_btn_Play;
    public GameObject go_btn_Option;
    public GameObject go_btn_Quit;

    public CinemachineVirtualCamera cam;
    public bool verifDutch;
    
    
    public Volume globalVolume;

    public bool verif;
    
    
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

        cam.m_Lens.OrthographicSize = 10;
        cam.m_Lens.Dutch = 0;
        verifDutch = false;
    }

    private void Update()
    {
        if (verifDutch)
        {
            cam.m_Lens.Dutch += 20 * Time.deltaTime;
            
        }
    }

    IEnumerator MainMenu() // ouverture du menu
    {
        //cam.m_Lens.Dutch = 5;
        
        yield return new WaitForSeconds(2);
        go_MainMenu.SetActive(true);
        cg_MenuMainMenu.DOFade(1, 2);
            
        yield return new WaitForSeconds(3f);
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
        Application.Quit();
    }

    public void Play()
    {
        StartCoroutine(PlayAnim());
        cam.DOCinemachineOrthoSize(0, 2).SetEase(Ease.InQuint);
        globalVolume.DOVignetteIntensity(1, 2f);
        verifDutch = true;
    }

    public void Option()
    {
        CinemachineShake.instance.ShakeCamera(3,3,0.2f);
    }


    IEnumerator PlayAnim()
    {
        cg_btn_Option.DOFade(0, 2.5F);
        cg_btn_Quit.DOFade(0, 2);
        
        yield return new WaitForSeconds(1.5f);
        
        cg_btn_Play.DOFade(0, 2);
        cg_MenuMainMenu.DOFade(0, 1.5f);
        //globalVolume
        verif = true;

        yield return new WaitForSeconds(2.5f);
        
        
        
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}
