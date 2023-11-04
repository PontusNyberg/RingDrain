using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreCardUI : MonoBehaviour {
    public static ScoreCardUI Instance { get; private set; }

    [SerializeField] TextMeshProUGUI scoreText;

    private void Awake() {
        Instance = this;
        scoreText.text = "0";
    }

    public void SetScoreText(string score) {
        scoreText.text = score;
    }

    public int GetScore() {
        return int.Parse(scoreText.text);
    }
}
