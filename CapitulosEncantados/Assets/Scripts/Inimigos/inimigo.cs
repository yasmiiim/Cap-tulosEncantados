using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inimigo : MonoBehaviour
{
    public float speed = 5.0f; 
    public Rigidbody2D enemyRbp;
    public int vida = 3;
    private bool faceFlip; 

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public Color damageColor = Color.red;
    public float colorChangeDuration = 0.2f;

    public float changeDirectionInterval = 2.0f; 
    private Animator animator; 

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>(); 
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        InvokeRepeating("ChangeDirection", changeDirectionInterval, changeDirectionInterval);
        
        enemyRbp.bodyType = RigidbodyType2D.Kinematic;
    }

    private void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        
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
