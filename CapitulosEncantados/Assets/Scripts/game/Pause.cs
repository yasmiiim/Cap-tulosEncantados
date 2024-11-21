using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
