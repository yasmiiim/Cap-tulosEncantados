using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingSpike : MonoBehaviour
{
    public float fallDelay = 1f;
    private Rigidbody2D rb;
    private bool playerDetected = false;
    private Vector3 initialPosition;
    public float destroyDelay = 2f;
    public LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !playerDetected)
        {
            playerDetected = true;
            Invoke("DropSpike", fallDelay);
        }
    }

    private void DropSpike()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            VidaPlayer player = collision.gameObject.GetComponent<VidaPlayer>();
            if (player != null)
            {
                player.Die();
            }
            Destroy(this.gameObject);
        }
        else if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            Invoke("DestroySpike", destroyDelay);
        }
    }

    public void ResetSpike()
    {
        transform.position = initialPosition;
        rb.bodyType = RigidbodyType2D.Kinematic;
        playerDetected = false;
    }

    private void DestroySpike()
    {
        Destroy(this.gameObject);
    }
}
