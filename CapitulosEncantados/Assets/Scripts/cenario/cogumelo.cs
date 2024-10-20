using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cogumelo : MonoBehaviour
{
    public float bounce = 20f;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Verifica se o contato é na parte de cima do cogumelo
            foreach (ContactPoint2D contact in other.contacts)
            {
                if (contact.normal.y < 0) // A colisão deve ocorrer de cima para baixo
                {
                    other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
                    break; // Sai do loop após aplicar a força
                }
            }
        }
    }
}
