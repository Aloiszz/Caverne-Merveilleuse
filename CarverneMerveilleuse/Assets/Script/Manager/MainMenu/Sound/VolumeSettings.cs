using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    
    [SerializeField] private Slider MainSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private const string MIXER_MAIN = "MainVolume";
    private const string MIXER_MUSIC = "MusicVolume";
    private const string MIXER_SFX = "SfxVolume";

    private void Awake()
    {
        MainSlider.onValueChanged.AddListener(setMainVolume);
        musicSlider.onValueChanged.AddListener(setMusicVolume);
        sfxSlider.onValueChanged.AddListener(setSfxVolume);
    }

    void setMainVolume(float value)
    {
        mixer.SetFloat(MIXER_MAIN, Mathf.Log10(value) * 20);
    }
    
    void setMusicVolume(float value)
    {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
    }
    
    void setSfxVolume(float value)
    {
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
    }
    
}
