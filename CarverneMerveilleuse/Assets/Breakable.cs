using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Breakable : MonoBehaviour
{
    public GameObject dent;
    public ParticleSystem eclat;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 8)
        {
            int random = Random.Range(0, AudioManager.instance.potsClips.Count);
            Instantiate(eclat, transform.position, Quaternion.identity,
                RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].transform);
            int drop = Random.Range(0, 6);
            if (drop > 2 && drop <= 4)
            {
                
                gameObject.transform.DOMove(new Vector3(Random.Range(-3, 4), Random.Range(-3, 4)), 0.1f);
                Instantiate(dent, gameObject.transform.position, Quaternion.identity, RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].transform);
            }
            else if (drop == 5)
            {
                for (int i = 0; i < 2; i++)
                {
                    gameObject.transform.DOMove(new Vector3(Random.Range(-3, 4), Random.Range(-3, 4)), 0.1f);
                    Instantiate(dent, gameObject.transform.position, Quaternion.identity, RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].transform);
                }
            }
            AudioManager.instance.SFXSource.PlayOneShot(AudioManager.instance.potsClips[random]);
            Destroy(gameObject);
        }
    }
    
}
