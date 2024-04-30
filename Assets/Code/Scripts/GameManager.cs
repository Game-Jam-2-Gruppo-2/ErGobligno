using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singeton
    [HideInInspector] public static GameManager Instance;

    public delegate void NewGame();
    public static event NewGame OnNewGame;

    public delegate void GamePaused();
    public static event GamePaused OnGamePause;

    private GameState CurrentState = GameState.Menu;
    private bool m_InGame = false;

    private void Awake()
    {
        //Singleton set up
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("MULTIPLE GAMEMANAGER FOUND");
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    //TODO: REMOVE
    private void Start()
    {
        ChangeState(GameState.Game);
    }

    private void ChangeState(GameState newState)
    {
        switch (newState)
        {
            case GameState.Menu:
                //TODO: Change State -> Menu
                break;

            case GameState.Game:
                if(!m_InGame)
                {
                    m_InGame = true;
                    OnNewGame?.Invoke();
                }
                //TODO: Change State -> Game
                Time.timeScale = 1;
                break;

            case GameState.Pause:
                if (CurrentState == newState)
                {
                    Time.timeScale = 0;
                    ChangeState(GameState.Game);
                    OnGamePause?.Invoke();
                }
                break;
        }
    }

    private void PauseGame()
    {
        ChangeState(GameState.Pause);
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        OnNewGame -= OnNewGame;
        OnGamePause -= OnGamePause;
    }
}

/// <summary>
/// Game states
/// </summary>
public enum GameState
{
    Menu,
    Pause,
    Game
}