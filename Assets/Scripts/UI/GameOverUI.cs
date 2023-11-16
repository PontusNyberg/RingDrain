using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {
    [SerializeField] private Button PlayAgainButton;
    [SerializeField] private Button ExitButton;
    [SerializeField] private GameObject ScoreList;

    private void Start() {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        Hide();

        PlayAgainButton.onClick.AddListener(() => {
            HealthBar.Instance.ResetHealthBar();
            GameManager.Instance.ResetState();
            Hide();
        });

        ExitButton.onClick.AddListener(() => { 
            Application.Quit();
        });
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e) {
        if(GameManager.Instance.IsGameOver()) {
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
