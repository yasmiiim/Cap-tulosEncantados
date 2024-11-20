using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class portal : MonoBehaviour
{
    public string sceneToLoad;
    public bool hasStone; // Variável para verificar se o jogador tem a Pedra
    public MensagemPedra mensagemManager; // Referência ao script do gerenciador de mensagem

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (hasStone) // Verifica se o jogador tem a Pedra
            {
                SceneManager.LoadScene(sceneToLoad); // Avança para a próxima fase
            }
            else
            {
                Debug.Log("Você precisa da Pedra para avançar!");
                mensagemManager.MostrarMensagem(); // Mostra a mensagem de texto
            }
        }
    }
}
