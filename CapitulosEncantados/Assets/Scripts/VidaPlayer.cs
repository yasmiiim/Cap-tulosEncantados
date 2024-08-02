using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaPlayer : MonoBehaviour
{
    public int vidaMaxima;
    public int vidaAtual;
   
    void Start()
    {
        vidaAtual = vidaMaxima;
    }

   
    void Update()
    {
        
    }

    public void ReceberDano()
    {
        vidaAtual -= 1;

        if (vidaAtual <= 0)
        {
            Debug.Log("Game Over");
        }
        
    }
}
