using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer;

    public static CinemachineShake instance;
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

    public void ShakeCamera(float intensityAmplitude, float intensityFrequency, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = 
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensityAmplitude;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = intensityFrequency;
        shakeTimer = time;
    }

    private void Update()
    {
        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = 
                    cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
                cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 0f;
            }
        }
        
    }
}
