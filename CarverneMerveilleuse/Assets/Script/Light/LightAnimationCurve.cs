using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightAnimationCurve : MonoBehaviour
{
    public float time;
    
    [Header("Animation Curve")]
    public AnimationCurve CourbeDeFlamme;
    
    public AnimationCurve CreditFlamme;
    public Light2D Flamme;
    private float graph, increment;
    public bool canRunGame;
    public bool verif;
    private bool ischecked;


    private void Start()
    {
        StartCoroutine(TimeToLight());
        Flamme = GetComponent<Light2D>();
        Flamme.intensity = 0;
    }

    void Update()
    {
        if (canRunGame)
        {
            Flamme.intensity = 1;
            increment += Time.deltaTime;
            graph = CourbeDeFlamme.Evaluate(increment);
            Flamme.intensity = graph;
        }

        if (MenuManager.instance.verif)
        {
            canRunGame = false;
            ischecked = true;
            //Flamme.intensity = 0;
            increment -= Time.deltaTime;
            graph = CourbeDeFlamme.Evaluate(increment);
            Flamme.intensity = graph;
        }
        else if (ischecked)
        {
            canRunGame = true;
        }
    }

    IEnumerator TimeToLight()
    {
        yield return new WaitForSeconds(time);
        canRunGame = true;
    }
}
