using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nuvem : MonoBehaviour
{
    public float disappearDelay = 0.5f;
    public float fadeDuration = 1f;

    private bool playerOnCloud = false;
    private SpriteRenderer spriteRenderer;
    private Color initialColor;
    private Animator animator;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialColor = spriteRenderer.color;
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !playerOnCloud)
        {
            playerOnCloud = true;
            if (animator != null)
            {
                animator.SetBool("isBlinding", true);
            }
            StartCoroutine(DisappearAfterDelay());
        }
    }

    private IEnumerator DisappearAfterDelay()
    {
        yield return new WaitForSeconds(disappearDelay);

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }

        gameObject.SetActive(false);
    }

    public void ResetCloud()
    {
        gameObject.SetActive(true);
        spriteRenderer.color = initialColor;
        playerOnCloud = false;
        if (animator != null)
        {
            animator.SetBool("isBlinding", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (animator != null)
            {
                animator.SetBool("isBlinding", false);
            }
            playerOnCloud = false;
        }
    }
}
