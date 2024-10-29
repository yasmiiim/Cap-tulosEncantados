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

    public GameObject balaprojetil;
    public Transform arma;
    public Transform groundCheck;
    private bool tiro;
    public float forcaDoTiro;
    private bool flipX = false;

    public float jumpForce;
    public bool pulo, isgrounded;
    private bool canDoubleJump;
    public float checkRadius = 0.1f;
    public LayerMask whatIsGround;

    public float Speed;

    private Rigidbody2D rig;
    private AudioSource soundFx;

    public int pedras;
    public score scoreManager;

    private HashSet<Collider2D> collectedColliders = new HashSet<Collider2D>();

    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool isDashing = false;
    private bool canDash = true;
    private float originalSpeed;

    private bool doubleShot = false;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        originalSpeed = Speed;
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        soundFx = GetComponent<AudioSource>();
    }

    void Update()
    {
        pulo = Input.GetButtonDown("Jump");
        isgrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        animator.SetBool(saltandoHash, !isgrounded);

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
                animator.SetTrigger(saltandoHash);
            }
        }

        if (isgrounded)
        {
            animator.SetBool(doubleJumpHash, false);
            animator.SetBool(saltandoHash, false);
        }

        if (!isDashing)
        {
            Move();
        }

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

        float inputAxis = Input.GetAxis("Horizontal");
        rig.velocity = new Vector2(inputAxis * Speed, rig.velocity.y);
        animator.SetBool(movendoHash, inputAxis != 0);

        if (inputAxis > 0 && flipX)
        {
            Flip();
        }
        else if (inputAxis < 0 && !flipX)
        {
            Flip();
        }
    }

    void Jump()
    {
        rig.velocity = new Vector2(rig.velocity.x, jumpForce);
        isgrounded = false;
        AudioObserver.OnPlaySfxEvent("pulo");
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
        animator.SetBool(poderHash, true);

        yield return new WaitForSeconds(3f);

        Speed /= 1.4f;
        doubleShot = false;
        animator.SetBool(poderHash, false);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("chao"))
        {
            isgrounded = false;
        }
    }
}

