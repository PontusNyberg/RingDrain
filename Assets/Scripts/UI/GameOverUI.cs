using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {
    [SerializeField] private Button PlayAgainButton;
    [SerializeField] private Button ExitButton;

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
