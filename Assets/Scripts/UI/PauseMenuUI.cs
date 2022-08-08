using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Quit and load back into main menu scene from pause menu
    /// </summary>
    /// 
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
