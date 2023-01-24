using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightShieldScript : MonoBehaviour
{
    private SpriteRenderer sr;
    public static LightShieldScript instance;
    public GameObject light;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance.gameObject);
        }

        instance = this;
    }

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(TimeBeforeDestroy());
    }

    private void Update()
    {
        transform.position = PlayerController.instance.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 6 && !col.gameObject.CompareTag("Boss"))
        {
            Mechant mechant = col.GetComponent<Mechant>();
            mechant.DebutHitStop();
            mechant.StartCoroutine(mechant.FinHitStop());
            mechant.rb.AddForce(-(PlayerController.instance.transform.position-col.transform.position).normalized * 100, ForceMode2D.Impulse);
            Destroy(gameObject);

        }

        if (col.gameObject.layer == 7)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator TimeBeforeDestroy()
    {
        yield return new WaitForSeconds(ItemManager.instance.tempsLightShield);
        for (int i = 0; i < 10; i++)
        {
            if (i%2 == 0)
            {
                light.SetActive(false);
                yield return new WaitForSeconds(0.2f);
            }
            else
            {
                light.SetActive(true);
                yield return new WaitForSeconds(0.2f);
            }
        }
        Destroy(gameObject);
    }
}
