using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escada : MonoBehaviour
{
    private float vertical;
    private float speed = 8f;
    private bool escada;
    private bool escalando;

    public Rigidbody2D playerRb;

    void Update()
    {
        vertical = Input.GetAxis("Vertical");
        if (escada && Mathf.Abs(vertical) > 0f)
        {
            escalando = true;
        }
        else if (!escada)
        {
            escalando = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("escada"))
        {
            escada = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("escada"))
        {
            escada = false;
            escalando = false;
        }
    }

    private void FixedUpdate()
    {
        if (escalando)
        {
            playerRb.gravityScale = 0f;
            playerRb.velocity = new Vector2(playerRb.velocity.x, vertical * speed);
        }
        else
        {
            playerRb.gravityScale = 2f;
        }
    }
}
