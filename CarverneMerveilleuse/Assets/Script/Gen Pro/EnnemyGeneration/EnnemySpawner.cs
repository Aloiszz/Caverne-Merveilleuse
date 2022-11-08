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
    public List<GameObject> ennemyAlive;

    [Tooltip("1 chiffre correspond au spyider, 2eme au Bat, 3eme Petrol")]
    public List<int> nbrEnnemySpawned; 
    private int rand;

    public int actualNumberOfWave;
    public int numberOfWave;
    private void Start()
    {
        spawnPointPosition = GameObject.FindGameObjectsWithTag("SpawnEnnemy");
        spawnPointPosition.ToList();
        
        Secure_So();
        SpawnEnnemy();
        LookForEnnemyAlive();
    }

    void LookForEnnemyAlive()
    {
        addEnnemyToList("CAC");
        addEnnemyToList("Dist");
        addEnnemyToList("Gros");
    }
    void addEnnemyToList(string tag)
    {
        foreach (var go in GameObject.FindGameObjectsWithTag(tag))
        {
            ennemyAlive.Add(go);
        }
    }


    void Secure_So()
    {
        actualNumberOfWave = SO_ennemySpawner.numberOfWave;
        numberOfWave = SO_ennemySpawner.numberOfWave;
    }
    private void Update()
    {
        
        if(ennemyAlive.Count <= 0)
        {
            Debug.Log("qsds");
            if (numberOfWave < actualNumberOfWave)
            {
                SpawnEnnemy();
                actualNumberOfWave--;
                
                //Door fermé (bool liée au script Room)
            }
            else
            {
                //Door ouverte
            }
        }
    }

    private void LateUpdate()
    {
        for(var i = ennemyAlive.Count - 1; i > -1; i--)
        {
            if (ennemyAlive[i] == null)
                ennemyAlive.RemoveAt(i);
        }
    }

    //----------------------- Spawn Ennemy Region-----------------
    public void SpawnEnnemy()
    {
        
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
