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

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
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

        spriteRenderer.flipX = direction.x > 0;
    }

    private void AttackPlayer()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Vector2 direction = (player.position - firePoint.position).normalized;

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * 5f;
        }

        lastAttackTime = Time.time;

        animator.SetBool("isAttacking", true); // Define o estado de ataque
    }
}
