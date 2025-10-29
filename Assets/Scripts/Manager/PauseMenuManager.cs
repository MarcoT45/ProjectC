using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    private bool isPaused = false;

    void Start()
    {
        if(pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
    }

    void Update()
    {
        if(ControlsManager.Instance.PausePressed)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        if(pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
            optionsMenuUI.SetActive(false);
            GameManager.Instance.PauseGame(false);
        }
        
        Time.timeScale = 1f; //Reprend le temps dans le jeu
        isPaused = false;
    }

    public void PauseGame()
    {
        if(pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
            optionsMenuUI.SetActive(false);
            GameManager.Instance.PauseGame(true);
        }


        Time.timeScale = 0f; //Stoppe le temps dans le jeu
        isPaused = true;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void OpenOptionMenu()
    {
        if(pauseMenuUI != null && optionsMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
            optionsMenuUI.SetActive(true);
        }
    }

    public void CloseOptionMenu()
    {
        if(pauseMenuUI != null && optionsMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
            optionsMenuUI.SetActive(false);
        }
    }
}
