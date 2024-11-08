using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Mathematics;
using UnityEngine.UI;

public class VidaPlayer : MonoBehaviour
{
    public ParticleSystem particulaExpPref;
    private Vector3 respawnPoint;

    public int vidaAtual;
    public int vidaMaxima;

    public Image[] coracao;
    public Sprite cheio;
    public Sprite vazio;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public Color damageColor = Color.red;

    public float invulnerabilityDuration = 1.5f;
    public float blinkInterval = 0.1f;
    private bool isInvulnerable = false;

    public static event Action OnPlayerDeath;

    private Collider2D playerCollider;
    private Rigidbody2D playerRigidbody;

    // Adiciona uma referência pública para o script caindoplat
    public caindoplat plataforma;

    // Adiciona uma referência pública para o script da nuvem
    public nuvem cloud;

    void Start()
    {
        vidaAtual = vidaMaxima;
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
        playerRigidbody = GetComponent<Rigidbody2D>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        respawnPoint = transform.position;
    }

    public void ReceberDano()
    {
        if (isInvulnerable) return;

        vidaAtual -= 1;

        if (vidaAtual <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvulnerabilityPeriod());
        }
    }

    public void Die()
    {
        vidaAtual = vidaMaxima;
        HealthLogic();
        isInvulnerable = false;
        StopAllCoroutines();
        ResetColor();

        StartCoroutine(RespawnAfterParticles());
    }

    private IEnumerator RespawnAfterParticles()
    {
        spriteRenderer.enabled = false;
        playerCollider.enabled = false;
        playerRigidbody.simulated = false;

        ParticleSystem particulaExplosao = Instantiate(this.particulaExpPref, this.transform.position, Quaternion.identity);

        yield return new WaitWhile(() => particulaExplosao.IsAlive(true));

        Destroy(particulaExplosao.gameObject);

        // Reposiciona o jogador imediatamente após as partículas desaparecerem
        transform.position = respawnPoint;

        // Reseta a posição da plataforma chamando o método ResetarPlataforma
        if (plataforma != null)
        {
            plataforma.ResetarPlataforma();
        }

        // Reseta a nuvem chamando o método ResetCloud, para garantir que ela reapareça
        if (cloud != null)
        {
            cloud.ResetCloud();
        }

        // Reativa o jogador após o término
        spriteRenderer.enabled = true;
        playerCollider.enabled = true;
        playerRigidbody.simulated = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Checkpoint")
        {
            respawnPoint = transform.position;
        }
        else if (collision.tag == "Coracao")
        {
            ColetarCoracao();
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D coli)
    {
        if (coli.collider.CompareTag("enemy"))
        {
            ReceberDano();
        }
        else if (coli.collider.CompareTag("obstaculo"))
        {
            Die();
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

    private void ColetarCoracao()
    {
        if (vidaAtual < vidaMaxima)
        {
            vidaAtual += 1;
            if (vidaAtual > vidaMaxima)
            {
                vidaAtual = vidaMaxima;
            }
            HealthLogic();
        }
    }

    private IEnumerator InvulnerabilityPeriod()
    {
        isInvulnerable = true;

        for (int i = 0; i < 2; i++)
        {
            spriteRenderer.color = damageColor;
            yield return new WaitForSeconds(blinkInterval);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(blinkInterval);
        }

        isInvulnerable = false;
        ResetColor();
    }

    private void ResetColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }
}
