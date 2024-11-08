using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nuvem : MonoBehaviour
{
    public float disappearDelay = 0.5f; // Tempo até a nuvem começar a desaparecer
    public float fadeDuration = 1f; // Tempo para a nuvem desaparecer completamente

    private bool playerOnCloud = false;
    private SpriteRenderer spriteRenderer;
    private Color initialColor;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialColor = spriteRenderer.color;
    }

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
        // Aguarda o tempo especificado antes de começar a desaparecer
        yield return new WaitForSeconds(disappearDelay);

        // Desaparecer gradualmente
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }

        // Desativa a nuvem após desaparecer completamente
        gameObject.SetActive(false);
    }

    // Método para reiniciar a nuvem
    public void ResetCloud()
    {
        // Reativa a nuvem e restaura a opacidade
        gameObject.SetActive(true);
        spriteRenderer.color = initialColor;
        playerOnCloud = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Não cancela o desaparecimento caso o jogador saia da nuvem
        if (collision.gameObject.tag == "Player")
        {
            playerOnCloud = false;
        }
    }
}
