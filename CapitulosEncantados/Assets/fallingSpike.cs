using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingSpike : MonoBehaviour
{
    public float fallDelay = 1f; // Tempo antes de o espinho cair
    private Rigidbody2D rb;
    private bool playerDetected = false;
    private Vector3 initialPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position; // Armazena a posição inicial do espinho
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !playerDetected)
        {
            playerDetected = true;
            Invoke("DropSpike", fallDelay); // Chama o método após o atraso
        }
    }

    private void DropSpike()
    {
        rb.bodyType = RigidbodyType2D.Dynamic; // Faz o espinho cair
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            VidaPlayer player = collision.gameObject.GetComponent<VidaPlayer>(); // Busca o script do jogador
            if (player != null)
            {
                player.Die(); // Chama o método de "morte" do jogador
                Destroy(this.gameObject);
            }
        }
    }

    // Método para resetar a posição do espinho
    public void ResetSpike()
    {
        transform.position = initialPosition; // Reposiciona o espinho na posição inicial
        rb.bodyType = RigidbodyType2D.Kinematic; // Reseta o tipo do Rigidbody para kinematic
        playerDetected = false; // Reseta o estado de detecção do jogador
    }
}