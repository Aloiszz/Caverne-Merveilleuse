using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public GameObject scoreBoard;
    public TMP_Text score1;
    public TMP_Text score2;
    public TMP_Text score3;
    public TMP_Text score4;
    public TMP_Text score5;
    public TMP_Text lastScore;
    public float range;
    public bool useRange;
    public static ScoreBoard instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }

        instance = this;
    }

    private void Start()
    {
        score1.text = PlayerPrefs.GetInt("Score1").ToString();
        score2.text = PlayerPrefs.GetInt("Score2").ToString();
        score3.text = PlayerPrefs.GetInt("Score3").ToString();
        score4.text = PlayerPrefs.GetInt("Score4").ToString();
        score5.text = PlayerPrefs.GetInt("Score5").ToString();
        lastScore.text = PlayerPrefs.GetInt("LastScore").ToString();
    }

    private void Update()
    {
        if (useRange)
        {
            if ((PlayerController.instance.transform.position - transform.position).magnitude < range)
            {
                scoreBoard.SetActive(true);
            }
            else
            {
                scoreBoard.SetActive(false);
            }
        }

    }
    
}
