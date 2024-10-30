using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cogumelo : MonoBehaviour
{
    public float bounce = 20f;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D contact in other.contacts)
            {
                if (contact.normal.y < 0)
                {
                    Rigidbody2D playerRb = other.gameObject.GetComponent<Rigidbody2D>();
                    Animator playerAnimator = other.gameObject.GetComponent<Animator>();

                    if (playerAnimator != null)
                    {
                        playerAnimator.SetBool("Pulando", true);
                    }

                    if (playerRb != null)
                    {
                        playerRb.velocity = new Vector2(playerRb.velocity.x, bounce);
                    }

                    StartCoroutine(ResetPulando(playerAnimator));
                    break;
                }
            }
        }
    }

    private IEnumerator ResetPulando(Animator playerAnimator)
    {
        yield return new WaitForSeconds(0.1f);
        playerAnimator.SetBool("Pulando", false);
    }

}
