using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;

    private enum State {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private State state;
    private float countDownTostartTimerDefaultVal = 1f;
    private float countdownToStartTimer;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 5f;

    private void Awake() {
        Instance = this;

        countdownToStartTimer = countDownTostartTimerDefaultVal;
        state = State.CountdownToStart;
    }

    private void Start() {
        PlayerController.Instance.GetComponent<Health>().OnHealthEmpty += Health_OnHealthEmpty;
    }

    private void Health_OnHealthEmpty(object sender, EventArgs e) {
        state = State.GameOver;
        OnStateChanged?.Invoke(this, new EventArgs());
    }

    private void Update() {
        switch(state) {
            case State.WaitingToStart:
                // Before start
                break;
            case State.CountdownToStart:
                // During countDown to start game
                countdownToStartTimer -= Time.deltaTime;
                if(countdownToStartTimer < 0f) {
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                // Playing
                gamePlayingTimer -= Time.deltaTime;
                if(gamePlayingTimer < 0f) {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                // Time is up or player has died
                break;
        }
    }

    public void ResetState() {
        countdownToStartTimer = countDownTostartTimerDefaultVal;
        state = State.CountdownToStart;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsGamePlaying() {
        return state == State.GamePlaying;
    }

    public bool IsCountdownToStartActive() {
        return state == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer() {
        return countdownToStartTimer;
    }

    public bool IsGameOver() {
        return state == State.GameOver;
    }

    public float GetGamePlayingTimer() {
        return gamePlayingTimer;
    }

}
