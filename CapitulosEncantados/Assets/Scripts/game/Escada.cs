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

    void Update()
    {
        vertical = Input.GetAxis("Vertical");

        if (escada && Mathf.Abs(vertical) > 0f)
        {
            escalando = true;
        }
        else
        {
            escalando = false;
        }

        if (playerAnimator != null)
        {
            playerAnimator.SetBool(isSubindoHash, escada && escalando);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("escada"))
        {
            escada = true;
            playerRb.velocity = new Vector2(playerRb.velocity.x, 0f);
            playerRb.gravityScale = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("escada"))
        {
            escada = false;
            escalando = false;
            playerRb.velocity = new Vector2(playerRb.velocity.x, 0f);
            playerRb.gravityScale = 2f;

            if (playerAnimator != null)
            {
                playerAnimator.SetBool(isSubindoHash, false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (escada && escalando)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, vertical * speed);
        }
    }
}
