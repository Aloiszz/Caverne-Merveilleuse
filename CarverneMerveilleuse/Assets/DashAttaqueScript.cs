using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttaqueScript : MonoBehaviour
{
    private void Update()
    {
        if (PlayerController.instance.isDashing)
        {
            transform.position = PlayerController.instance.gameObject.transform.position;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.gameObject.layer == 6 | col.gameObject.layer == 13)
        {
            Debug.Log("Coucou");
            if (ItemManager.instance.isPushDashGet)
            {
                col.GetComponent<Rigidbody2D>().AddForce(-(PlayerController.instance.gameObject.transform.position - col.gameObject.transform.position).normalized * ItemManager.instance.puissancePushDash, ForceMode2D.Impulse);
            }

            if (ItemManager.instance.isDegatDashGet)
            {
                col.GetComponent<Mechant>().OtherHit();
            }
        }
    }
}
