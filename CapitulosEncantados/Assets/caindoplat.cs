using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caindoplat : MonoBehaviour
{
     public float velocidadeDescida = 2f; // Velocidade de descida da plataforma
    private bool jogadorEmCima = false;   // Verifica se o jogador está em cima da plataforma
    private Vector3 posicaoInicial;       // Posição inicial da plataforma
    private bool plataformaResetada = false; // Verifica se a plataforma já foi resetada
    private Vector3 posicaoDestino;       // Posição final da descida

    private void Start()
    {
        // Armazena a posição inicial da plataforma
        posicaoInicial = transform.position;
        posicaoDestino = posicaoInicial + Vector3.down * 15f; // Define o quanto a plataforma deve descer
    }

    private void Update()
    {
        // Desce a plataforma apenas se o jogador estiver em cima e se não foi resetada
        if (jogadorEmCima && !plataformaResetada)
        {
            // Usa Lerp para suavizar a descida até a posição de destino
            transform.position = Vector3.Lerp(transform.position, posicaoDestino, velocidadeDescida * Time.deltaTime);

            // Verifica se a plataforma chegou próximo o suficiente do destino para parar o movimento
            if (Vector3.Distance(transform.position, posicaoDestino) < 15f)
            {
                plataformaResetada = true; // Marca como resetada para evitar movimentos adicionais
            }
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
        plataformaResetada = false; // Permite que a plataforma se mova novamente na próxima colisão
    }
}
