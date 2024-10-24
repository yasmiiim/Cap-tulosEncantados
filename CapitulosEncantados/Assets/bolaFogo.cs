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
        
        // Atualiza a posição da bola de fogo
        transform.position = new Vector3(posicaoInicial.x, posicaoInicial.y + novaAltura, posicaoInicial.z);
    }
}
