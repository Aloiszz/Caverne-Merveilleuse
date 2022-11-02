using System.Collections;
using System.Collections.Generic;
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
    
    public GameObject panel_MainMenu;
    public CanvasGroup cg_MenuMainMenu;
    
    public CanvasGroup cg_btn_Play;
    public CanvasGroup cg_btn_Option;
    public CanvasGroup cg_btn_Quit;
    
    void Start()
    {
        //sceneProps = GetComponentsInChildren<SpriteRenderer>;
        StartCoroutine(MainMenu());
    }

    IEnumerator MainMenu()
    {
        yield return new WaitForSeconds(2);
        cg_MenuMainMenu.DOFade(1, 2);
            
        yield return new WaitForSeconds(3f);
        cg_btn_Play.DOFade(1, 3);
        
        yield return new WaitForSeconds(1.5f);
        cg_btn_Option.DOFade(1, 3);
        
        yield return new WaitForSeconds(1.5f);
        cg_btn_Quit.DOFade(1, 3);
    }

    
    void Update()
    {
        
    }


    public void Quit()
    {
        Application.Quit();
    }

    public void Play()
    {
        StartCoroutine(PlayAnim());
    }

    public void Option()
    {
        CinemachineShake.instance.ShakeCamera(10,10,2);
    }

    IEnumerator PlayAnim()
    {
        cg_btn_Option.DOFade(0, 2);
        cg_btn_Quit.DOFade(0, 2);
        
        yield return new WaitForSeconds(3);
        
        cg_btn_Play.DOFade(0, 2);
        
        yield return new WaitForSeconds(2);
        
        
        
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}
