using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class DialogueManager : MonoBehaviour
{
    [Header("cam")]
    public CinemachineVirtualCamera cam;
    
    [Header("Canvas")] 
    public GameObject ArchimageGO;
    public CanvasGroup ArchimageCG;
    public GameObject BtnInteraction;
    public CanvasGroup btnInteractionCG;
    public GameObject BtnScore;
    public CanvasGroup btnScoreCG;
    
    public GameObject Score;
    
    private Queue<string> sentences;
    [Space]
    [Header("Dialogue")]
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI dialogue;

    public Animator Animator;
    private bool OnOff = true;
    private void Start()
    {
        sentences = new Queue<string>();
        Score.SetActive(true);
        Score.GetComponent<CanvasGroup>().DOFade(0, 0);
    }

    private void Update()
    {
        if (DialogueCollider.instance.isInRange)
        {
            if (Introduction.instance.playIntro)
            {
                //cam.DOCinemachineOrthoSize(7, 2);
                ArchimageGO.SetActive(true);
                //BtnScore.SetActive(false);
                ArchimageCG.DOFade(1, .2f);
            }
            else
            {
                ArchimageGO.SetActive(true);
                /*BtnInteraction.SetActive(true);*/
                BtnScore.SetActive(true);
                
                ArchimageCG.DOFade(1, .2f);
                /*btnInteractionCG.DOFade(1, .2f);*/
                btnScoreCG.DOFade(1, .2f);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (OnOff)
                {
                    Score.GetComponent<CanvasGroup>().DOFade(1, 1.25f);
                    OnOff = false;
                }
                else
                {
                    Score.GetComponent<CanvasGroup>().DOFade(0, 1.25f);
                    OnOff = true;
                }
                
            }
        }
        else
        {
            if (!Introduction.instance.playIntro)
            {
                cam.DOCinemachineOrthoSize(10, 2);
            }
            
            ArchimageGO.SetActive(false);
            BtnInteraction.SetActive(false);
            BtnScore.SetActive(false);
            
            ArchimageCG.DOFade(0, .2f);
            btnInteractionCG.DOFade(0, .2f);
            btnScoreCG.DOFade(0, .2f);
            
            Score.GetComponent<CanvasGroup>().DOFade(0, 1.25f);
        }
    }


    public void StartDialogue(Dialogue dialogue)
    {
        PlayerController.instance.enabled = false;
        PlayerLightAttack.instance.enabled = false;
        PlayerHeavyAttack.instance.enabled = false;
        PlayerThrowAttack.instance.enabled = false;
        
        SceneManager.instance.playModeCG_.DOFade(0, 0.5f);

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
            PlayerHeavyAttack.instance.activate = false;
            PlayerThrowAttack.instance.enabled = true;
            
            SceneManager.instance.playModeCG_.DOFade(1,0.5f);

            if (Introduction.instance.playIntro)
            {
                Introduction.instance.EndIntro();
            }
        }
        else
        {
            GetComponent<DialogueTrigger>().TriggerDialogue();
        }
    }
}
