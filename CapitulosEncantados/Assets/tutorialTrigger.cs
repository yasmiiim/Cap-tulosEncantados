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
    private bool isTutorialActive = false;  // Controla se o tutorial está ativo no momento

    private void Start()
    {
        // Configure o botão para chamar a função CloseTutorial ao ser pressionado
        closeButton.onClick.AddListener(CloseTutorial);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasShownMessage)  // Verifique se o jogador entrou e a mensagem ainda não foi mostrada
        {
            hasShownMessage = true;  // Marque a mensagem como mostrada primeiro
            tutorialText.text = tutorialMessage;
            messageBox.SetActive(true);  // Ative a caixa de mensagem
            StartCoroutine(PauseGame());  // Atraso para pausar o jogo
        }
    }

    private IEnumerator PauseGame()
    {
        yield return new WaitForEndOfFrame();  // Atraso de um frame para garantir que o input não seja processado
        Time.timeScale = 0;  // Pause o jogo
        isTutorialActive = true;  // Indica que o tutorial está ativo
    }

    private void Update()
    {
        // Verifica se o tutorial está ativo e se a tecla de espaço foi pressionada
        if (isTutorialActive && Input.GetKeyDown(KeyCode.Space))
        {
            CloseTutorial();  // Fecha o tutorial ao pressionar espaço
        }
    }

    public void CloseTutorial()
    {
        Time.timeScale = 1;  // Retome o jogo antes de fechar a mensagem para evitar problemas de interação
        messageBox.SetActive(false);  // Desative a caixa de mensagem
        isTutorialActive = false;  // Indica que o tutorial não está mais ativo
    }
}
