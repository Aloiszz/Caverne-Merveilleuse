using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerifIntro : MonoBehaviour
{

    public int compte = 0;

    public static VerifIntro instance;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        
        if (instance != null && instance != this) 
        {
            Destroy(gameObject);
        } 
        else 
        { 
            instance = this; 
        }
        Introduction.instance.playIntro = true;
    }

    private bool isis;
    private void Update()
    {
        if (compte == 0)
        {
            Introduction.instance.playIntro = true;
            Introduction.instance.Start1();
            compte++;
        }
        if (compte > 1)
        {
            Introduction.instance.playIntro = false;
            Introduction.instance.Start2();
        }

        if (!isis)
        {
            isis = true;
            
        }
    }

    public void STOP()
    {
        Introduction.instance.playIntro = false;
    }
    
    
}
