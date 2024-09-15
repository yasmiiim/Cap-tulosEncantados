using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abelha : MonoBehaviour
{
    public float beeSpeed = 2.0f; // Velocidade das abelhas
    private bool isMoving = false; // Controle para saber se as abelhas estão se movendo

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero; // As abelhas começam paradas
    }

    void Update()
    {
        // Se o movimento estiver ativado, as abelhas se moverão para a esquerda
        if (isMoving)
        {
            rb.velocity = Vector2.left * beeSpeed;
        }
    }

    // Método para ativar o movimento das abelhas
    public void StartMoving()
    {
        isMoving = true;
    }

    // Método chamado quando a abelha sai da tela
    private void OnBecameInvisible()
    {
        Destroy(gameObject); // Destrói a abelha
    }
}
