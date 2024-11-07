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

    private void Start()
    {
        // Configure o botão para chamar a função CloseTutorial ao ser pressionado
        closeButton.onClick.AddListener(CloseTutorial);

        // Configura o botão para usar um evento de clique que funcione com o tempo pausado
        closeButton.onClick.AddListener(() => {
            Time.timeScale = 1;
            CloseTutorial();
        });
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // Verifique se o jogador entrou no trigger
        {
            tutorialText.text = tutorialMessage;
            messageBox.SetActive(true);  // Ative a caixa de mensagem
            Time.timeScale = 0;  // Pause o jogo
        }
    }

    public void CloseTutorial()
    {
        messageBox.SetActive(false);  // Desative a caixa de mensagem
    }
}