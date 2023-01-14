using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LifeManager : MonoBehaviour
{
    public SO_PlayerController PlayerControllerSO;
    
    [Header("Life")]
    [SerializeField] private Image life_Bar;
    [SerializeField] private TextMeshProUGUI lifeTxt;
    private bool verif;
    [SerializeField] private int maxLife;
    
    [Space]
    [Header("Rage")]
    [SerializeField] private Image life_Bar_RageScore; // dépend du scoreRage
    [SerializeField] private Image life_Bar_RageLife; // Dépend de la vie
    public float timeInRage = 3;
    [HideInInspector] public float timeInRageMax;
    public bool isInRage;
    [SerializeField]private bool rageBarScore; // savoir juste quelle bar utilisé entre bar life et bar score
    [SerializeField]private bool rageBarLife;
    [SerializeField] private Volume globalVolume;
    [SerializeField] private Renderer2DData blit; // eye blood
    [SerializeField] private GameObject _rageArea;
    
    [SerializeField] private Image r_key_img;
    [SerializeField] private TextMeshProUGUI rageTxt;
    [SerializeField] private int listScoreRageIndex = 0;
    

    [Space] [Header("VFX")] 
    public GameObject RageWave;
    
    
    public static LifeManager instance;
    
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
        r_key_img.DOFade(0, 0);
        rageTxt.DOFade(0, 0);
        
        StartCoroutine(AfficheHealthBar());
        
        maxLife = PlayerControllerSO.life;
        timeInRageMax = timeInRage;
    }

    private void Update()
    {
        
        if (verif)
        {
            life_Bar.DOFillAmount((float)PlayerController.instance.life / (float)PlayerController.instance.lifeDepard, 0.3f);
        }
        
        life_Bar_RageScore.DOFillAmount((float)Score.instance.scoreRage / (float)Score.instance.listScoreRage[listScoreRageIndex], 0.3f);

        RageDueToLife();
        RageDueToScoreRage();
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            isInRage = true;
            PlayerController.instance.Rage();
            CameraZoom();
            RageShockWave();
        }
        
        if (isInRage)
        {
            blit.rendererFeatures[0].SetActive(true);

            timeInRage -= 1 * Time.deltaTime;
            if (rageBarScore)
            {
                life_Bar_RageScore.DOFillAmount(timeInRage / timeInRageMax, 0);
                StartCoroutine(WaitForRageScoreBar());
            }
            if(rageBarLife)
            {
                life_Bar_RageLife.DOFillAmount(timeInRage / timeInRageMax, 0);
            }

            if (globalVolume.weight <= 1)
            {
                globalVolume.weight += 4 * Time.deltaTime;
            }
            if (timeInRage <= 0)
            {
                timeInRage = timeInRageMax;
                isInRage = false;
                PlayerController.instance.Rage();
                Score.instance.scoreRage = 0;
                r_key_img.DOFade(0, 0);
            }
        }
        else
        {
            blit.rendererFeatures[0].SetActive(false);
            if (globalVolume.weight >= 0)
            {
                globalVolume.weight -= 2 * Time.deltaTime;
            }
        }
    }



    void RageDueToLife()
    {
        if (PlayerController.instance.life > PlayerController.instance.lifeDepard) // rage quand surplus de vie
        {
            life_Bar_RageLife.DOFillAmount((float)1, 1);
            r_key_img.DOFade(1, .2f);
            rageTxt.DOFade(1, 0.2f);
            
            //CinemachineShake.instance.ShakeCamera(2,2,10f);
            //r_key_img.transform.DOShakePosition(0.1f, 5);
            //rageTxt.transform.DOShakePosition(0.1f, 5);
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                CinemachineShake.instance.StopAllCoroutines();
                r_key_img.DOFade(0, .2f);
                rageTxt.DOFade(0, 0.2f);
                isInRage = true;
                rageBarLife = true;
                PlayerController.instance.Rage();
                CameraZoom();
                RageShockWave();
                if (PlayerController.instance.life >= PlayerController.instance.lifeDepard)
                {
                    PlayerController.instance.life = PlayerController.instance.lifeDepard;
                }
                else
                {
                    PlayerController.instance.life++;
                }
            }
        }
        else
        {
            r_key_img.DOFade(0, .2f);
            rageTxt.DOFade(0, .2f);
        }

    }

    void RageDueToScoreRage()
    {
        if (Score.instance.scoreRage > Score.instance.listScoreRage[listScoreRageIndex]) // rage quand score de rage atteint
        {
            r_key_img.DOFade(1, .2f);
            rageTxt.DOFade(1, 0.2f);
            
            //CinemachineShake.instance.ShakeCamera(2,2,10f);
            //r_key_img.transform.DOShakePosition(0.1f, 5);
            //rageTxt.transform.DOShakePosition(0.1f, 5);
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                listScoreRageIndex++;
                r_key_img.DOFade(0, .2f);
                rageTxt.DOFade(0, 0.2f);
                isInRage = true;
                rageBarScore = true;
                PlayerController.instance.Rage();
                CameraZoom();
                RageShockWave();
                if (PlayerController.instance.life >= PlayerController.instance.lifeDepard)
                {
                    PlayerController.instance.life = PlayerController.instance.lifeDepard;
                }
                else
                {
                    PlayerController.instance.life++;
                }
            }
        }
        else
        {
            r_key_img.DOFade(0, .2f);
        }
    }

    void RageShockWave()
    {
        _rageArea.SetActive(true);
        StartCoroutine(RageAreaTime());
    }

    IEnumerator RageAreaTime()
    {
        yield return new WaitForSeconds(0.1f);
        _rageArea.SetActive(false);
    }

    void CameraZoom()
    {
        CinemachineCameraZoom.instance.CameraZoom(5f, 0.05f, 1.5f);
        //CinemachineCameraZoom.instance.cinemachineVirtualCamera.DOCinemachineOrthoSize(5, 0.05f).OnComplete((() => cameraEnd()));
        CinemachineShake.instance.ShakeCamera(4,4,1f);
        Instantiate(RageWave, PlayerController.instance.transform.position, Quaternion.identity,
            RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].transform);
        
        StartCoroutine(RageZoom());
    }

    IEnumerator RageZoom()
    {
        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < 6; i++)
        {
            CinemachineCameraZoom.instance.CameraZoom(9.5f, 0.2f, .2f);
            yield return new WaitForSeconds(.4f);
        }
        
    }

    void cameraEnd()
    {
        CinemachineCameraZoom.instance.cinemachineVirtualCamera.DOCinemachineOrthoSize(10, .5f);
    }

    IEnumerator WaitForRageScoreBar()
    {
        yield return new WaitForSeconds(3);
        rageBarScore = false;
        rageBarLife = false;
    }


    IEnumerator AfficheHealthBar()
    {
        life_Bar.DOFillAmount((float)1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        verif = true;
    }
}