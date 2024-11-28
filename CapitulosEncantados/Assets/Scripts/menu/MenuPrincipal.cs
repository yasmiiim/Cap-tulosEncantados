using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    [SerializeField] private string nomeLevel;

    [Header("UI References")]
    [SerializeField] private GameObject painelInformacoes; // O painel que contém a imagem e o botão "X"

    public void Jogar()
    {
        SceneManager.LoadScene(nomeLevel);
    }

    public void Sair()
    {
        Debug.Log("Saiu do jogo");
        Application.Quit();
    }

    public void MostrarInformacoes()
    {
        painelInformacoes.SetActive(true); // Ativa o painel de informações
    }

    public void FecharInformacoes()
    {
        painelInformacoes.SetActive(false); // Desativa o painel de informações
    }
}
