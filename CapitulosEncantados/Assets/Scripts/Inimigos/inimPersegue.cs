using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inimPersegue : MonoBehaviour
{
   public float distanciaDePerseguicao = 3f;
    public float velocidade = 2f;
    public Transform jogador;
    public int vida = 3;
    public Color corNormal = Color.white;
    public Color corDano = Color.red;
    public float tempoDano = 0.1f;

    private SpriteRenderer spriteRenderer;
    private bool estaPerseguindo = false;
    public Animator animator;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distanciaParaJogador = Vector2.Distance(new Vector2(transform.position.x, 0), new Vector2(jogador.position.x, 0));

        if (distanciaParaJogador < distanciaDePerseguicao)
        {
            estaPerseguindo = true;
            animator.SetBool("isWalking", true);
        }
        else
        {
            estaPerseguindo = false;
            animator.SetBool("isWalking", false);
        }

        if (estaPerseguindo)
        {
            Vector2 direcao = new Vector2(jogador.position.x - transform.position.x, 0).normalized;
            transform.position = new Vector2(
                Mathf.MoveTowards(transform.position.x, jogador.position.x, velocidade * Time.deltaTime),
                transform.position.y
            );

            spriteRenderer.flipX = jogador.position.x > transform.position.x;
        }
    }

    private void OnTriggerEnter2D(Collider2D colisao)
    {
        if (colisao.gameObject.CompareTag("bala"))
        {
            ReceberDano(1);
            Destroy(colisao.gameObject);
        }
    }

    public void ReceberDano(int dano)
    {
        vida -= dano;
        StartCoroutine(EfeitoVisualDano());

        if (vida <= 0)
        {
            Morrer();
        }
    }

    void Morrer()
    {
        Destroy(gameObject);
    }

    private System.Collections.IEnumerator EfeitoVisualDano()
    {
        spriteRenderer.color = corDano;
        yield return new WaitForSeconds(tempoDano);
        spriteRenderer.color = corNormal;
    }
}
