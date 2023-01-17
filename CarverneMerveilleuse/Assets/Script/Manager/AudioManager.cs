using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [Header("Music")] 
    [SerializeField] private AudioSource Source;
    [SerializeField] private AudioClip mainMenuClip;
    [SerializeField] private AudioClip combatClip;
    [SerializeField] private AudioClip noCombatClip;
    [SerializeField] private AudioClip preBossClip;
    [SerializeField] private AudioClip bossPhase2Clip;

    [SerializeField] private float volumeIntensity = 0.8f;

    [Space]
    [SerializeField] private List<AudioClip> ClocheClip;
    private int rand;
    [SerializeField] private AudioClip quitClip;
    
    public static AudioManager instance;
    
    [Space]
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


    public void StopMusic()
    {
        Source.Stop();
    }

    public void PlayIntroMusic()
    {
        StopMusic();
        Source.PlayOneShot(mainMenuClip,volumeIntensity);
    }
    
    public void PlayCombatMusic()
    {
        StopMusic();
        Source.PlayOneShot(combatClip,volumeIntensity);
    }
    
    public void PlayNoCombatMusic()
    {
        StopMusic();
        Source.PlayOneShot(noCombatClip,volumeIntensity);
    }
    
    public void PlayPreBossMusic()
    {
        StopMusic();
        Source.PlayOneShot(preBossClip,volumeIntensity);
    }
    public void PlayBossPhase2()
    {
        StopMusic();
        Source.PlayOneShot(bossPhase2Clip,volumeIntensity);
    }

    public void PlayCloche()
    {
        rand = Random.Range(0, ClocheClip.Count);
        Source.PlayOneShot(ClocheClip[rand]);
    }


    public void Quit()
    {
        StopMusic();
        Source.PlayOneShot(quitClip);
    }
}
