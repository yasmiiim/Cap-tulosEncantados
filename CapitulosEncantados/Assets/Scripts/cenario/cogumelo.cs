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
                    other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
                    
                    animator.SetBool("isJumping", true);

                    StartCoroutine(ResetIsJumping());
                    break;
                }
            }
        }
    }

    private IEnumerator ResetIsJumping()
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("isJumping", false);
    }
}
