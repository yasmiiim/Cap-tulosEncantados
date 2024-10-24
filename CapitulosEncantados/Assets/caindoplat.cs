using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caindoplat : MonoBehaviour
{
    public float velocidadeDescida = 2f; // Velocidade de descida da plataforma
    private bool jogadorEmCima = false;  // Verifica se o jogador está em cima da plataforma

    private void Update()
    {
        // Desce a plataforma apenas se o jogador estiver em cima
        if (jogadorEmCima)
        {
            transform.position += Vector3.down * velocidadeDescida * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D colisao)
    {
        if (colisao.gameObject.CompareTag("Player"))
        {
            jogadorEmCima = true; // O jogador está em cima da plataforma
        }
    }

    private void OnCollisionExit2D(Collision2D colisao)
    {
        if (colisao.gameObject.CompareTag("Player"))
        {
            jogadorEmCima = false; // O jogador não está mais em cima da plataforma
        }
    }
}
