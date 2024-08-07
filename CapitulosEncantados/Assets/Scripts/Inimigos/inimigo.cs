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

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("bala"))
        {
            ApplyDamage(1);
            Destroy(col.gameObject);
        }
    }

    private void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void FlipEnemy()
    {
        if (faceFlip)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col != null && !col.collider.CompareTag("Player"))
        {
            faceFlip = !faceFlip;
            FlipEnemy();
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
        Destroy(this.gameObject);
    }
}
