using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Grounded : MonoBehaviour
{
    private Character player; 

    void Start()
    {
        player = gameObject.transform.parent.gameObject.GetComponent<Character>();
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        // Verifica se o personagem está colidindo com o chão (camada 8)
        if (collider.gameObject.layer == 8) 
        {
            player.isGrounded = true; // O personagem está no chão
            player.jumpCount = player.maxJumpCount; // Restaura o número de saltos
        }
    }

    void OnCollisionExit2D(Collision2D collider)
    {
        // Verifica se o personagem saiu do chão (camada 8)
        if (collider.gameObject.layer == 8) 
        {
            player.isGrounded = false; // O personagem saiu do chão
        }
    }
}

