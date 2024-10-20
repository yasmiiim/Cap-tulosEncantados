using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerAbelha : MonoBehaviour
{
    public GameObject[] bees; // Array que vai conter as abelhas na cena

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o jogador entrou no gatilho
        if (collision.gameObject.tag == "Player")
        {
            // Para cada abelha no array, ativar o movimento
            foreach (GameObject bee in bees)
            {
                bee.GetComponent<abelha>().StartMoving();
            }
        }
    }
}
