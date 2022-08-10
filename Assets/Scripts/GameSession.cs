using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [SerializeField] private GameObject cameraController;
    [SerializeField] private GameObject player;
    private GameObject playerHold;
    [SerializeField] private GameObject goal;
    private GameObject goalHold;
    [SerializeField] private int playerLives = 3;
    
    private int gameScore = 0;

    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] private float deathTimer = 2.5f;

    [HideInInspector] public GameState gameState;
    [HideInInspector] public GameState previousGameState;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private CanvasGroup canvasGroup;

    private int currLevelIndex = 0;

    private Vector2 originPos;

    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;

        // Whenever a game session already exists then destroy this one
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        gameState = GameState.gameStarted;

        //StartCoroutine(Fade(0f, 1f, 0f, Color.black));
    }

    private void Update()
    {
        HandleGameStart();
    }

    private void GameStart()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = gameScore.ToString();

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            InstantiatePlayer(new Vector2(0f, 4f));
        }
    }

    public void InstantiatePlayer(Vector2 vecPos)
    {
        originPos = new Vector2(vecPos.x, vecPos.y);
        scoreText.text = gameScore.ToString();

        playerHold = Instantiate(player, originPos, Quaternion.identity);
        //playerHold.transform.SetParent(transform);
        cameraController.GetComponent<CinemachineTarget>().player = playerHold;
        cameraController.GetComponent<CinemachineTarget>().SetCinemachineTargetGroup();
        gameState = GameState.playingLevel;
    }

    public void InstantiateGoal(Vector2 vecPos)
    {
        goalHold = Instantiate(goal, vecPos, Quaternion.identity);
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            StartCoroutine(DecreaseLifeCounter());
        }
        else
        {
            ResetGameSession();
        }
    }

    public void AddToScore(int val)
    {
        gameScore += val;
        scoreText.text = gameScore.ToString();
    }


    IEnumerator DecreaseLifeCounter()
    {
        playerLives--;
        livesText.text = playerLives.ToString();

        yield return new WaitForSecondsRealtime(deathTimer);

        int currSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //SceneManager.LoadScene(currSceneIndex);
        Destroy(playerHold);
        InstantiatePlayer(originPos);
    }

    void PlayDungeonLevel(int val)
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(val);
        Destroy(gameObject);

        StartCoroutine(DisplayLevelText());
    }

    void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene("MainMenuScene");
        Destroy(gameObject);
    }

    void OnOpen(InputValue inputValue)
    {
        PauseGameMenu();
    }

    private void HandleGameStart()
    {
        switch(gameState)
        {
            // Used for when instantiating gamesession in menu screen
            case GameState.gameStarted:
                int currSceneIndex = SceneManager.GetActiveScene().buildIndex;
                int nextSceneIndex = currSceneIndex + 1;

                // Check to see if next scene is equal to total then reset nextSceneIndex
                if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
                {
                    nextSceneIndex = 0;
                }

                gameState = GameState.playingLevel;
                GameStart();

                break;

            // If in dungeon keep track of UI
            case GameState.playingLevel:
                
                break;

            // Before entering boss stage
            case GameState.bossStage:

                break;

            // Restart the game
            case GameState.restartGame:

                ResetGameSession();

                break;

            case GameState.gamePaused:


                break;
        }
    }

    /// <summary>
    /// Get level and make calls for displaying text
    /// </summary>
    private IEnumerator DisplayLevelText()
    {
        StartCoroutine(Fade(0f, 1f, 0f, Color.black));

        player.GetComponent<AgentMovement>().DisablePlayer();

        string message = "LEVEL " + currLevelIndex.ToString();

        yield return StartCoroutine(DisplayMessage(message, Color.white, 2f));

        player.GetComponent<AgentMovement>().EnablePlayer();

        yield return StartCoroutine(Fade(0f, 1f, 2f, Color.black));
    }

    /// <summary>
    /// 
    /// </summary>
    private IEnumerator DisplayMessage(string message, Color color, float displayTime)
    {
        messageText.SetText(message);
        messageText.color = color;

        yield return null;

        messageText.SetText("");
    }

    /// <summary>
    /// Used to fade screen and instantiate message for user
    /// </summary>
    /// 
    public IEnumerator Fade(float startAlpha, float targetAlpha, float timeToFade, Color color)
    {
        Image image = canvasGroup.GetComponent<Image>();
        image.color = color;

        float timeCount = 0;

        while (timeCount <= timeToFade)
        {
            timeCount += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timeCount / timeToFade);

            yield return null;
        }
    }

    /// <summary>
    /// Win Game
    /// </summary>
    /// 
    public void GameWon()
    {
        previousGameState = GameState.gameWon;

        player.GetComponent<AgentMovement>().DisablePlayer();

        int rank = FindObjectOfType<HighScoreManager>().GetRank(gameScore);

        string rankText;

        if (rank > 0 && rank <= Settings.numOfHighScoresTotal)
        {
            rankText = "YOUR SCORE IS RANKS " + rank.ToString("#0") + " IN THE TOP " + Settings.numOfHighScoresTotal.ToString("#0");

            string name = player.name;

            // Add score to highscore manager
            FindObjectOfType<HighScoreManager>().AddScore(new Score()
            {
                playerName = name,
                playerScore = gameScore
            }, rank);
        }
        else
        {
            rankText = "YOUR SCORE ISN'T RANKED IN THE TOP " + Settings.numOfHighScoresTotal.ToString("#0");
        }


        gameState = GameState.restartGame;
    }

    public void GameLost()
    {
        previousGameState = GameState.gameLost;

        player.GetComponent<AgentMovement>().DisablePlayer();

        gameState = GameState.restartGame;
    }

    /// <summary>
    /// Pause menu
    /// </summary>
    /// 
    private void PauseGameMenu()
    {
        if (gameState != GameState.gamePaused)
        {
            pauseMenu.SetActive(true);

            player.GetComponent<AgentMovement>().DisablePlayer();

            previousGameState = gameState;
            gameState = GameState.gamePaused;
        }
        else if (gameState == GameState.gamePaused)
        {
            pauseMenu.SetActive(false);

            player.GetComponent<AgentMovement>().EnablePlayer();

            gameState = previousGameState;
            previousGameState = GameState.gamePaused;
        }
    }
}
