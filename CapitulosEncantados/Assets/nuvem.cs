using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nuvem : MonoBehaviour
{
    public float disappearDelay = 0.5f; // Tempo até a nuvem desaparecer
    private bool playerOnCloud = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o jogador colidiu com a nuvem
        if (collision.gameObject.tag == "Player" && !playerOnCloud)
        {
            playerOnCloud = true;
            StartCoroutine(DisappearAfterDelay());
        }
    }

    private IEnumerator DisappearAfterDelay()
    {
        // Aguarda o tempo especificado
        yield return new WaitForSeconds(disappearDelay);

        // Desativa a nuvem
        gameObject.SetActive(false);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Caso o jogador saia da nuvem antes de 2 segundos, o processo é cancelado
        if (collision.gameObject.tag == "Player")
        {
            playerOnCloud = false;
            StopCoroutine(DisappearAfterDelay());
        }
    }
}
