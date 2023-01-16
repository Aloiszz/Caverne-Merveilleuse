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
            else if(gameObject.name == "ZoneAOE")
            {
                PlayerController.instance.LoseLife(BossScript.instance.damage);
                PlayerController.instance.rb.AddForce((PlayerController.instance.transform.position-BossScript.instance.transform.position).normalized * BossScript.instance.puissancePushAOE *10, ForceMode2D.Impulse);
            }
            else
            {
                PlayerController.instance.LoseLife(BossScript.instance.damage);
            }
            
        }
    }

}
