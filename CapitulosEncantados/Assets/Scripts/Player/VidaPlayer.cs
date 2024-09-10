using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VidaPlayer : MonoBehaviour
{
    public int vidaMaxima;
    public int vidaAtual;

    public static event Action OnPlayerDeath; // Evento que será chamado ao morrer

    void Start()
    {
        vidaAtual = vidaMaxima;
    }

    public void ReceberDano()
    {
        vidaAtual -= 1;

        if (vidaAtual <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
        Debug.Log("Player has died. Triggering death event.");
        OnPlayerDeath?.Invoke(); // Aciona o evento de morte
    }

    private void OnCollisionEnter2D(Collision2D coli)
    {
        // Verifica se o objeto colidido tem a tag "enemy" e aplica dano
        if (coli.collider.CompareTag("enemy"))
        {
            ReceberDano();
        }
        // Verifica se o objeto colidido tem a tag "obstaculo" e aplica dano
        else if (coli.collider.CompareTag("obstaculo"))
        {
            ReceberDano();
        }
    }
    

}
