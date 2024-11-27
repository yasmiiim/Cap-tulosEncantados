using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuparal : MonoBehaviour
{
    public Transform cameraTransform; // Referência à câmera principal
    public float paralaxeMultiplier; // Intensidade do efeito de paralaxe
    public float movimentoAutomatico = 1f; // Velocidade do movimento automático
    private float textureUnitSizeX; // Tamanho da textura em X (para looping)

    void Start()
    {
        // Calcula o tamanho da textura em X para criar um loop perfeito
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }

    void Update()
    {
        // Movimento automático constante para a direita
        transform.position += new Vector3(movimentoAutomatico * Time.deltaTime, 0, 0);

        // Verifica se precisamos fazer o looping da textura
        if (Mathf.Abs(transform.position.x - cameraTransform.position.x) >= textureUnitSizeX)
        {
            float offsetPositionX = (transform.position.x - cameraTransform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y, transform.position.z);
        }
    }
}
