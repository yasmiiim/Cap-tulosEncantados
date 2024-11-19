using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bolaFogo : MonoBehaviour
{
    public float velocidade = 5f;
    public float alturaMaxima = 10f;
    public float alturaMinima = 0f;
    public Sprite spriteSubindo;
    public Sprite spriteDescendo;

    private SpriteRenderer spriteRenderer;
    private bool subindo = true;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteSubindo;
    }

    private void Update()
    {
        if (subindo)
        {
            transform.Translate(Vector2.up * velocidade * Time.deltaTime);
            if (transform.position.y >= alturaMaxima)
            {
                subindo = false;
                spriteRenderer.sprite = spriteDescendo;
            }
        }
        else
        {
            transform.Translate(Vector2.down * velocidade * Time.deltaTime);
            if (transform.position.y <= alturaMinima)
            {
                Destroy(gameObject);
            }
        }
    }
}
