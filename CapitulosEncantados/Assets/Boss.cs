using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform player; // Referência ao jogador
    public float moveSpeed = 2f; // Velocidade de movimento do boss
    private SpriteRenderer spriteRenderer; // Componente de renderização do sprite
    private Animator animator; // Componente de animação

    private void Start()
    {
        // Obtém os componentes necessários
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

        // Calcula a direção para o jogador
        Vector2 direction = (player.position - transform.position).normalized;

        // Move o boss na direção do jogador
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        // Atualiza a animação
        animator.SetBool("isWalking", true);

        // Corrige o flipX baseado na posição do jogador
        if (direction.x > 0)
        {
            spriteRenderer.flipX = true; // Vira para a direita
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = false; // Vira para a esquerda
        }
    }
}
