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

    [SerializeField] private int difficulty_Index;
    
    
    [SerializeField] private GameObject childSlot;
    [SerializeField] private GameObject[] spawnPointPosition;
    public List<GameObject> ennemyAlive;

    private bool isVerif; // verifie que premier Spawn a eu lieu
    private int rand;

    public int actualNumberOfWave;
    public int numberOfWave;

    private int randPosSpyder; //position de spawn de l'ennemi
    private int randPosBat;
    private int randPosPetrol;
    
    private void Start()
    {
        spawnPointPosition = GameObject.FindGameObjectsWithTag("SpawnEnnemy");
        spawnPointPosition.ToList();
        
        
        RoomManager.instance.canThePotitChatSpawn = false;
        Secure_So();
        Invoke("SpawnEnnemy", EnnemyManager.instance.timeBeforeFighting);
        LookForEnnemyAlive();
        StartCoroutine(Wait());
        
        actualNumberOfWave = 0;
    }


    IEnumerator Wait()
    {
        yield return new WaitForSeconds(EnnemyManager.instance.timeBeforeFighting+1);
        isVerif = true;
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
        numberOfWave = SO_ennemySpawner.numberOfWave;

        if (RoomManager.instance.roomMemoryIndex <= 5)
        {
            difficulty_Index = SO_ennemySpawner.difficulty_Index;
        }
        if (RoomManager.instance.roomMemoryIndex > 5 &&  RoomManager.instance.roomMemoryIndex < 7)
        {
            difficulty_Index = SO_ennemySpawner.difficulty_Index;
            difficulty_Index++;
        }
        if (RoomManager.instance.roomMemoryIndex >= 7)
        {
            difficulty_Index = SO_ennemySpawner.difficulty_Index;
            difficulty_Index += 2;
        }
    }
    
    private void Update()
    {
        
        
        if(ennemyAlive.Count <= 0)
        {
            if (numberOfWave > actualNumberOfWave)
            {
                
                if (isVerif)
                {   
                    SpawnEnnemy();
                    isVerif = false;
                    StartCoroutine(Wait());
                }
                LookForEnnemyAlive();
                GetComponent<Room>().CloseTheDoor(); //Door fermé
                RoomManager.instance.canThePotitChatSpawn = false;
            }
            else
            {
                GetComponent<Room>().OpenTheDoor(); // Door Ouverte
                RoomManager.instance.canThePotitChatSpawn = true;
            }
        }

        if (PlayerController.instance.life <= 0)
        {
            foreach (var i in ennemyAlive)
            {
                Destroy(i);
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
        actualNumberOfWave++;
        for (int i = 0; i < SO_ennemySpawner.spawn_Spyder[difficulty_Index]; i++)
        {
            var apparitionChance = Random.Range(0, 100);
            if (apparitionChance < SO_ennemySpawner.difficulty_Spyder[difficulty_Index])
            {
                //rand = Random.Range(0, spawnPointPosition.Length);
                ChooseRandomPosition(1);
                Instantiate(EnnemyManager.instance.SpawningVFX, spawnPointPosition[randPosSpyder].transform.position, quaternion.identity, transform);
                Instantiate(EnnemyManager.instance.spider, spawnPointPosition[randPosSpyder].transform.position, quaternion.identity, transform);
                AudioManager.instance.PlaySpawn();
            }
        }
        
        for (int j = 0; j < SO_ennemySpawner.spawn_Bat[difficulty_Index]; j++)
        {
            var apparitionChance = Random.Range(0, 100);
            if (apparitionChance < SO_ennemySpawner.difficulty_Bat[difficulty_Index])
            {
                //rand = Random.Range(0, spawnPointPosition.Length);
                ChooseRandomPosition(2);
                Instantiate(EnnemyManager.instance.SpawningVFX, spawnPointPosition[randPosBat].transform.position, quaternion.identity, transform);
                Instantiate(EnnemyManager.instance.bat, spawnPointPosition[randPosBat].transform.position, quaternion.identity, transform); 
                AudioManager.instance.PlaySpawn();
            }
        }
        
        for (int k = 0; k < SO_ennemySpawner.spawn_Petrol[difficulty_Index]; k++)
        {
            var apparitionChance = Random.Range(0, 100);
            if (apparitionChance < SO_ennemySpawner.difficulty_Petrol[difficulty_Index])
            {
                //rand = Random.Range(0, spawnPointPosition.Length);
                ChooseRandomPosition(3);
                Instantiate(EnnemyManager.instance.SpawningVFX, spawnPointPosition[randPosPetrol].transform.position, quaternion.identity, transform);
                Instantiate(EnnemyManager.instance.petrol, spawnPointPosition[randPosPetrol].transform.position, quaternion.identity, transform);
                AudioManager.instance.PlaySpawn();
            }
        }
    }
    

    void ChooseRandomPosition(int TypeEnnemi)
    {
        switch (TypeEnnemi)
        {
            case 1: // Spyder
                randPosSpyder = Random.Range(0, spawnPointPosition.Length);
                break;
            
            case 2: // Bat
                randPosBat = Random.Range(0, spawnPointPosition.Length);
                break;
            
            case 3: // Petrol
                randPosPetrol = Random.Range(0, spawnPointPosition.Length);
                break;
        }
    }
}
