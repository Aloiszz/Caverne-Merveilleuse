using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject childSlot;
    [SerializeField] private GameObject[] spawnPointPosition;
    private int rand;
    private void Start()
    {
        SpawnEnnemy();
    }

    private void Update()
    {
        if (spawnPointPosition.Length < 0)
        {
            //Door fermé
        }
        else
        {
            //Door ouverte
        }
    }

    //----------------------- Spawn Ennemy Region-----------------
    public void SpawnEnnemy()
    {
        spawnPointPosition = GameObject.FindGameObjectsWithTag("SpawnEnnemy");
        spawnPointPosition.ToList();

        for (int i = 0; i < 2; i++)
        {
            rand = Random.Range(0, spawnPointPosition.Length);
            Instantiate(EnnemyManager.instance.spider, spawnPointPosition[rand].transform.position, quaternion.identity, childSlot.transform);
        }
        
    }
}
