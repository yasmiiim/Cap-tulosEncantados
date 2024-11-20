using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutirial : MonoBehaviour
{
    public string[] dialogueObj;
    public int dialogueIndex;

    public GameObject dialoguepanel;
    public Text dialogueText;
    
    public Text nameRaposa;
    public Image imageRaposa;
    public Sprite spriteRaposa;

    public bool readyToSpeak;
    public bool startDialogue;

    private void Start()
    {
        dialoguepanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && readyToSpeak)
        {
            if (!startDialogue)
            {
                FindObjectOfType<Character>().Speed = 0f; // Desativar a animação do personagem aqui, se necessário
                StartDialogue();
            }
            else if (dialogueText.text == dialogueObj[dialogueIndex])
            {
                NextDialogue();
            }
        }
    }

    void NextDialogue()
    {
        dialogueIndex++;

        if (dialogueIndex < dialogueObj.Length)
        {
            StartCoroutine(ShowDialogue());
        }
        else
        {
            dialoguepanel.SetActive(false);
            startDialogue = false;
            dialogueIndex = 0;
            FindObjectOfType<Character>().Speed = 5f; // Restaura a velocidade do personagem
        }
    }

    void StartDialogue()
    {
        nameRaposa.text = "Sage";
        imageRaposa.sprite = spriteRaposa;
        startDialogue = true;
        dialogueIndex = 0;
        dialoguepanel.SetActive(true);
        StartCoroutine(ShowDialogue());
    }

    IEnumerator ShowDialogue()
    {
        dialogueText.text = ""; // Limpa o texto atual antes de mostrar o próximo diálogo

        foreach (char letter in dialogueObj[dialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            readyToSpeak = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        readyToSpeak = false;
    }
}
