using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inimigo : MonoBehaviour
{
    public float speed = 5.0f; // Velocidade de movimento
    public Rigidbody2D enemyRbp;
    public int vida = 3;
    private bool faceFlip; // Controle de direção (virado para a direita ou para a esquerda)

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public Color damageColor = Color.red;
    public float colorChangeDuration = 0.2f;

    public float changeDirectionInterval = 2.0f; // Tempo em segundos para mudar de direção
    private Animator animator; // Referência ao Animator

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>(); // Pegando o Animator
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        // Inicia a repetição da troca de direção a cada intervalo de tempo definido
        InvokeRepeating("ChangeDirection", changeDirectionInterval, changeDirectionInterval);

        // Certifica-se de que o Rigidbody2D do inimigo está como Kinematic
        enemyRbp.bodyType = RigidbodyType2D.Kinematic;
    }

    private void Update()
    {
        // Faz o inimigo se mover continuamente para a esquerda ou direita
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // Aciona a animação de caminhada quando o inimigo estiver se movendo
        if (animator != null)
        {
            animator.SetBool("isWalking", true); // Assume que a animação de caminhada tem um parâmetro booleano chamado "isWalking"
        }
    }

    // Função que será chamada repetidamente para mudar a direção
    private void ChangeDirection()
    {
        faceFlip = !faceFlip; // Inverte a direção
        FlipEnemy();
    }

    private void FlipEnemy()
    {
        // Se o inimigo estiver de frente, vira de costas, e vice-versa
        if (faceFlip)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0); // Virado para a direita
        }
        else
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0); // Virado para a esquerda
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("bala"))
        {
            ApplyDamage(1);
            Destroy(col.gameObject);
        }

        // Verifica se o inimigo colidiu com o jogador
        if (col.gameObject.CompareTag("Player"))
        {
            // Aqui você pode adicionar a lógica de causar dano ao jogador
            // Exemplo: col.GetComponent<VidaPlayer>().ReceberDano(dano);
        }
    }

    private void ApplyDamage(int damageAmount)
    {
        vida -= damageAmount;
        if (vida <= 0)
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

    private void Die()
    {
        // Chama a animação de morte (se tiver)
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
        Destroy(this.gameObject);
    }
}
