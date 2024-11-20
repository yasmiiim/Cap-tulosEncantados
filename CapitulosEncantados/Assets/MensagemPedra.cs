using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MensagemPedra : MonoBehaviour
{
    public GameObject mensagemUI; // Referência ao GameObject de texto
    public float duracaoMensagem = 2f; // Duração em segundos

    public void MostrarMensagem()
    {
        mensagemUI.SetActive(true); // Mostra a mensagem
        Invoke("OcultarMensagem", duracaoMensagem); // Oculta após um tempo
    }

    private void OcultarMensagem()
    {
        mensagemUI.SetActive(false); // Oculta a mensagem
    }
}
