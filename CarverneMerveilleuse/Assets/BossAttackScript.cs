using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackScript : MonoBehaviour
{
    private SpriteRenderer sprite;
    public bool prevention;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void Prevention()
    {
        if (prevention)
        {
            StartCoroutine(PreventionAttack());
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerController.instance.LoseLife();
        }
    }

    IEnumerator PreventionAttack()
    {
        sprite.color.g.Equals(1);
        yield return new WaitForSeconds(0.3f);
        sprite.color.g.Equals(0);
    }
}
