using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class score : MonoBehaviour
{
    private int _score;
    public Text ScoreTxt;

    void Start()
    {
        // Carrega a pontuação do PlayerPrefs
        _score = PlayerPrefs.GetInt("Score", 0);
        UpdateScoreText();
    }

    public void AddScore(int value)
    {
        _score += value;
        UpdateScoreText();
        // Salva a nova pontuação no PlayerPrefs
        PlayerPrefs.SetInt("Score", _score);
    }

    private void UpdateScoreText()
    {
        ScoreTxt.text = _score.ToString();
    }
}
