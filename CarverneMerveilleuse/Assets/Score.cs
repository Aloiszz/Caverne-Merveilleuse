using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int score;
    public int scoreRage;
    public List<int> listScore;
    public List<int> listScoreRage;
    public bool addScore;
    [Tooltip("Permet de reset le score dans les playerpref")]
    public bool resetPlayerPrefScore;

    public static Score instance;

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
        
        listScore.Add(PlayerPrefs.GetInt("Score1"));
        listScore.Add(PlayerPrefs.GetInt("Score2"));
        listScore.Add(PlayerPrefs.GetInt("Score3"));
        listScore.Add(PlayerPrefs.GetInt("Score4"));
        listScore.Add(PlayerPrefs.GetInt("Score5"));
    }

    private void Update()
    {
        if (addScore)
        {
            AddScore();
            addScore = false;
        }

        if (resetPlayerPrefScore)
        {
            PlayerPrefs.SetInt("Score1", 0);
            PlayerPrefs.SetInt("Score2", 0);
            PlayerPrefs.SetInt("Score3", 0);
            PlayerPrefs.SetInt("Score4", 0);
            PlayerPrefs.SetInt("Score5", 0);
        }
    }

    public void AddScore()
    {
        if (listScore.Count == 0)
        {
            listScore.Add(score);
        }
        else
        {
            for (int i = 0; i < listScore.Count; i++)
            {
                if (score >= listScore[i])
                {
                    listScore.Insert(i,score);
                    break;
                }

                if (score < listScore[i] && i + 1 == listScore.Count )
                {
                    listScore.Add(score);
                    break;
                }
            }
        }
        

        if (listScore.Count > 5)
        {
            listScore.RemoveAt(listScore.Count - 1);
        }
        PlayerPrefs.SetInt("Score1", listScore[0]);
        PlayerPrefs.SetInt("Score2", listScore[1]);
        PlayerPrefs.SetInt("Score3", listScore[2]);
        PlayerPrefs.SetInt("Score4", listScore[3]);
        PlayerPrefs.SetInt("Score5", listScore[4]);
        

    }
    

}
