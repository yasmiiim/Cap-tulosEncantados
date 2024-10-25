using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caindoplat : MonoBehaviour
{
    public float velocidadeDescida = 2f; // Velocidade de descida da plataforma
    private bool jogadorEmCima = false;  // Verifica se o jogador está em cima da plataforma
    private Vector3 posicaoInicial;      // Posição inicial da plataforma
    private bool plataformaResetada = false; // Verifica se a plataforma já foi resetada

    private void Start()
    {
        // Armazena a posição inicial da plataforma
        posicaoInicial = transform.position;
    }

    private void Update()
    {
        // Desce a plataforma apenas se o jogador estiver em cima e se não foi resetada
        if (jogadorEmCima && !plataformaResetada)
        {
            // Usa Lerp para suavizar a descida
            transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.down * velocidadeDescida, Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D colisao)
    {
        if (colisao.gameObject.CompareTag("Player"))
        {
            jogadorEmCima = true; // O jogador está em cima da plataforma
            plataformaResetada = false; // Permite o movimento de descida novamente
        }
    }

    private void OnCollisionExit2D(Collision2D colisao)
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
        plataformaResetada = true; // Marca a plataforma como resetada, evitando movimentos adicionais até que o jogador toque novamente
    }
}
