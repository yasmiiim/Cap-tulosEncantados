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
    public score scoreManager;

    private HashSet<Collider2D> collectedColliders = new HashSet<Collider2D>();

    // Dash variables
    public float dashSpeed = 15f;       // Velocidade do dash
    public float dashDuration = 0.2f;   // Duração do dash em segundos
    public float dashCooldown = 1f;     // Tempo de recarga do dash
    private bool isDashing = false;     // Se o player está atualmente dando dash
    private bool canDash = true;        // Se o player pode dar dash no momento

    private float originalSpeed;        // Para armazenar a velocidade original

    // Novas variáveis para super velocidade e dois tiros
    private bool doubleShot = false; // Indica se o jogador tem dois tiros

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        jumpCount = maxJumpCount;
        originalSpeed = Speed; // Armazena a velocidade original no início do jogo
    }

    void Awake()
    {
        soundFx = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Mover e pular somente se o player não está dando dash
        if (!isDashing)
        {
            Move();
            Jump();
        }

        // Ativar dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

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

    // Método para atirar com dois tiros
    private void Atirar()
    {
        if (tiro)
        {
            AudioObserver.OnPlaySfxEvent("attack");
            GameObject temp = Instantiate(balaprojetil);
            temp.transform.position = arma.position;
            temp.GetComponent<Rigidbody2D>().velocity = new Vector2(forcaDoTiro, 0);
            Destroy(temp.gameObject, 0.5f);

            // Se o jogador tiver dois tiros, disparar um segundo projétil
            if (doubleShot)
            {
                GameObject temp2 = Instantiate(balaprojetil);
                temp2.transform.position = arma.position + new Vector3(0, 0.5f, 0); // Ajuste a posição vertical do segundo tiro
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

    // Função do Dash
    private IEnumerator Dash()
    {
        canDash = false;   // O player não pode dar dash enquanto estiver recarregando
        isDashing = true;  // O player está atualmente dando dash

        float dashDirection = Input.GetAxisRaw("Horizontal"); // Pegando a direção do movimento
        if (dashDirection == 0) dashDirection = flipX ? -1 : 1; // Se não houver movimento, usar a direção do flip

        Vector2 dashVelocity = new Vector2(dashDirection * dashSpeed, rig.velocity.y); // Velocidade do dash
        rig.velocity = dashVelocity; // Aplicar a velocidade de dash

        yield return new WaitForSeconds(dashDuration); // Aguardar a duração do dash

        // Parar o movimento horizontal após o dash
        rig.velocity = new Vector2(0, rig.velocity.y);

        isDashing = false; // Finalizar o dash

        yield return new WaitForSeconds(dashCooldown); // Aguardar o tempo de recarga

        canDash = true; // Permitir que o player possa dar dash novamente
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificar se o objeto coletado é uma pedra
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

        // Verificar se o objeto coletado é uma poção
        if (collision.gameObject.tag == "Pocao")
        {
            // Ativar o superpoder de velocidade e dois tiros
            StartCoroutine(ActivateSuperSpeedAndDoubleShot());
            Destroy(collision.gameObject);
        }
    }

    // Método para ativar super velocidade e dois tiros temporariamente
    private IEnumerator ActivateSuperSpeedAndDoubleShot()
    {
        Speed *= 2; // Multiplica a velocidade
        doubleShot = true; // Ativa o modo de dois tiros

        yield return new WaitForSeconds(3f); // Aguarda 3 segundos

        Speed /= 2; // Retorna à velocidade original
        doubleShot = false; // Desativa o modo de dois tiros
    }
}

