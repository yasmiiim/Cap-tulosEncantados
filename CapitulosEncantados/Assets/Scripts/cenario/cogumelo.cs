using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cogumelo : MonoBehaviour
{
       public float bounce = 20f; // Força do pulo
    public Animator animator; // Animator do cogumelo

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D contact in other.contacts)
            {
                // Verifica se o jogador está colidindo na parte superior do cogumelo
                if (contact.normal.y < 0)
                {
                    Rigidbody2D playerRb = other.gameObject.GetComponent<Rigidbody2D>();
                    Animator playerAnimator = other.gameObject.GetComponent<Animator>();

                    // Ativa o parâmetro isJumping no Animator do cogumelo
                    if (animator != null)
                    {
                        animator.SetBool("isJumping", true); // Ativa a animação
                        StartCoroutine(ResetIsJumping());   // Reseta após um curto tempo
                    }

                    // Configura o Animator do jogador, se disponível
                    if (playerAnimator != null)
                    {
                        playerAnimator.SetBool("Pulando", true);
                    }

                    // Aplica o impulso no jogador
                    if (playerRb != null)
                    {
                        playerRb.velocity = new Vector2(playerRb.velocity.x, bounce);
                    }

                    // Reseta o estado "Pulando" no jogador após um curto tempo
                    if (playerAnimator != null)
                    {
                        StartCoroutine(ResetPulando(playerAnimator));
                    }

                    break;
                }
            }
        }
    }

    private IEnumerator ResetIsJumping()
    {
        // Aguarda um curto período antes de desativar o parâmetro
        yield return new WaitForSeconds(0.2f);
        if (animator != null)
        {
            animator.SetBool("isJumping", false);
        }
    }

    private IEnumerator ResetPulando(Animator playerAnimator)
    {
        yield return new WaitForSeconds(0.1f);
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("Pulando", false);
        }
    }
}
