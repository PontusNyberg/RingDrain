using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamePlayingCounterUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI playingCounterText;

    private void Update() {
        int countdownNumber = Mathf.CeilToInt(GameManager.Instance.GetGamePlayingTimer());
        playingCounterText.text = Mathf.CeilToInt(countdownNumber).ToString();
    }
}
