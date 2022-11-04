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
    public Light2D Flamme;
    private float graph, increment;
    public bool canRunGame;
    public bool verif;


    private void Start()
    {
        StartCoroutine(TimeToLight());
        Flamme = GetComponent<Light2D>();
        Flamme.intensity = 0;
    }

    void Update()
    {
        if (canRunGame && !MenuManager.instance.verif)
        {
            Flamme.intensity = 1;
            increment += Time.deltaTime;
            graph = CourbeDeFlamme.Evaluate(increment);
            Flamme.intensity = graph;
        }

        if (MenuManager.instance.verif)
        {
            canRunGame = false;
            increment -= Time.deltaTime;
            graph = CourbeDeFlamme.Evaluate(increment);
            Flamme.intensity = graph;
        }
    }

    IEnumerator TimeToLight()
    {
        yield return new WaitForSeconds(time);
        canRunGame = true;
    }
}
