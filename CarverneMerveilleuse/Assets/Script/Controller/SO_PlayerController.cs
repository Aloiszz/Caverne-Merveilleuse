using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerController", menuName = "ScriptableObjects/new Player Controller", order = 1)]

public class SO_PlayerController : ScriptableObject
{
    [Header("Movement")]
    public float speedMovement;

    [Header("Dash")] 
    public bool isDash = true;
    public float dashForce;
    public float dashReload;
    public float dashInvinsibleTime;

    [Header("Life")]
    public int life;
    public float invinsibleTimer;

    [Header("Drag")]
    public float linearDragDeceleration;
    public float linearDragMultiplier;

    [Header("Damage")] 
    public bool isStriking;
    public bool isCoolDown;
    public float heavyCloseDamage;
    [Header(" ")] //Just space between variable
    public float lightCloseDamage;
    public float lightCloseDamageCoolDown;
    public float lightRangeDamage;

    [Header("Cinemachine Schake")] 
    public float intensityLightCloseDamage;
    public float frequencyLightCloseDamage;
    public float timerLightCloseDamage;
    [Header(" ")] //Just space between variable
    public float intensityHeavyCloseDamage;
    public float frequencyHeavyCloseDamage;
    public float timerHeavyCloseDamage;
}
