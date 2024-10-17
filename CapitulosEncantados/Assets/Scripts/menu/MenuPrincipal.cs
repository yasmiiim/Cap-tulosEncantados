using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    [SerializeField] private string nomeLevel;
    public void Jogar()
    {
        SceneManager.LoadScene(nomeLevel);
    }

    public void Sair()
    {
        Debug.Log("Saiu do jogo");
        Application.Quit();
    }
}
