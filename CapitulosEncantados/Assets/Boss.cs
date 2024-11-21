using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;
    public float attackDistance = 3f;
    public float attackCooldown = 1f;
    private float lastAttackTime;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public GameObject projectilePrefab;
    public Transform firePoint;

    // Sistema de vida
    public int maxHealth = 50;
    private int currentHealth;
    public float damageFlashDuration = 0.1f; // Duração do flash branco ao levar dano

    private void Start()
    {
        currentHealth = maxHealth; // Configura a vida inicial
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackDistance)
        {
            AttackPlayer();
            animator.SetBool("isWalking", false);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false); // Para de atacar quando fora da distância
        }

        // Verificar a direção do jogador
        bool isPlayerOnRight = player.position.x > transform.position.x;

        // Virar o Boss e o firePoint (invertendo o flipX e a posição do firePoint)
        spriteRenderer.flipX = isPlayerOnRight;

        // Ajustar a posição do firePoint para seguir a rotação do Boss
        if (isPlayerOnRight)
        {
            firePoint.localPosition = new Vector3(Mathf.Abs(firePoint.localPosition.x), firePoint.localPosition.y, firePoint.localPosition.z);
        }
        else
        {
            firePoint.localPosition = new Vector3(-Mathf.Abs(firePoint.localPosition.x), firePoint.localPosition.y, firePoint.localPosition.z);
        }
    }

    private void AttackPlayer()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        animator.SetBool("isAttacking", true); // Define o estado de ataque
        Invoke(nameof(ShootProjectile), 0.4f); // Dispara após 0.4 segundos
        lastAttackTime = Time.time;
    }

    private void ShootProjectile()
    {
        if (!animator.GetBool("isAttacking")) return; // Verifica se ainda está atacando

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Calcula a direção com base apenas na posição relativa do jogador
        Vector2 direction = (player.position.x > transform.position.x) ? Vector2.right : Vector2.left;

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * 5f; // Define a velocidade do projétil
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Indica que o Boss foi atingido com um flash branco
        StartCoroutine(DamageFlash());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator DamageFlash()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.white; // Fica branco
        yield return new WaitForSeconds(damageFlashDuration);
        spriteRenderer.color = originalColor; // Volta à cor original
    }

    private void Die()
    {
        Destroy(gameObject); // Destroi o Boss
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("bala"))
        {
            TakeDamage(1);
            Destroy(col.gameObject);
        }
    }
}
