using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caindoplat : MonoBehaviour
{
        private Rigidbody2D rb;                  // Referência ao Rigidbody2D da plataforma
    private bool jogadorEmCima = false;      // Verifica se o jogador está completamente em cima da plataforma
    private Vector3 posicaoInicial;          // Posição inicial da plataforma
    public float velocidadeTerminal = 1f;    // Velocidade máxima de descida

    private void Start()
    {
        // Armazena a posição inicial da plataforma
        posicaoInicial = transform.position;

        // Obtém o Rigidbody2D e desativa a gravidade inicialmente
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;  // Impede a rotação da plataforma
    }

    private void Update()
    {
        // Define a gravidade conforme o estado do jogador na plataforma
        if (jogadorEmCima)
        {
            rb.gravityScale = 1; // Ativa a gravidade quando o jogador está em cima

            // Limita a velocidade de descida para a velocidade terminal
            if (rb.velocity.y < -velocidadeTerminal)
            {
                rb.velocity = new Vector2(rb.velocity.x, -velocidadeTerminal);
            }
        }
        else
        {
            rb.gravityScale = 0;         // Desativa a gravidade quando o jogador sai
            rb.velocity = Vector2.zero;  // Zera a velocidade para garantir que pare
        }
    }

    private void OnTriggerEnter2D(Collider2D colisao)
    {
        if (colisao.gameObject.CompareTag("Player"))
        {
            jogadorEmCima = true; // O jogador está completamente em cima da plataforma
        }
    }

    private void OnTriggerExit2D(Collider2D colisao)
    {
        if (colisao.gameObject.CompareTag("Player"))
        {
            jogadorEmCima = false; // O jogador não está mais em cima da plataforma
        }
    }

    // Método para resetar a plataforma a partir de outro script (ex.: quando o jogador morre)
    public void ResetarPlataforma()
    {
        transform.position = posicaoInicial;
        jogadorEmCima = false;
        rb.gravityScale = 0;         // Desativa a gravidade novamente para resetar a plataforma
        rb.velocity = Vector2.zero;  // Zera a velocidade para que pare no ponto inicial
    }
}
