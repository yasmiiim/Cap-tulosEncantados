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
        _score = 0;
        UpdateScoreText();
    }

    public void AddScore(int value)
    {
        _score += value;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        ScoreTxt.text = _score.ToString();
    }
}
