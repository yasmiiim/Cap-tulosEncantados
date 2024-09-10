using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenuManager : MonoBehaviour
{
    public static DeathMenuManager Instance { get; private set; }

    [SerializeField] private GameObject deathMenuUI;

    private void Awake()
    {
        // Verifica se a instância já existe
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Opcional: se quiser manter o objeto ao trocar de cena
        }
        else
        {
            Destroy(gameObject); // Remove o objeto se já existir uma instância
        }
    }

    public void ShowDeathMenu()
    {
        if (deathMenuUI != null)
        {
            deathMenuUI.SetActive(true); // Exibe o menu de morte
        }
    }

    public void HideDeathMenu()
    {
        if (deathMenuUI != null)
        {
            deathMenuUI.SetActive(false); // Oculta o menu de morte
        }
    }
}
