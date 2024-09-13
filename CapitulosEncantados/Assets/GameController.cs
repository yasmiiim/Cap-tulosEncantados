using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private void OnEnable()
    {
        VidaPlayer.OnPlayerDeath += ShowDeathMenu; 
    }

    private void OnDisable()
    {
        VidaPlayer.OnPlayerDeath -= ShowDeathMenu; 
    }

    private void ShowDeathMenu()
    {
        if (DeathMenuManager.Instance != null)
        {
            DeathMenuManager.Instance.ShowDeathMenu();
            Time.timeScale = 0; 
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1; 
        if (DeathMenuManager.Instance != null)
        {
            DeathMenuManager.Instance.HideDeathMenu();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }
}
