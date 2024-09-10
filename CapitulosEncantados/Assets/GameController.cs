using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private void OnEnable()
    {
        VidaPlayer.OnPlayerDeath += ShowDeathMenu; // Inscreve-se no evento
    }

    private void OnDisable()
    {
        VidaPlayer.OnPlayerDeath -= ShowDeathMenu; // Desinscreve-se do evento
    }

    private void ShowDeathMenu()
    {
        if (DeathMenuManager.Instance != null)
        {
            DeathMenuManager.Instance.ShowDeathMenu();
            Time.timeScale = 0; // Pausa o jogo
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Despausa o jogo
        if (DeathMenuManager.Instance != null)
        {
            DeathMenuManager.Instance.HideDeathMenu();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Recarrega a cena atual
    }

    public void QuitGame()
    {
        Time.timeScale = 1; // Despausa o jogo
        if (DeathMenuManager.Instance != null)
        {
            DeathMenuManager.Instance.HideDeathMenu();
        }
    }
}
