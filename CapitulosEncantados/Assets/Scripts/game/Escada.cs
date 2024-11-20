using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escada : MonoBehaviour
{
     private float vertical;
    private float speed = 5f;
    private bool escada;
    private bool escalando;

    public Rigidbody2D playerRb;
    public Animator playerAnimator;

    private int isSubindoHash = Animator.StringToHash("isSubindo");
    public Transform escadaTopo; // Ponto no topo da escada
    public Transform escadaBase; // Ponto na base da escada

    void Update()
    {
        vertical = Input.GetAxis("Vertical");

        if (escada)
        {
            if (Mathf.Abs(vertical) > 0f)
            {
                escalando = true;
            }
            else if (escalando) // Permite que a animação de escada continue
            {
                playerRb.velocity = Vector2.zero;
            }
        }
        else
        {
            escalando = false;
        }

        if (playerAnimator != null)
        {
            playerAnimator.SetBool(isSubindoHash, escada);
        }

        if (escalando && playerRb.transform.position.y >= escadaTopo.position.y)
        {
            playerRb.velocity = Vector2.zero;
            playerRb.gravityScale = 2f;
            escada = false;
            escalando = false;
            playerAnimator.SetBool(isSubindoHash, false);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("escada"))
        {
            escada = true;
            playerRb.velocity = Vector2.zero;
            playerRb.gravityScale = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("escada"))
        {
            escada = false;
            escalando = false;
            playerRb.velocity = Vector2.zero;
            playerRb.gravityScale = 2f;

            if (playerAnimator != null)
            {
                playerAnimator.SetBool(isSubindoHash, false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (escada)
        {
            if (Mathf.Abs(vertical) > 0f)
            {
                playerRb.velocity = new Vector2(playerRb.velocity.x, vertical * speed);
            }
            else if (escalando) // Mantém o jogador parado na escada
            {
                playerRb.velocity = Vector2.zero;
            }

            if (vertical < 0 && playerRb.transform.position.y <= escadaBase.position.y)
            {
                playerRb.gravityScale = 2f;
                escada = false;
                escalando = false;

                if (playerAnimator != null)
                {
                    playerAnimator.SetBool(isSubindoHash, false);
                }
            }
        }
    }
}
