using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class score : MonoBehaviour
{
    private static int _score; // Variável estática para manter a pontuação entre as cenas
    public Text ScoreTxt;

    void Start()
    {
        // Verifica se estamos na cena inicial
        if (IsInitialScene())
        {
            ResetScore(); // Reseta a pontuação se estivermos na cena inicial
        }
        UpdateScoreText();
    }

    public void AddScore(int value)
    {
        _score += value;
        UpdateScoreText();
        // Salva a nova pontuação no PlayerPrefs, se desejado
        PlayerPrefs.SetInt("Score", _score);
    }

    private void ResetScore()
    {
        // Reseta a pontuação para 0
        _score = 0;
        PlayerPrefs.SetInt("Score", _score); // Salva a pontuação zerada
    }

    private void UpdateScoreText()
    {
        ScoreTxt.text = _score.ToString();
    }

    private bool IsInitialScene()
    {
        // Aqui você pode verificar se está na cena inicial, substitua "NomeDaCenaInicial" pelo nome correto
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "NomeDaCenaInicial";
    }
}
