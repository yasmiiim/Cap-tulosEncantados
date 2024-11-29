using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class portaBoss : MonoBehaviour
{
    [SerializeField] private string nomeCena; // Nome da cena a ser carregada

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o jogador colidiu com a porta
        if (other.CompareTag("Player"))
        {
            Debug.Log("Colidiu com a porta. Carregando a cena: " + nomeCena);
            SceneManager.LoadScene(nomeCena);
        }
    }
}
