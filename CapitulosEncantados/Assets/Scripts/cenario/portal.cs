using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class portal : MonoBehaviour
{
    public string sceneToLoad; 
    public bool hasStone; // Variável para verificar se o jogador tem a Pedra

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
                // Aqui você pode adicionar um feedback visual ou sonoro para avisar o jogador
            }
        }
    }
}
