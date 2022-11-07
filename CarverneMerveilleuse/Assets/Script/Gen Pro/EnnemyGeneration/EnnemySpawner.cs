using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnnemySpawner : MonoBehaviour
{
    [Header("ScriptableObject")] 
    public SO_EnnemyBigRoom SO_ennemySpawner;
    
    
    [SerializeField] private GameObject childSlot;
    [SerializeField] private GameObject[] spawnPointPosition;
    public GameObject[] ennemyAlive;

    [Tooltip("1 chiffre correspond au spyider, 2eme au Bat, 3eme Petrol")]
    public List<int> nbrEnnemySpawned; 
    private int rand;
    private void Start()
    {
        SpawnEnnemy();
        LookForEnnemyAlive();
    }

    void LookForEnnemyAlive()
    {
        ennemyAlive = GameObject.FindGameObjectsWithTag("CAC");
        /*ennemyAlive = GameObject.FindGameObjectsWithTag("Dist");
        ennemyAlive = GameObject.FindGameObjectsWithTag("Gros");*/
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

        

        for (int i = 0; i < SO_ennemySpawner.spawn_Spyder[SO_ennemySpawner.difficulty_Index]; i++)
        {
            var apparitionChance = Random.Range(0, 100);
            if (apparitionChance < SO_ennemySpawner.difficulty_Spyder[SO_ennemySpawner.difficulty_Index])
            {
                rand = Random.Range(0, spawnPointPosition.Length);
                Instantiate(EnnemyManager.instance.spider, spawnPointPosition[rand].transform.position, quaternion.identity, transform);
            }
        }
        
        for (int j = 0; j < SO_ennemySpawner.spawn_Bat[SO_ennemySpawner.difficulty_Index]; j++)
        {
            var apparitionChance = Random.Range(0, 100);
            if (apparitionChance < SO_ennemySpawner.difficulty_Bat[SO_ennemySpawner.difficulty_Index])
            {
                rand = Random.Range(0, spawnPointPosition.Length);
                Instantiate(EnnemyManager.instance.bat, spawnPointPosition[rand].transform.position, quaternion.identity, transform); 
            }
        }
        
        for (int k = 0; k < SO_ennemySpawner.spawn_Petrol[SO_ennemySpawner.difficulty_Index]; k++)
        {
            var apparitionChance = Random.Range(0, 100);
            if (apparitionChance < SO_ennemySpawner.difficulty_Petrol[SO_ennemySpawner.difficulty_Index])
            {
                rand = Random.Range(0, spawnPointPosition.Length);
                Instantiate(EnnemyManager.instance.petrol, spawnPointPosition[rand].transform.position, quaternion.identity, transform);
            }
                
        }
    }
}
