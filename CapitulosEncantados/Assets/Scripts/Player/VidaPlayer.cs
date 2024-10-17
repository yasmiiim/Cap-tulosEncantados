using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class VidaPlayer : MonoBehaviour
{
  private Vector3 respawnPoint;

    public int vidaAtual;
    public int vidaMaxima;

    public Image[] coracao;
    public Sprite cheio;
    public Sprite vazio;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public Color damageColor = Color.red;

    public float invulnerabilityDuration = 1.5f; // Tempo de invulnerabilidade após sofrer dano
    public float blinkInterval = 0.1f; // Intervalo entre piscadas
    private bool isInvulnerable = false; // Para verificar se o player está invulnerável

    public static event Action OnPlayerDeath; // Evento que será chamado ao morrer

    void Start()
    {
        vidaAtual = vidaMaxima;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        respawnPoint = transform.position;
    }

    public void ReceberDano()
    {
        if (isInvulnerable) return;  // Se o jogador estiver invulnerável, não faz nada

        vidaAtual -= 1;

        if (vidaAtual <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvulnerabilityPeriod()); // Ativa a invulnerabilidade por um tempo
        }
    }

    public void Die()
    {
        transform.position = respawnPoint;  // Reposiciona o jogador no ponto de respawn
        vidaAtual = vidaMaxima;            // Restaura a vida ao máximo
        HealthLogic();                     // Atualiza a interface de vida (corações)
        isInvulnerable = false;            // Garante que o jogador não fique invulnerável após morrer
        StopAllCoroutines();               // Garante que todas as corrotinas parem
        ResetColor();                      // Garante que a cor seja resetada ao renascer
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Checkpoint")
        {
            respawnPoint = transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D coli)
    {
        // Verifica se o objeto colidido tem a tag "enemy" e aplica dano
        if (coli.collider.CompareTag("enemy"))
        {
            ReceberDano();
        }
        // Verifica se o objeto colidido tem a tag "obstaculo" e aplica dano
        else if (coli.collider.CompareTag("obstaculo"))
        {
            ReceberDano();
        }
    }

    void Update()
    {
        HealthLogic();
    }

    void HealthLogic()
    {
        if (vidaAtual > vidaMaxima)
        {
            vidaAtual = vidaMaxima;
        }
        for (int i = 0; i < coracao.Length; i++)
        {
            if (i < vidaAtual)
            {
                coracao[i].sprite = cheio;
            }
            else
            {
                coracao[i].sprite = vazio;
            }

            if (i < vidaMaxima)
            {
                coracao[i].enabled = true;
            }
            else
            {
                coracao[i].enabled = false;
            }
        }
    }

    // Corrotina para o tempo de invulnerabilidade com efeito de piscar
    private IEnumerator InvulnerabilityPeriod()
    {
        isInvulnerable = true; // Define que o jogador está invulnerável

        // Apenas pisca duas vezes
        for (int i = 0; i < 2; i++)
        {
            // Alterna entre a cor original e a cor de dano (pisca)
            spriteRenderer.color = damageColor;  // Cor de dano
            yield return new WaitForSeconds(blinkInterval); // Espera o intervalo
            spriteRenderer.color = originalColor;  // Cor original
            yield return new WaitForSeconds(blinkInterval); // Espera o intervalo
        }

        isInvulnerable = false; // Remove a invulnerabilidade
        ResetColor(); // Garante que a cor seja restaurada ao final do tempo de invulnerabilidade
    }

    private void ResetColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;  // Reseta para a cor original
        }
    }

}
