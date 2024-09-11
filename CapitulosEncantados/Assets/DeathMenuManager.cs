using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenuManager : MonoBehaviour
{
    public static DeathMenuManager Instance { get; private set; }

    [SerializeField] private GameObject deathMenuUI;

    private void Awake()
    {
       
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public void ShowDeathMenu()
    {
        if (deathMenuUI != null)
        {
            deathMenuUI.SetActive(true); 
        }
    }

    public void HideDeathMenu()
    {
        if (deathMenuUI != null)
        {
            deathMenuUI.SetActive(false); 
        }
    }
}
