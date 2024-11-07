using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // Necessário para usar TextMeshPro
using UnityEngine.UI;  // Necessário para usar Image e Button

public class TutorialTrigger : MonoBehaviour
{
    public string tutorialMessage;  // Mensagem que será exibida
    public TextMeshProUGUI tutorialText;  // Referência ao elemento de texto da UI
    public GameObject messageBox;  // Referência à caixa de mensagem
    public Button closeButton;  // Referência ao botão de fechar

    private bool hasShownMessage = false;  // Variável para controlar se a mensagem já foi mostrada

    private void Start()
    {
        // Configure o botão para chamar a função CloseTutorial ao ser pressionado
        closeButton.onClick.AddListener(CloseTutorial);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasShownMessage)  // Verifique se o jogador entrou e a mensagem ainda não foi mostrada
        {
            tutorialText.text = tutorialMessage;
            messageBox.SetActive(true);  // Ative a caixa de mensagem
            Time.timeScale = 0;  // Pause o jogo
            hasShownMessage = true;  // Marque que a mensagem foi mostrada
        }
    }

    public void CloseTutorial()
    {
        Time.timeScale = 1;  // Retome o jogo antes de fechar a mensagem para evitar problemas de interação
        messageBox.SetActive(false);  // Desative a caixa de mensagem
    }

}