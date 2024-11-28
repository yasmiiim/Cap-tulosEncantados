using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameover : MonoBehaviour
{
    // Nome da cena do menu principal
      [SerializeField] private string menuSceneName = "Menu";
  
      // Nome da cena atual (ou recomeçar a cena manualmente)
      [SerializeField] private string currentSceneName = "";
  
      private void Start()
      {
          // Caso o nome da cena atual não esteja definido no Inspector, usar a cena ativa
          if (string.IsNullOrEmpty(currentSceneName))
          {
              currentSceneName = SceneManager.GetActiveScene().name;
          }
      }
  
      // Método chamado ao clicar no botão "Jogar Novamente"
      public void PlayAgain()
      {
          SceneManager.LoadScene(currentSceneName);
      }
  
      // Método chamado ao clicar no botão "Sair para o Menu"
      public void GoToMenu()
      {
          SceneManager.LoadScene(menuSceneName);
      }
  
      // Método para sair do jogo (opcional, útil para builds)
      public void QuitGame()
      {
          Debug.Log("Saindo do jogo...");
          Application.Quit();
      }
}
