using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Character : MonoBehaviour
{
 private Animator animator;
    private int movendoHash = Animator.StringToHash("Movendo");
    private int saltandoHash = Animator.StringToHash("Pulando");
    private int doubleJumpHash = Animator.StringToHash("DoubleJump");
    private int poderHash = Animator.StringToHash("Poder");
    private int caindoHash = Animator.StringToHash("isCaindo");
    private int isAttackingHash = Animator.StringToHash("isAttacking"); // Novo parâmetro

    public GameObject balaprojetil;
    public Transform arma;
    public Transform groundCheck;
    public Transform wallCheck;
    private bool tiro;
    public float forcaDoTiro;

    public float jumpForce;
    public bool pulo, isgrounded;
    private bool canDoubleJump;
    public float checkRadius = 0.1f;
    public LayerMask whatIsGround;

    public float Speed;

    private Rigidbody2D rig;
    private bool flipX = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        pulo = Input.GetButtonDown("Jump");
        isgrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        animator.SetBool(saltandoHash, !isgrounded);

        // Atualiza a animação de queda
        if (!isgrounded && rig.velocity.y < 0)
        {
            animator.SetBool(caindoHash, true);
        }
        else
        {
            animator.SetBool(caindoHash, false);
        }

        if (pulo)
        {
            if (isgrounded)
            {
                Jump();
                canDoubleJump = true;
                animator.SetBool(saltandoHash, true);
            }
            else if (canDoubleJump)
            {
                Jump();
                canDoubleJump = false;
                animator.SetBool(doubleJumpHash, true);
            }
        }

        Move();

        // Controle de ataque
        tiro = Input.GetKeyDown(KeyCode.Z);
        if (tiro)
        {
            StartCoroutine(Attack());
        }
    }

    private void Move()
    {
        float inputAxis = Input.GetAxis("Horizontal");
        rig.velocity = new Vector2(inputAxis * Speed, rig.velocity.y);

        bool isWalking = inputAxis != 0;
        animator.SetBool(movendoHash, isWalking);

        // Lógica para inverter a sprite
        if (inputAxis > 0 && flipX)
        {
            Flip();
        }
        else if (inputAxis < 0 && !flipX)
        {
            Flip();
        }
    }

    private void Jump()
    {
        rig.velocity = new Vector2(rig.velocity.x, jumpForce);
        isgrounded = false;
    }

    private IEnumerator Attack()
    {
        animator.SetBool(isAttackingHash, true); // Ativa o estado de ataque
        Atirar();

        // Duração do ataque
        yield return new WaitForSeconds(0.5f);

        animator.SetBool(isAttackingHash, false); // Volta ao estado normal
    }

    private void Atirar()
    {
        GameObject temp = Instantiate(balaprojetil);
        temp.transform.position = arma.position;
        temp.GetComponent<Rigidbody2D>().velocity = new Vector2(forcaDoTiro, 0);
        Destroy(temp.gameObject, 0.2f);
    }

    private void Flip()
    {
        flipX = !flipX;
        float x = transform.localScale.x;
        x *= -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        forcaDoTiro *= -1;
    }

    private void OnDrawGizmosSelected()
    {
        // Gizmo para visualizar a área de verificação do chão
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
}

