using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abelha : MonoBehaviour
{
    public float beeSpeed = 2.0f; // Velocidade das abelhas
    private bool isMoving = false; // Controle para saber se as abelhas estão se movendo

    private Rigidbody2D rb;

    
    public int vida = 3;
    
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public Color damageColor = Color.red;
    public float colorChangeDuration = 0.2f;
    

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("bala"))
        {
            ApplyDamage(1);
            Destroy(col.gameObject);
        }
    }
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
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
        Destroy(this.gameObject);
    }
}
