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
    public GameObject deathParticlesPrefab;

    // Variáveis para reset
    private Vector3 posicaoInicial;
    private bool estaVivo = true; // Verifica se o inimigo está vivo

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Salva a posição inicial
        posicaoInicial = transform.position;
    }

    void Update()
    {
        if (!estaVivo) return; // Não faz nada se o inimigo estiver morto

        // Calcula a distância horizontal até o jogador
        float distanciaParaJogador = Vector2.Distance(new Vector2(transform.position.x, 0), new Vector2(jogador.position.x, 0));

        // Define se deve perseguir ou não
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

        // Movimenta o inimigo se estiver perseguindo
        if (estaPerseguindo)
        {
            Vector2 direcao = new Vector2(jogador.position.x - transform.position.x, 0).normalized;
            transform.position = new Vector2(
                Mathf.MoveTowards(transform.position.x, jogador.position.x, velocidade * Time.deltaTime),
                transform.position.y
            );

            // Ajusta o flip para o lado do jogador
            spriteRenderer.flipX = jogador.position.x > transform.position.x;
        }
    }

    private void OnTriggerEnter2D(Collider2D colisao)
    {
        // Detecta se foi atingido por uma bala
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
        estaVivo = false; // Marca o inimigo como morto

        // Gera partículas de morte, se configurado
        if (deathParticlesPrefab != null)
        {
            Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
        }

        gameObject.SetActive(false); // Desativa o inimigo
    }

    private IEnumerator EfeitoVisualDano()
    {
        // Muda a cor para indicar dano
        spriteRenderer.color = corDano;
        yield return new WaitForSeconds(tempoDano);
        spriteRenderer.color = corNormal;
    }

    // Método para resetar o inimigo
    public void Resetar()
    {
        if (!estaVivo) return; // Não reseta inimigos mortos
        transform.position = posicaoInicial; // Volta para a posição inicial
        estaPerseguindo = false; // Para de perseguir
        animator.SetBool("isWalking", false); // Reseta a animação
    }
}
