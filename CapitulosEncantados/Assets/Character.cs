using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour
{
    public float Speed;
    public float Jumpforce;
    public int maxJumpCount = 2; // Número máximo de saltos (double jump é 2 saltos)

    public int jumpCount; // Contador de saltos restantes
    public bool isGrounded; // Verifica se o personagem está no chão

    private Rigidbody2D rig;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        jumpCount = maxJumpCount; // Inicializa o número de saltos
    }

    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime * Speed;

        float inputAxis = Input.GetAxis("Horizontal");

        if (inputAxis > 0)
        {
            transform.eulerAngles = new Vector2(0f, 0f);
        }
        else if (inputAxis < 0)
        {
            transform.eulerAngles = new Vector2(0f, 180f);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            rig.AddForce(new Vector2(0f, Jumpforce), ForceMode2D.Impulse);
            jumpCount--; // Decrementa o contador de saltos
        }
    }
}
