using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Importar o SceneManager

public class Pause : MonoBehaviour
{
    public Transform pauseMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.gameObject.activeSelf)
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
        pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    private void PauseGame()
    {
        pauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    // Método para carregar o menu inicial
    public void LoadMainMenu()
    {
        Time.timeScale = 1; // Certificar que o tempo volta ao normal antes de trocar de cena
        SceneManager.LoadScene("Menu"); 
    }
}