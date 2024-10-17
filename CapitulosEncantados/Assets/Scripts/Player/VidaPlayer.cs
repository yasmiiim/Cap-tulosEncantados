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
    public float colorChangeDuration = 0.2f;

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
        vidaAtual -= 1;

        if (vidaAtual <= 0)
        {
            Die();
        }
        else
        {
            ChangeColor();
        }
    }

    private void ChangeColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = damageColor;
            Invoke("ResetColor", colorChangeDuration);
        }
    }

    private void ResetColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }

    public void Die()
    {
        transform.position = respawnPoint;  // Reposiciona o jogador no ponto de respawn
        vidaAtual = vidaMaxima;            // Restaura a vida ao máximo
        HealthLogic();                     // Atualiza a interface de vida (corações)
        // OnPlayerDeath?.Invoke(); // Aciona o evento de morte (caso seja necessário)
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
    

}
