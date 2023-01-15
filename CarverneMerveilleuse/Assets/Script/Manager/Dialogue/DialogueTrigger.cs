using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    public List<Dialogue> dialogue;
    public int indexDialogue = 0;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue[indexDialogue]);
        indexDialogue++;
        Debug.Log(indexDialogue);
        Introduction.instance.Dialogue();
    }
}
