using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine;
using TMPro;

public class DiscussionManager : MonoBehaviour
{
    
    private Queue<string> sentences;
    [Space]
    [Header("Dialogue")]
    public TextMeshProUGUI dialogue;
    
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void StartDialogue(Discussion dialogue)
    {
        sentences.Clear();

        foreach (var sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogue.text = "";
        foreach (var letter in sentence.ToCharArray())
        {
            dialogue.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {

        if (GetComponent<DiscussionTrigger>().indexDialogue == GetComponent<DiscussionTrigger>().dialogue.Count)
        {
            /*if (Introduction.instance.playIntro)
            {
                Introduction.instance.EndIntro(); // Fin du tuto
            }*/
        }
        else
        {
            GetComponent<DiscussionTrigger>().TriggerTuto();
        }
    }
}

