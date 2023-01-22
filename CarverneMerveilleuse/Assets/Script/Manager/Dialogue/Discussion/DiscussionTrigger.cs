using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscussionTrigger : MonoBehaviour
{
    public List<Discussion> dialogue;
    public int indexDialogue = 0;
    
    public void TriggerTuto()
    {
        FindObjectOfType<DiscussionManager>().StartDialogue(dialogue[indexDialogue]);
        indexDialogue++;
        Debug.Log(indexDialogue);
        Introduction.instance.Dialogue();
    }

    private void Start()
    {
        TriggerTuto();
        Debug.Log("hihiih");
    }
}
