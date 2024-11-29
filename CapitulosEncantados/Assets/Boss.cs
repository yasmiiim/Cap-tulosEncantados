using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
      public Transform player;
    public float moveSpeed = 1f;
    public float attackDistance = 3f;
    public float attackCooldown = 1.5f;
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

    public GameObject deathParticlesPrefab;

    public Animator fadeAnimator; // Animator do fade
    private bool isTransitioning = false; // Para evitar múltiplas ativações

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

        // Calcula a direção em relação ao jogador
        Vector2 direction = (player.position - firePoint.position).normalized;

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * 3f; // Ajuste a velocidade aqui
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
        if (deathParticlesPrefab != null)
        {
            GameObject deathParticles =  Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
            Destroy(deathParticles, 2f);
        }
        
        spriteRenderer.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        animator.enabled = false;
        
        DropStone(); // Solta a pedra ao morrer
        StartCoroutine(HandleDeath());
    }
    
    private IEnumerator HandleDeath()
    {
        
        if (fadeAnimator != null)
        {
            fadeAnimator.SetTrigger("End");
        }

        // Aguarda o tempo do fade-out
        float fadeOutDuration = GetAnimationDuration("levelend");
        yield return new WaitForSeconds(fadeOutDuration);

        // Carrega a cena de vitória
        SceneManager.LoadScene("vitoria");
        
        
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
        StartCoroutine(TransicaoCena("gameover")); // Faz a transição para a cena de game over
    }

    private IEnumerator TransicaoCena(string sceneName)
    {
        if (isTransitioning) yield break;

        isTransitioning = true;

        if (fadeAnimator != null)
        {
            fadeAnimator.SetTrigger("End");
        }

        float fadeOutDuration = GetAnimationDuration("levelend"); // Nome do clipe de fade out no Animator
        yield return new WaitForSeconds(fadeOutDuration);

        SceneManager.LoadScene(sceneName);
    }

    private float GetAnimationDuration(string animationName)
    {
        if (fadeAnimator == null) return 0f;

        AnimationClip[] clips = fadeAnimator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == animationName)
            {
                return clip.length;
            }
        }

        return 0f; // Retorna 0 caso não encontre a animação
    }
//
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
