using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 1f;
    public float attackDistance = 3f;
    public float attackCooldown = 3f;
    private float lastAttackTime;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public GameObject projectilePrefab;
    public Transform firePoint;

    public Image healthBar;

    public int maxHealth = 20;
    private int currentHealth;
    public float damageFlashDuration = 0.1f;

    private Vector3 initialPosition;

    public GameObject healthPotionPrefab;
    public GameObject speedPotionPrefab;

    private int damageCounter = 0;

    private Coroutine damageFlashCoroutine;
    
    public GameObject stonePrefab;

    

    private void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
        initialPosition = transform.position;
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
            animator.SetBool("isAttacking", false);
        }

        bool isPlayerOnRight = player.position.x > transform.position.x;
        spriteRenderer.flipX = isPlayerOnRight;

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

        animator.SetBool("isAttacking", true);
        Invoke(nameof(ShootProjectile), 0.4f);
        lastAttackTime = Time.time;
    }

    private void ShootProjectile()
    {
        if (!animator.GetBool("isAttacking")) return;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Vector2 direction = (player.position.x > transform.position.x) ? Vector2.right : Vector2.left;

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * 5f;
        }

        animator.SetBool("isAttacking", false);
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        damageCounter += damage;

        if (damageCounter >= 5 && damageCounter % 5 == 0)
        {
            DropPotion(healthPotionPrefab);
        }

        if (damageCounter >= 7 && damageCounter % 7 == 0)
        {
            DropPotion(speedPotionPrefab);
        }

        if (damageFlashCoroutine != null)
        {
            StopCoroutine(damageFlashCoroutine);
        }

        damageFlashCoroutine = StartCoroutine(DamageFlashRoutine());

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            UpdateHealthBar();
        }
    }

    private void DropPotion(GameObject potionPrefab)
    {
        if (potionPrefab != null)
        {
            float directionOffset = spriteRenderer.flipX ? 1f : -1f;
            Vector3 dropPosition = transform.position + new Vector3(directionOffset, 0f, 0f);
            Instantiate(potionPrefab, dropPosition, Quaternion.identity);
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    private IEnumerator DamageFlashRoutine()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(damageFlashDuration);
        spriteRenderer.color = originalColor;
        damageFlashCoroutine = null;
    }

    private void Die()
    {
        DropStone(); // Solta a pedra ao morrer
        Destroy(gameObject); // Remove o Boss da cena
    }

    private void DropStone()
    {
        if (stonePrefab != null)
        {
            Vector3 dropPosition = transform.position;
            Instantiate(stonePrefab, dropPosition, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("bala"))
        {
            TakeDamage(1);
            Destroy(col.gameObject);
        }
    }

    private void OnEnable()
    {
        VidaPlayer.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        VidaPlayer.OnPlayerDeath -= HandlePlayerDeath;
    }

    private void HandlePlayerDeath()
    {
        RestoreHealth();
        ResetPosition();
    }

    private void RestoreHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    private void ResetPosition()
    {
        transform.position = initialPosition;
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
    }
}
