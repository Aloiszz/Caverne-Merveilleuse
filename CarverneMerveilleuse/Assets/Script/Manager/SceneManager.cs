using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class SceneManager : MonoBehaviour
{
    [Header("Canvas Group")] 
    public CanvasGroup playModeCG_;
    public CanvasGroup levelCG_;
    public CanvasGroup deathCG_;
    public CanvasGroup pauseCG_;
    
    [Header("Panel")] 
    public GameObject playModePanel_;
    public GameObject LevelPanel_;
    public GameObject death_;
    public GameObject pauseMenu_;


    [Header("Animator")] 
    public Animator animator;

    [Header("option Panel")] 
    public CanvasGroup img;
    public CanvasGroup optionPanel;
    
    [Header("Credit Panel")]
    public CanvasGroup creditPanel;
    public CanvasGroup cg_CreditTitle;
    
    
    public static SceneManager instance;
    
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
        //death_.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 1)
        {
            Pause();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Unpause();
        }
    }

    public void PlayMode()
    {
        playModePanel_.SetActive(true);
        LevelPanel_.SetActive(false);
        
        playModeCG_.DOFade(1, 0.5f);
        levelCG_.DOFade(0, 0.5f);

        //StartCoroutine(UnPauseTime());
    }
    
    public void level()
    {
        playModePanel_.SetActive(false);
        LevelPanel_.SetActive(true);

        playModeCG_.DOFade(0, 0.5f);
        levelCG_.DOFade(1, 0.5f);

        //StartCoroutine(PauseTime());
    }

    public void Death()
    {
        playModePanel_.SetActive(false);
        death_.SetActive(true);
        
        playModeCG_.DOFade(0, 0.5f);
        deathCG_.DOFade(1, 0.5f);
    }

    public void GenProLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        deathCG_.DOFade(0, 0);
        death_.SetActive(false);
    }
    
    public void GALevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GA");
    }
    
    public void BossFight()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Scene test");
    }
    public void Pause()
    {
        img.DOFade(0, 0);
        optionPanel.DOFade(0, 0.5f);
        
        StopCoroutine(UnPauseTime());
        playModeCG_.DOFade(0, 0.5f);
        pauseCG_.DOFade(1, 0.5f);
        pauseMenu_.SetActive(true);
        //StartCoroutine(PauseTime());
    }
    public void Unpause()
    {
        StopCoroutine(PauseTime());
        playModeCG_.DOFade(1, 0.5f);
        pauseCG_.DOFade(0, 0.5f);
        pauseMenu_.SetActive(false);
        //StartCoroutine(UnPauseTime());
    }
    
    IEnumerator PauseTime()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0f;
    }
    IEnumerator UnPauseTime()
    {
        Time.timeScale = 1;
        yield return new WaitForSeconds(0.5f);
    }
    public void Quit()
    {
        Debug.Log("quit");
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    
    public void Option()
    {
        creditPanel.DOFade(0, 1);
        img.DOFade(1, 0);
        pauseCG_.DOFade(0, 2);
        optionPanel.DOFade(1, 2);
        
        animator.enabled = true;
        animator.SetBool("IsMain", false);
    }
    
    
    public void Credit()
    {
        optionPanel.DOFade(0, 2);
        creditPanel.DOFade(1, 2);
        //verif = true;
        /*BackToOptionFromCredit = true;
        
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
        cg_CreditTitle.DOFade(1, 2);*/
        
        animator.SetTrigger("Credit");
        StartCoroutine(CreditTitle());
    }

    IEnumerator CreditTitle()
    {
        yield return new WaitForSeconds(3);
        cg_CreditTitle.DOFade(0, 2);
    }
}
