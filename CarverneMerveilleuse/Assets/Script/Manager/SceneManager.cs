using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


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
    private void Pause()
    {
        StopCoroutine(UnPauseTime());
        playModeCG_.DOFade(0, 0.5f);
        pauseCG_.DOFade(1, 0.5f);
        pauseMenu_.SetActive(true);
        StartCoroutine(PauseTime());
    }
    public void Unpause()
    {
        StopCoroutine(PauseTime());
        playModeCG_.DOFade(1, 0.5f);
        pauseCG_.DOFade(0, 0.5f);
        pauseMenu_.SetActive(false);
        StartCoroutine(UnPauseTime());
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
        Application.Quit();
    }
}
