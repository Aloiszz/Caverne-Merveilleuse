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
    
    [Tooltip("1 chiffre correspond au spyider, 2eme au Bat, 3eme Petrol")]
    public List<int> nbrEnnemySpawned; 
    private int rand;
    private void Start()
    {
        SpawnEnnemy();
    }

    private void Update()
    {
        if (spawnPointPosition.Length < 0)
        {
            //Door fermé (bool liée au script Room)
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

        for (int i = 0; i < nbrEnnemySpawned[0]; i++)
        {
            rand = Random.Range(0, spawnPointPosition.Length);
            Instantiate(EnnemyManager.instance.spider, spawnPointPosition[rand].transform.position, quaternion.identity, transform);
        }
        
        for (int j = 0; j < nbrEnnemySpawned[1]; j++)
        {
            rand = Random.Range(0, spawnPointPosition.Length);
            Instantiate(EnnemyManager.instance.bat, spawnPointPosition[rand].transform.position, quaternion.identity, transform);
        }
        
        for (int k = 0; k < nbrEnnemySpawned[2]; k++)
        {
            rand = Random.Range(0, spawnPointPosition.Length);
            Instantiate(EnnemyManager.instance.petrol, spawnPointPosition[rand].transform.position, quaternion.identity, transform);
        }
    }
}
