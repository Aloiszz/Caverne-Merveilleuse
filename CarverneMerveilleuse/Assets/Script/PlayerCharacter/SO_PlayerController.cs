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
    
}
