using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singeton
    [HideInInspector] public static GameManager Instance;

    public delegate void NewGame();
    public static event NewGame OnNewGame;

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

                }
                //TODO: Change State -> Game

                break;

            case GameState.Pause:
                if (CurrentState == newState)
                    ChangeState(GameState.Game);
                break;
        }
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        OnNewGame -= OnNewGame;
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