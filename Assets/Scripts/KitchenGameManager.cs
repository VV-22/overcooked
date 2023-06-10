using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{
    
    public static KitchenGameManager Instance { get; private set;}

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    private enum State 
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }
    private bool isGamePaused = false;

    private State state;
    // private float waitingToStartTimer = 1f;
    private float CountdownToStartTimer = 3f;
    private float GamePlayingTimer;
    private float GamePlayingTimerMax = 10f;

    private void Awake()
    {
        state = State.WaitingToStart;
        Instance = this;
    }
    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if(state == State.WaitingToStart)
        {
            state = State.CountdownToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    public void TogglePauseGame()
    {
        isGamePaused  = !isGamePaused;
        if(isGamePaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Update()
    {
        switch(state)
        {
            case State.WaitingToStart:
                break;
            case State.CountdownToStart:
                CountdownToStartTimer -= Time.deltaTime;
                if(CountdownToStartTimer < 0f)
                {
                    state = State.GamePlaying;
                    GamePlayingTimer = GamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                GamePlayingTimer -= Time.deltaTime;
                if(GamePlayingTimer < 0f)
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
        // Debug.Log(state);
    }

    public bool IsGamePlaying()
    {
        return state== State.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return state == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return CountdownToStartTimer;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (GamePlayingTimer / GamePlayingTimerMax) ;
    }
}
