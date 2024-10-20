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

    // pulo e pulo duplo
    public float jumpForce;
    public bool pulo, isgrounded;
    private bool canDoubleJump; // Verifica se o pulo duplo está disponível

    public float Speed;

    private Rigidbody2D rig;
    private AudioSource soundFx;

    public int pedras;
    public score scoreManager;

    private HashSet<Collider2D> collectedColliders = new HashSet<Collider2D>();

    // Dash variables
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool isDashing = false;
    private bool canDash = true;
    private float originalSpeed;

    // Novas variáveis para super velocidade e dois tiros
    private bool doubleShot = false;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        originalSpeed = Speed;
    }

    void Awake()
    {
        soundFx = GetComponent<AudioSource>();
    }

    void Update()
    {
        pulo = Input.GetButtonDown("Jump");
        
        // Verifica se o jogador pode pular (pulo simples ou duplo)
        if (pulo)
        {
            if (isgrounded)
            {
                Jump(); // Pulo normal
                canDoubleJump = true; // Habilita o pulo duplo
            }
            else if (canDoubleJump)
            {
                Jump(); // Pulo duplo
                canDoubleJump = false; // Desabilita após o pulo duplo
            }
        }

        // Movimentos e ações durante o dash
        if (!isDashing)
        {
            Move();
        }

        // Ativar dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        tiro = Input.GetKeyDown(KeyCode.Z);
        Atirar();
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("chao"))
        {
            isgrounded = true;
            canDoubleJump = false; // Reseta o pulo duplo ao tocar o chão
        }
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

    // Método para pular
    void Jump()
    {
        rig.velocity = new Vector2(rig.velocity.x, jumpForce);
        isgrounded = false;
        AudioObserver.OnPlaySfxEvent("jump"); // Som de pulo, caso tenha um evento de áudio
    }

    private void Atirar()
    {
        if (tiro)
        {
            AudioObserver.OnPlaySfxEvent("attack");
            GameObject temp = Instantiate(balaprojetil);
            temp.transform.position = arma.position;
            temp.GetComponent<Rigidbody2D>().velocity = new Vector2(forcaDoTiro, 0);
            Destroy(temp.gameObject, 0.5f);

            if (doubleShot)
            {
                GameObject temp2 = Instantiate(balaprojetil);
                temp2.transform.position = arma.position + new Vector3(0, 0.5f, 0);
                temp2.GetComponent<Rigidbody2D>().velocity = new Vector2(forcaDoTiro, 0);
                Destroy(temp2.gameObject, 0.5f);
            }
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

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        float dashDirection = Input.GetAxisRaw("Horizontal");
        if (dashDirection == 0) dashDirection = flipX ? -1 : 1;

        Vector2 dashVelocity = new Vector2(dashDirection * dashSpeed, rig.velocity.y);
        rig.velocity = dashVelocity;

        yield return new WaitForSeconds(dashDuration);

        rig.velocity = new Vector2(0, rig.velocity.y);

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pedra" && !collectedColliders.Contains(collision))
        {
            AudioObserver.OnPlaySfxEvent("coletar");
            Destroy(collision.gameObject);

            if (scoreManager != null)
            {
                scoreManager.AddScore(1);
            }

            collectedColliders.Add(collision);
        }

        if (collision.gameObject.tag == "Pocao")
        {
            StartCoroutine(ActivateSuperSpeedAndDoubleShot());
            Destroy(collision.gameObject);
        }
    }

    private IEnumerator ActivateSuperSpeedAndDoubleShot()
    {
        Speed *= 1.4f;
        doubleShot = true;

        yield return new WaitForSeconds(3f);

        Speed /= 2;
        doubleShot = false;
    }
}

