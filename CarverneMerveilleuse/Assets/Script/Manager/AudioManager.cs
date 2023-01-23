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

    [Header("ambiance")] 
    [SerializeField] private AudioClip shopClip;
    [SerializeField] private AudioClip CaveClip;
    [SerializeField] private float volumeIntensity = 0.8f;

    [Space] [Header("Ennemis")] 
    public AudioSource SFXSource;
    [SerializeField] private AudioClip apparitionSound;
    [SerializeField] private AudioClip batAtkClip;
    [SerializeField] private AudioClip batDeathClip;
    [SerializeField] private AudioClip spiderAtkClip;
    [SerializeField] private AudioClip spiderDeathClip;
    [SerializeField] private AudioClip grosAtkClip;
    [SerializeField] private AudioClip grosDeathClip;
    [HideInInspector] public bool onePlay; //C'est pour qu'on destroy pas les oreilles des gens lors du spawn des ennemis
    
    [Header("Audio")] 
    public AudioClip FermetureDoorClip;
    public AudioClip OuvertureDoorClip;

    [Header("Shop et chat")] 
    public AudioClip AchatDitem;
    public AudioClip monterObject;
    public AudioClip descenteObject;
    public AudioClip monterObjectMerveilleux;
    public AudioClip descenteObjectMerveilleux;
    public AudioClip chatAnnonce;
    public AudioClip refutDachat;
    
    [Header("Pots casser")]
    public List<AudioClip> potsClips;
    
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
        Source.PlayOneShot(combatClip,volumeIntensity-.4f);
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
    
    public void PlayShop()
    {
        StopMusic();
        Source.PlayOneShot(shopClip,volumeIntensity - .3f);
    }
    public void PlayCave()
    {
        StopMusic();
        Source.PlayOneShot(CaveClip,volumeIntensity);
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

    public void PlaySpiderAtk(AudioSource spiderSource)
    {
        spiderSource.PlayOneShot(spiderAtkClip);
    }
    public void PlaySpiderDeath(AudioSource spiderSource)
    {
        spiderSource.PlayOneShot(spiderDeathClip);
    }
    public void PlayBatAtk(AudioSource batSource)
    {
        batSource.PlayOneShot(batAtkClip);
    }
    public void PlayBatDeath(AudioSource batSource)
    {
        batSource.PlayOneShot(batDeathClip);
    }
    public void PlayGrosAtk(AudioSource grosSource)
    {
        grosSource.PlayOneShot(grosAtkClip);
    }
    public void PlayGrosDeath(AudioSource grosSource)
    {
        grosSource.PlayOneShot(grosDeathClip);
    }

    public void PlayFermetureDoor()
    {
        SFXSource.PlayOneShot(AudioManager.instance.FermetureDoorClip);
    }
    public void PlayOuvertureDoor()
    {
        SFXSource.PlayOneShot(AudioManager.instance.OuvertureDoorClip);
    }
    
    public void PlaySpawn()
    {
        if (!onePlay)
        {
            //SFXSource.PlayOneShot(apparitionSound);
            Source.PlayOneShot(apparitionSound);
            onePlay = true;
        }
    }


    public void Achatitem()
    {
        SFXSource.PlayOneShot(AchatDitem);
    }

    public void AffichageItemOuverture(bool Merveilleux)
    {
        if (!Merveilleux)
        {
            SFXSource.PlayOneShot(monterObject);
        }
        else
        {
            SFXSource.PlayOneShot(monterObjectMerveilleux);
        }
    }
    public void AffichageItemFermeture(bool Merveilleux)
    {
        if (!Merveilleux)
        {
            SFXSource.PlayOneShot(descenteObject);
        }
        else
        {
            SFXSource.PlayOneShot(descenteObjectMerveilleux);
        }
    }

    public void CantBuy()
    {
        SFXSource.PlayOneShot(refutDachat);
    }
}
