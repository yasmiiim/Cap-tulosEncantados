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
    private int isSubindoHash = Animator.StringToHash("isSubindo");
    private int isAttackingHash = Animator.StringToHash("isAttacking");
    private int isAttackingPoderHash = Animator.StringToHash("isAttackingPoder");
    private int caindoPoderHash = Animator.StringToHash("caindoPoder");
    private int idlePoderHash = Animator.StringToHash("idlePoder");
    private int isSlidingHash = Animator.StringToHash("isSliding");

    public GameObject balaprojetil;
    public Transform arma;
    public Transform groundCheck;
    public Transform wallCheck;
    private bool tiro;
    public float forcaDoTiro;
    private bool flipX = false;

    public float jumpForce;
    public bool pulo, isgrounded;
    private bool canDoubleJump;
    public float checkRadius = 0.1f;
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;

    public float Speed;

    private Rigidbody2D rig;
    private AudioSource soundFx;

    public int pedras;
    public score scoreManager;

    private HashSet<Collider2D> collectedColliders = new HashSet<Collider2D>();

    
    
    
    private float stepDistance = 1f;
    private float distanceTravelled = 0f; 
    private Vector2 lastPosition; 
    
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool isDashing = false;
    private bool canDash = true;
    private float originalSpeed;

    private bool doubleShot = false;

    private bool isTouchingWall;
    private bool isWallSliding;
    public float wallSlideSpeed = 2f;
    private bool canWallJump = false;

    private bool canAttack = true;

    public portal portalScript;
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        originalSpeed = Speed;
        
        lastPosition = transform.position;
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
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, checkRadius, whatIsWall);
        animator.SetBool(saltandoHash, !isgrounded);

        if (!isgrounded && rig.velocity.y < 0 && !isWallSliding)
        {
            animator.SetBool(caindoHash, true);

            if (animator.GetBool(poderHash))
            {
                animator.SetBool(caindoPoderHash, true);
            }
        }
        else
        {
            animator.SetBool(caindoHash, false);
            animator.SetBool(caindoPoderHash, false);
        }

        if (isgrounded && !Input.GetButton("Horizontal") && animator.GetBool(poderHash))
        {
            animator.SetBool(idlePoderHash, true);
        }
        else
        {
            animator.SetBool(idlePoderHash, false);
        }

        if (pulo)
        {
            if (isgrounded)
            {
                Jump();
                canDoubleJump = true;
                animator.SetBool(saltandoHash, true);
            }
            else if (isTouchingWall && !isgrounded)
            {
                WallJump();
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

        if (isTouchingWall && !isgrounded && rig.velocity.y < 0)
        {
            isWallSliding = true;
            WallSlide();
            animator.SetBool(isSlidingHash, true); // Ativa a animação de deslizar
            animator.SetBool(caindoHash, false);  // Garante que "caindo" seja desativado
          
            if ((transform.localScale.x > 0 && !flipX) || (transform.localScale.x < 0 && flipX))
            {
                Flip(); // Gira o sprite para olhar corretamente para a parede
            }
        }
        else
        {
            isWallSliding = false;
            animator.SetBool(isSlidingHash, false);
        }

        if (isgrounded)
        {
            animator.SetBool(doubleJumpHash, false);
            animator.SetBool(saltandoHash, false);
        }

        if (!isDashing)
        {
            Move();
            HandleFootsteps();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.Z) && canAttack)
        {
            StartCoroutine(Attack());
        }
    }
    
    private void HandleFootsteps()
    {
        // Verifica se a animação de movimento está ativa antes de calcular a distância percorrida
        if (!animator.GetBool(movendoHash)) 
        {
            distanceTravelled = 0f; // Reseta a distância acumulada se não está andando
            return;
        }

        // Calcula a distância percorrida desde o último frame
        Vector2 currentPosition = transform.position;
        float frameDistance = Vector2.Distance(currentPosition, lastPosition);

        if (frameDistance > 0 && isgrounded) // Apenas conta quando está no chão e em movimento
        {
            distanceTravelled += frameDistance;

            if (distanceTravelled >= stepDistance)
            {
                AudioObserver.OnPlaySfxEvent("walking"); // Toca o som do passo
                distanceTravelled = 0f; // Reseta a distância acumulada
            }
        }

        lastPosition = currentPosition; // Atualiza a última posição
    }

    private IEnumerator Attack()
    {
        canAttack = false;

        // Verifica se está no estado de poder e ativa a animação correspondente
        if (animator.GetBool(poderHash))
        {
            animator.SetBool(isAttackingPoderHash, true); // Ativa animação de ataque com poder
        }
        else
        {
            animator.SetBool(isAttackingHash, true); // Ativa animação de ataque normal
        }

        Atirar(); // Executa o ataque

        // Espera até que a duração da animação termine
        yield return new WaitForSeconds(GetAttackAnimationDuration());

        // Reseta os estados de ataque
        animator.SetBool(isAttackingHash, false);
        animator.SetBool(isAttackingPoderHash, false);
        canAttack = true; // Permite atacar novamente
    }

    private void Move()
    {

        float inputAxis = Input.GetAxis("Horizontal");
        rig.velocity = new Vector2(inputAxis * Speed, rig.velocity.y);

        bool isWalking = inputAxis != 0;
        bool isJumping = !isgrounded || (isTouchingWall && rig.velocity.y < 0);

        if (animator.GetBool(poderHash))
        {
            if (isJumping)
            {
                animator.SetBool("WalkPower", false);
                animator.SetBool("JumpPower", true);
            }
            else
            {
                animator.SetBool("WalkPower", isWalking);
                animator.SetBool("JumpPower", false);
            }
        }
        else
        {
            animator.SetBool(movendoHash, isWalking);
            animator.SetBool(saltandoHash, isJumping);
        }

        if (isgrounded && !isJumping && animator.GetBool(poderHash))
        {
            animator.SetBool("WalkPower", isWalking);
            animator.SetBool("JumpPower", false);
        }

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
        AudioObserver.OnPlaySfxEvent("pulo");
    }

    private void WallSlide()
    {
        rig.velocity = new Vector2(rig.velocity.x, -wallSlideSpeed);
        canWallJump = true;
    }

    private void WallJump()
    {
        if (canWallJump)
        {
            rig.velocity = new Vector2(flipX ? jumpForce : -jumpForce, jumpForce);
            canWallJump = false;
            animator.SetBool(isSlidingHash, false);
        }
    }

    private void Atirar()
    {
        AudioObserver.OnPlaySfxEvent("attack");
        GameObject temp = Instantiate(balaprojetil);
        temp.transform.position = arma.position;
        temp.GetComponent<Rigidbody2D>().velocity = new Vector2(forcaDoTiro, 0);
        Destroy(temp.gameObject, 0.2f);

        if (doubleShot)
        {
            GameObject temp2 = Instantiate(balaprojetil);
            temp2.transform.position = arma.position + new Vector3(0, 0.5f, 0);
            temp2.GetComponent<Rigidbody2D>().velocity = new Vector2(forcaDoTiro, 0);
            Destroy(temp2.gameObject, 0.2f);
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

    private float GetAttackAnimationDuration()
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "ataqueplayer")
            {
                return clip.length;
            }
        }

        return 0.5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pedra" && !collectedColliders.Contains(collision))
        {
            AudioObserver.OnPlaySfxEvent("coletar");
            Destroy(collision.gameObject);
            portalScript.hasStone = true; 

            if (scoreManager != null)
            {
                scoreManager.AddScore(1);
            }

            collectedColliders.Add(collision);
        }

        if (collision.gameObject.tag == "Pocao")
        {
            AudioObserver.OnPlaySfxEvent("coletar");
            StartCoroutine(ActivateSuperSpeedAndDoubleShot());
            Destroy(collision.gameObject);
        }
    }

    private IEnumerator ActivateSuperSpeedAndDoubleShot()
    {
        Speed *= 1.4f;
        doubleShot = true;
        animator.SetBool(poderHash, true);

        if (isgrounded)
        {
            animator.SetBool(idlePoderHash, true);
        }

        yield return new WaitForSeconds(3f);

        Speed /= 1.4f;
        doubleShot = false;
        animator.SetBool(poderHash, false);
        animator.SetBool(caindoPoderHash, false);
        animator.SetBool(idlePoderHash, false);
    }
}

