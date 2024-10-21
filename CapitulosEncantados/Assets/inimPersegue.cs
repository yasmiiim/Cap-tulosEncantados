using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inimPersegue : MonoBehaviour
{
     public float distanciaDePerseguicao = 5f;  // Distância em que o inimigo começa a perseguir o jogador
    public float velocidade = 2f;              // Velocidade de movimento do inimigo
    public Transform jogador;                  // Referência ao Transform do jogador
    public int vida = 3;                       // Vida do inimigo
    public Color corNormal = Color.white;      // Cor normal do inimigo
    public Color corDano = Color.red;          // Cor do inimigo ao receber dano
    public float tempoDano = 0.1f;             // Tempo que o inimigo fica vermelho após levar dano

    private SpriteRenderer spriteRenderer;     // Referência ao SpriteRenderer do inimigo
    private bool estaPerseguindo = false;      // Verifica se o inimigo está perseguindo

    void Start()
    {
        // Obtém o componente SpriteRenderer para mudar a cor do inimigo
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Calcula a distância horizontal entre o inimigo e o jogador
        float distanciaParaJogador = Vector2.Distance(new Vector2(transform.position.x, 0), new Vector2(jogador.position.x, 0));

        // Verifica se o jogador está dentro da distância de perseguição
        if (distanciaParaJogador < distanciaDePerseguicao)
        {
            estaPerseguindo = true;
        }
        else
        {
            estaPerseguindo = false;
        }

        // Se estiver perseguindo, move o inimigo em direção ao jogador apenas no eixo X (horizontal)
        if (estaPerseguindo)
        {
            Vector2 direcao = new Vector2(jogador.position.x - transform.position.x, 0).normalized;
            transform.position = new Vector2(
                Mathf.MoveTowards(transform.position.x, jogador.position.x, velocidade * Time.deltaTime),
                transform.position.y
            );
        }
    }

    // Método chamado quando o inimigo colide com um objeto
    private void OnTriggerEnter2D(Collider2D colisao)
    {
        if (colisao.gameObject.CompareTag("bala"))  // Verifica se o objeto que colidiu tem a tag "Bala"
        {
            ReceberDano(1);  // Inimigo recebe 1 de dano
            Destroy(colisao.gameObject);  // Destrói a bala após o impacto
        }
    }

    // Função para o inimigo receber dano
    public void ReceberDano(int dano)
    {
        vida -= dano;

        // Aplica o efeito visual de dano (ficar vermelho)
        StartCoroutine(EfeitoVisualDano());

        if (vida <= 0)
        {
            Morrer();
        }
    }

    // Função para destruir o inimigo quando a vida for menor ou igual a zero
    void Morrer()
    {
        Destroy(gameObject);  // Destroi o inimigo
    }

    // Coroutine para mudar a cor do inimigo quando ele levar dano
    private System.Collections.IEnumerator EfeitoVisualDano()
    {
        spriteRenderer.color = corDano;  // Muda a cor para vermelho
        yield return new WaitForSeconds(tempoDano);  // Espera o tempo definido
        spriteRenderer.color = corNormal;  // Retorna à cor original
    }
}
