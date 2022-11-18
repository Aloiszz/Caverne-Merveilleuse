using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerThrowAttack", menuName ="ScriptableObjects/new Player Throw Attack", order = 1)]

public class SO_Player_ThrowAttack : ScriptableObject
{
    
    [Header("Weapons Spec")]
    public  int maxBounce = 3; // max de rebond possible
    public float distance; // distance max d'affichage du raycast

    [Header(" " +
            " ")]
    public List<float> ThrowSpeed; // vitesse de déplacement de l'arme
    public int ThrowSpeedIndex;
    
    public List<float> ThrowDamage; // degat de l'arme
    public int ThrowDamageIndex;
    
    
}
