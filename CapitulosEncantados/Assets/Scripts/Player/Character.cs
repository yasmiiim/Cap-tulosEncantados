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

    // Variáveis para superpoder
    public float superSpeedMultiplier = 2f; // Multiplicador de velocidade quando o superpoder é ativado
    public float superPowerDuration = 3f;   // Duração do superpoder em segundos
    private bool isSuperPowerActive = false; // Flag para indicar se o superpoder está ativo

    private float originalSpeed; // Armazena a velocidade original

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
            Destroy(temp.gameObject, 0.5f);
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
            // Ativar o superpoder de velocidade
            StartCoroutine(ActivateSuperSpeed());
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pedra")
        {
            collectedColliders.Remove(collision);
        }
    }

    // Função para ativar o superpoder de velocidade
    private IEnumerator ActivateSuperSpeed()
    {
        // Se o superpoder não está ativo, ativa
        if (!isSuperPowerActive)
        {
            isSuperPowerActive = true;
            Speed *= superSpeedMultiplier; // Multiplica a velocidade pela variável de superpoder

            // Aguarda o tempo de duração do superpoder
            yield return new WaitForSeconds(superPowerDuration);

            // Restaura a velocidade original após a duração
            Speed = originalSpeed;
            isSuperPowerActive = false;
        }
    }
}

