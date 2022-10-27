using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerLightAttack", menuName ="ScriptableObjects/new Player Light Attack", order = 1)]

public class SO_PlayerLightAttack : ScriptableObject
{
    [Header("Verification")] 
    public bool isStriking;
    public bool isCoolDown = false;
    
    [Header("Damage")]
    public List<float> lightDamage;
    public int lightDamageIndex;
    public List<float> lastLightDamage;
    public int lastLightDamageIndex;
    
    public bool pushEnnemy = false;
    
    [Header("Combo")]
    public List<float> coolDown; // temps entre chaque coups
    public int coolDownIndex;
    public List<float> coolDownEndCombo; // temps entre chaque combo
    public int coolDownEndComboIndex;
    public float timerRemaining; // temps restant entre chaque coup pour arriver au combo
    public int combo; //nombre de coup que peut enchainner le joueur avant le coolDown final

    [Header("Cinemachine Schake")] 
    public float intensityLightCloseDamage;
    public float frequencyLightCloseDamage;
    public float timerLightCloseDamage;
    
}