using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bolaFogo : MonoBehaviour
{
    public float alturaMaxima = 5f;  // Altura máxima que a bola de fogo vai alcançar
    public float velocidade = 2f;      // Velocidade da subida e descida
    private Vector3 posicaoInicial;    // Posição inicial da bola de fogo

    private void Start()
    {
        posicaoInicial = transform.position;  // Armazena a posição inicial
    }

    private void Update()
    {
        // Calcula a nova posição da bola de fogo
        float novaAltura = Mathf.Sin(Time.time * velocidade) * alturaMaxima;
        transform.position = new Vector3(posicaoInicial.x, posicaoInicial.y + novaAltura, posicaoInicial.z);

        // Verifica se a bola atingiu a altura de 5
        if (transform.position.y >= posicaoInicial.y + 5f)
        {
            transform.localEulerAngles = new Vector3(0, 0, 180); // Vira de ponta-cabeça
        }
        else if (transform.position.y <= posicaoInicial.y)
        {
            transform.localEulerAngles = new Vector3(0, 0, 0); // Volta ao normal
        }
    }
}
