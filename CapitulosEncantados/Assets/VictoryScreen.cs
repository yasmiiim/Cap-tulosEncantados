using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    public string firstLevelSceneName = "fase1";
    public string mainMenuSceneName = "Menu";

    public void PlayAgain()
    {
        SceneManager.LoadScene(firstLevelSceneName);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Jogo fechado!"); // Apenas para depuração no editor
    }
}
