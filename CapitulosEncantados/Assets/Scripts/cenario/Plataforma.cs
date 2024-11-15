using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataforma : MonoBehaviour
{
   public float moveSpeed = 2f;
    public bool platform1, platform2;

    public bool moveRight = true, moveUp = true;

    public float minYPosition = 0f; // Posição mínima no eixo Y
    public float maxYPosition = 0f; // Posição máxima no eixo Y

    public float minXPosition = 0f; // Posição mínima no eixo X
    public float maxXPosition = 0f; // Posição máxima no eixo X

    void Update()
    {
        if (platform1)
        {
            // Movimento horizontal
            if (transform.position.x > maxXPosition)
            {
                moveRight = false;
            }
            else if (transform.position.x < minXPosition)
            {
                moveRight = true;
            }

            if (moveRight)
            {
                transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            }
        }

        if (platform2)
        {
            // Movimento vertical
            if (transform.position.y > maxYPosition)
            {
                moveUp = false;
            }
            else if (transform.position.y < minYPosition)
            {
                moveUp = true;
            }

            if (moveUp)
            {
                transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o objeto que colidiu é o player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Define o player como filho da plataforma
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Verifica se o objeto que saiu da colisão é o player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Remove o player como filho da plataforma
            collision.transform.SetParent(null);
        }
    }
}
