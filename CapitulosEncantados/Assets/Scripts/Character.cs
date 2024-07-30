using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Character : MonoBehaviour
{
    public GameObject balaprojetil;
    public Transform arma;
    private bool tiro;
    public float forcaDoTiro;
    private bool flipX = false;

    public float Speed;
    public float Jumpforce;
    public int maxJumpCount = 2;

    public int jumpCount;
    public bool isGrounded;

    private Rigidbody2D rig;

    private AudioSource soundFx;




    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        jumpCount = maxJumpCount; // Inicializa o número de saltos
    }


    void Awake()
    {
        soundFx = GetComponent<AudioSource>();

    }

    void Update()
    {
        Move();
        Jump();

        tiro = Input.GetButtonDown("Fire1");

        Atirar();
    }

    void Move()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime * Speed;

        float inputAxis = Input.GetAxis("Horizontal");

        if (flipX == true && inputAxis > 0)
        {
            Flip();
            //transform.eulerAngles = new Vector2(0f, 0f);
        }
        else if (flipX == false && inputAxis < 0)
        {
            Flip();
            //transform.eulerAngles = new Vector2(0f, 180f);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            //soundFx.Play();
            rig.AddForce(new Vector2(0f, Jumpforce), ForceMode2D.Impulse);
            jumpCount--; // Decrementa o contador de saltos
        }
    }

    private void Atirar()
    {
        if (tiro == true)
        {
            GameObject temp = Instantiate(balaprojetil);
            temp.transform.position = arma.position;
            temp.GetComponent<Rigidbody2D>().velocity = new Vector2(forcaDoTiro, 0);
            Destroy(temp.gameObject, 5f);
        }
    }

    private void Flip()
    {
        flipX = !flipX;
        float x = transform.localScale.x;
        x *= -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        forcaDoTiro *= -1;
    }

}
