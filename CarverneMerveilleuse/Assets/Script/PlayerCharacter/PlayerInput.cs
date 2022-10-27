using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public float pressed;
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            pressed += 1;
        }

        /*if (pressed > 15)
        {
            PlayerHeavyAttack.instance.HeavyAttack();
            Debug.Log("Heavy");
        }
        if(pressed < 15 && pressed > 5)
        {
            PlayerLightAttack.instance.LightAttack();
            Debug.Log("Light");
        }*/

        if (Input.GetMouseButtonUp(0))
        {
            pressed = 0;
        }
    }
}
