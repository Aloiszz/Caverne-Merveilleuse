using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (gameObject.name == "Chute(Clone)")
            {
                PlayerController.instance.LoseLife(BossScript.instance.chuteDamage);
            }
            else
            {
                PlayerController.instance.LoseLife(BossScript.instance.damage);
            }
            
        }
    }

}
