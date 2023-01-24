using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public class Breakable : MonoBehaviour
{
    public GameObject dent;
    public ParticleSystem eclat;
    public bool cantDrop;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 8)
        {
            int random = Random.Range(0, AudioManager.instance.potsClips.Count);
            Instantiate(eclat, transform.position, Quaternion.identity,
                transform.parent);
            int drop = Random.Range(0, 6);
            if (!cantDrop)
            {
                if (drop == 0)
                {
                
                    gameObject.transform.DOMove(new Vector3(Random.Range(-3, 4), Random.Range(-3, 4)), 0.1f);
                    Instantiate(dent, gameObject.transform.position, Quaternion.identity, transform.parent);
                }
                
            }
            AudioManager.instance.SFXSource.PlayOneShot(AudioManager.instance.potsClips[random]);
            Destroy(gameObject);
        }
    }
    
}
