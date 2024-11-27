using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    public Transform jogador; // Referência ao jogador
    public float velocidade = 5f; // Velocidade para acompanhar o jogador
    public Vector2 offset; // Deslocamento em relação ao jogador

    private Vector3 posicaoInicial;

    void Start()
    {
        // Armazena a posição inicial da lua
        posicaoInicial = transform.position;
    }

    void LateUpdate()
    {
        if (jogador != null)
        {
            // Calcula a posição alvo da lua
            Vector3 posicaoAlvo = new Vector3(
                jogador.position.x + offset.x,
                jogador.position.y + offset.y,
                posicaoInicial.z
            );

            // Move suavemente para a posição alvo
            transform.position = Vector3.Lerp(transform.position, posicaoAlvo, velocidade * Time.deltaTime);
        }
    }
}
