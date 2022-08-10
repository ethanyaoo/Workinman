using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private float timerTimeTotal = 10f;
    [SerializeField] private float timeLeft;

    private bool timerIsOn = false;

    public TextMeshProUGUI textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = timerTimeTotal;
        timerIsOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsOn)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                DisplayTimeUI(timeLeft);
            }
            else
            {
                timeLeft = 0;
                timerIsOn = false;

                FindObjectOfType<GameSession>().GameLost();
            }
        }
    }

    private void DisplayTimeUI(float timeLeftInTimer)
    {
        timeLeftInTimer += 1;

        float minutes = Mathf.FloorToInt(timeLeftInTimer / 60);
        float seconds = Mathf.FloorToInt(timeLeftInTimer % 60);

        textMeshPro.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
