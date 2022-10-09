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
    
    [Header("Panel")] 
    public GameObject playModePanel_;
    public GameObject LevelPanel_;

    

    public void PlayMode()
    {
        playModePanel_.SetActive(true);
        LevelPanel_.SetActive(false);
        
        playModeCG_.DOFade(1, 0.5f);
        levelCG_.DOFade(0, 0.5f);

        StartCoroutine(UnPauseTime());
    }
    
    public void level()
    {
        playModePanel_.SetActive(false);
        LevelPanel_.SetActive(true);

        playModeCG_.DOFade(0, 0.5f);
        levelCG_.DOFade(1, 0.5f);

        StartCoroutine(PauseTime());
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


    public void GenProLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
    
    public void GALevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GA");
    }
    
    public void BossFight()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Scene test");
    }
}
