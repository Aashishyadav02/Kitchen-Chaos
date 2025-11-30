using System;
using Unity.VisualScripting;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{ 
    public static KitchenGameManager Instance{get; private set;}
    
    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    
    private enum State
    {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        GameOver,
    }

    private State state;
    
    private float countDownToStartTimer = 3f;
    private float gamePlayingTimer;
    private float gamePlayingTimermax = 200f;
    private bool isGamePaused = false;

    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnIntreractAction += GameInput_OnIntreractAction;
    }

    private void GameInput_OnIntreractAction(object sender, EventArgs e)
    {
        if (state == State.WaitingToStart)
        {
            state = State.CountDownToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                break;
            case State.CountDownToStart:
                countDownToStartTimer -= Time.deltaTime;
                if (countDownToStartTimer < 0)
                {
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimermax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0)
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
        return state == State.GamePlaying;
    }

    public bool IsCountDownToStartActive()
    {
       return state == State.CountDownToStart;
    }

    public float GetCountDownToStartTimer()
    {
        return countDownToStartTimer;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public float GetGamePlayingTimerNormlized()
    {
        return 1-(gamePlayingTimer / gamePlayingTimermax );
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
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
}
