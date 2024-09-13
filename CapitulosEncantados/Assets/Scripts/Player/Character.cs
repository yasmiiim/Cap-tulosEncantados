using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


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
    
    public int pedras;
    public Text ScoreTxt;
    private int score;
    

    void Start()
    {
        score = 0;
        rig = GetComponent<Rigidbody2D>();
        jumpCount = maxJumpCount;
    }
//FAZER ESSE DEPOIS
    //private void OnCollisionEnter(Collision2D collision)
    //{
        //if (collision.gameObject.layer == 6)
        //{
           // playerAnim.SetBool("Jump", false);
           // isGrounded = true;
           // doubleJump = false;
      //  }
   // }

    void Awake()
    {
        soundFx = GetComponent<AudioSource>();
    }

    void Update()
    {
        ScoreTxt.text = score.ToString();
        Move();
        Jump();

        tiro = Input.GetKeyDown(KeyCode.Z);

        Atirar();
    }

    void Move()
    {
        
        AudioObserver.OnPlaySfxEvent("walking");
        
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime * Speed;

        float inputAxis = Input.GetAxis("Horizontal");

        if (flipX == true && inputAxis > 0)
        {
            Flip();
        }
        else if (flipX == false && inputAxis < 0)
        {
            Flip();
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            rig.AddForce(new Vector2(0f, Jumpforce), ForceMode2D.Impulse);
            jumpCount--;
            AudioObserver.OnPlaySfxEvent("pulo");
        }
    }

    private void Atirar()
    {
        if (tiro)
        {
            AudioObserver.OnPlaySfxEvent("attack");
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pedra")
        {
            AudioObserver.OnPlaySfxEvent("coletar");
            Destroy(collision.gameObject);
            score = score + 1;
        }
    }
}

