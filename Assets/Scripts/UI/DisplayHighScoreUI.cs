using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHighScoreUI : MonoBehaviour
{
    [SerializeField] private GameObject scoreObj;
    [SerializeField] private Transform anchorPos;

    private void Start()
    {
        DisplayScores();
    }

    private void DisplayScores()
    {
        HighScores highScores = FindObjectOfType<HighScoreManager>().GetHighScores();

        GameObject scoreInstance;

        int rank = 0;

        foreach (Score score in highScores.scoreList)
        {
            rank++;

            scoreInstance = Instantiate(scoreObj, anchorPos);

            ScorePrefab scorePrefab = scoreInstance.GetComponent<ScorePrefab>();

            scorePrefab.rankText.text = rank.ToString();
            scorePrefab.nameText.text = score.playerName;
            scorePrefab.scoreText.text = score.playerScore.ToString();
        }

        scoreInstance = Instantiate(scoreObj, anchorPos);
    }
}
