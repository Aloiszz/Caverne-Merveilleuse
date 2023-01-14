using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{


    public static AudioManager instance;

    [SerializeField] private AudioMixer mixer;
    
    public const string MAIN_KEY = "mainVolume";
    public const string MUSIC_KEY = "musicVolume";
    public const string SFX_KEY = "sfxVolume";
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        LoadVolume();
    }

    void LoadVolume()
    {
        float mainVolume = PlayerPrefs.GetFloat(MAIN_KEY,1f);
        mixer.SetFloat(VolumeSettings.MIXER_MAIN, Mathf.Log10(mainVolume) * 20);
        
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY,1f);
        mixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
        
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY,1f);
        mixer.SetFloat(VolumeSettings.MIXER_SFX, Mathf.Log10(sfxVolume) * 20);
    }
}
