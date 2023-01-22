using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDashIntro : MonoBehaviour
{
    
    [SerializeField] private DiscussionTrigger _discussionTrigger;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            NextStepTuto();
            Debug.Log("Collider");
            Destroy(gameObject);
        }
    }


    void NextStepTuto()
    {
        _discussionTrigger.TriggerTuto();
    }
}
