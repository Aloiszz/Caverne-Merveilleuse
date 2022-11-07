using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnnemyBigRoom", menuName ="ScriptableObjects/new Ennemy Big Room", order = 1)]

public class SO_EnnemyBigRoom : ScriptableObject
{
    public int numberOfWave = 3;

    public int difficulty_Index = 0;
    
    [Header("----------" +
            "Difficulty" +
            "----------")]
    [Tooltip("1er chiffre ==> Easy, 2eme ==> Mid, 3eme ==> Hard")]
    public List<float> difficulty_Spyder;
    
    [Tooltip("1er chiffre ==> Easy, 2eme ==> Mid, 3eme ==> Hard")]
    public List<float> difficulty_Bat;
    
    [Tooltip("1er chiffre ==> Easy, 2eme ==> Mid, 3eme ==> Hard")]
    public List<float> difficulty_Petrol;
    
    
    [Header("----------" +
            "Spawn" +
            "----------")]
    [Tooltip("1er chiffre ==> easy, 2eme ==> mid, 3eme ==> hard")]
    public List<float> spawn_Spyder;
    
    [Tooltip("1er chiffre ==> easy, 2eme ==> mid, 3eme ==> hard")]
    public List<float> spawn_Bat;
    
    [Tooltip("1er chiffre ==> easy, 2eme ==> mid, 3eme ==> hard")]
    public List<float> spawn_Petrol;
    
}
