using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CinemachineCameraZoom : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    public static CinemachineCameraZoom instance;

    private float initialZoom = 10f;
    static float t = 0;
    private float timeToComeBack;
    private bool verif;
    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        
        if (instance != null && instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            instance = this; 
        } 
    }

    private void Start()
    {
        cinemachineVirtualCamera.m_Lens.OrthographicSize = initialZoom;
    }

    public void CameraZoom(float targetZoom, float timeToArrive, float timeToComeBack)
    {
        StopAllCoroutines();
        StartCoroutine(Lerp(cinemachineVirtualCamera.m_Lens.OrthographicSize, targetZoom, timeToArrive));
        
        this.timeToComeBack = timeToComeBack;
    }

    public void StopZoom(float timeToComeBack)
    {
        StopAllCoroutines();
        StartCoroutine(Lerp(cinemachineVirtualCamera.m_Lens.OrthographicSize, initialZoom, timeToComeBack));
    }
    
    IEnumerator Lerp(float start, float end, float time) 
    {
        t = 0f;
        while(cinemachineVirtualCamera.m_Lens.OrthographicSize != end) 
        {
            cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(start, end, t);
            t += Time.deltaTime / time;
            yield return null;
        }
        yield return null;
        StopAllCoroutines();
        StartCoroutine(Lerp(cinemachineVirtualCamera.m_Lens.OrthographicSize, initialZoom, timeToComeBack));
    }
}
