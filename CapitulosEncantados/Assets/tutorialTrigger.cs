using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  
using UnityEngine.UI; 
public class TutorialTrigger : MonoBehaviour
{
    public string tutorialMessage;  // Mensagem que será exibida
    public TextMeshProUGUI tutorialText;  // Referência ao elemento de texto da UI
    public GameObject messageBox;  // Referência à caixa de mensagem (imagem)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // Verifique se o jogador entrou no trigger
        {
            tutorialText.text = tutorialMessage;
            messageBox.SetActive(true);  // Ative a caixa de mensagem
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // Verifique se o jogador saiu do trigger
        {
            messageBox.SetActive(false);  // Desative a caixa de mensagem
        }
    }
}

