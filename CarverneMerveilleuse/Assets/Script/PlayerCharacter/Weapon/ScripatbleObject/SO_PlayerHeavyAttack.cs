using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHeavyAttack", menuName ="ScriptableObjects/new Player Heavy Attack", order = 1)]

public class SO_PlayerHeavyAttack : ScriptableObject
{
    [Header("Verification")] 
    public bool isStriking;
    public bool isCoolDown = false;
    
    [Header("Damage")]
    public List<float> heavyDamage;
    public int heavyDamageIndex;

    public bool pushEnnemy = false;
    
    [Header("Tour")]
    public List<float> loadingCoolDown; // temps entre chaque coups
    public int loadingCoolDownIndex;
    public List<float> coolDown; // temps entre chaque coups
    public int coolDownIndex;
    public float timerRemaining; // temps restant entre chaque coup pour arriver au combo
    public List<int> numberOfTurn; //nombre de tour que peut faire l'attaque
    public int numberOfTurnIndex;
    public List<float> timeTo360; //nombre de tour que peut faire l'attaque
    public int timeTo360Index;


    [Header("Cinemachine Schake")] 
    public float intensityLightCloseDamage;
    public float frequencyLightCloseDamage;
    public float timerLightCloseDamage;
}
