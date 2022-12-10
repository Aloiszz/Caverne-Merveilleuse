using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;
public class DialogueManager : MonoBehaviour
{
    [Header("Canvas")] 
    public GameObject ArchimageGO;
    public CanvasGroup ArchimageCG;
    
    
    private Queue<string> sentences;
    [Space]
    [Header("Dialogue")]
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI dialogue;

    public Animator Animator;
    private void Start()
    {
        sentences = new Queue<string>();
    }

    private void Update()
    {
        if (DialogueCollider.instance.isInRange)
        {
            ArchimageGO.SetActive(true);
            ArchimageCG.DOFade(1, .2f);
        }
        else
        {
            ArchimageGO.SetActive(false);
            ArchimageCG.DOFade(1, .2f);
        }
    }


    public void StartDialogue(Dialogue dialogue)
    {
        PlayerController.instance.enabled = false;
        PlayerLightAttack.instance.enabled = false;
        PlayerHeavyAttack.instance.enabled = false;
        PlayerThrowAttack.instance.enabled = false;

        Animator.SetBool("isOpen", true);
        nameTxt.text = dialogue.name;
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

        if (GetComponent<DialogueTrigger>().indexDialogue == GetComponent<DialogueTrigger>().dialogue.Count)
        {
            Animator.SetBool("isOpen", false);
            Debug.Log("Fin");
            PlayerController.instance.enabled = true;
            PlayerLightAttack.instance.enabled = true;
            PlayerHeavyAttack.instance.enabled = true;
            PlayerThrowAttack.instance.enabled = true;
        }
        else
        {
            GetComponent<DialogueTrigger>().TriggerDialogue();
        }
    }
}
