using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataforma : MonoBehaviour
{
    public float moveSpeed = 2f;
    public bool platform1, platform2;

    public bool moveRight = true, moveUp = true;

    public float minYPosition = 0f; // Posição mínima no eixo Y (para evitar encostar no chão)
    public float maxYPosition = 0f; // Posição máxima no eixo Y (pode ser ajustada no Unity)

    void Update()
    {
        if (platform1)
        {
            if (transform.position.x > -5)
            {
                moveRight = false;
            }
            else if (transform.position.x < -8)
            {
                moveRight = true;
            }

            if (moveRight)
            {
                transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector2.right * -moveSpeed * Time.deltaTime);
            }
        }
        
        if (platform2)
        {
            // Verifica se a plataforma está acima da posição máxima
            if (transform.position.y > maxYPosition)
            {
                moveUp = false;
            }
            else if (transform.position.y < minYPosition) // Altera aqui para usar minYPosition
            {
                moveUp = true;
            }

            if (moveUp)
            {
                transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
            }
            else
            {
                // Verifica se a plataforma pode se mover para baixo
                if (transform.position.y > minYPosition)
                {
                    transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
                }
            }
        }
    }
}
