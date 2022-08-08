using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject playTutorialButton;
    [SerializeField] private GameObject playGameButton;
    [SerializeField] private GameObject ruleButton;
    [SerializeField] private GameObject highScoreButton;
    [SerializeField] private GameObject returnToMainMenuButton;
    [SerializeField] private GameObject exitGameButton;

    private bool isRulesSceneLoaded = false;
    private bool isHighScoresSceneLoaded = false;

    // Start is called before the first frame update
    void Start()
    {
        returnToMainMenuButton.SetActive(false);
    }

    public void PlayTutorial()
    {
        SceneManager.LoadScene("TutorialGameScene");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void LoadHighScores()
    {
        playTutorialButton.SetActive(false);
        playGameButton.SetActive(false);
        highScoreButton.SetActive(false);
        isHighScoresSceneLoaded = true;

        returnToMainMenuButton.SetActive(true);

        SceneManager.LoadScene("HighScoreScene", LoadSceneMode.Additive);
    }

    public void LoadRules()
    {
        exitGameButton.SetActive(false);
        playTutorialButton.SetActive(false);
        playGameButton.SetActive(false);
        highScoreButton.SetActive(false);
        ruleButton.SetActive(false);

        returnToMainMenuButton.SetActive(true);

        isRulesSceneLoaded = true;

        SceneManager.LoadScene("RulesScene", LoadSceneMode.Additive);
    }

    public void ReturnToMainMenu()
    {
        if (isHighScoresSceneLoaded)
        {
            SceneManager.UnloadSceneAsync("HighScoreScene");
            isHighScoresSceneLoaded = false;
        }
        
        if (isRulesSceneLoaded)
        {
            SceneManager.UnloadSceneAsync("RulesScene");
            isRulesSceneLoaded = false;
        }

        returnToMainMenuButton.SetActive(false);
        playTutorialButton.SetActive(true);
        playGameButton.SetActive(true);
        highScoreButton.SetActive(true);
        ruleButton.SetActive(true);
        exitGameButton.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
