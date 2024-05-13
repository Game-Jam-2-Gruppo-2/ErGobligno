using System;
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

	public delegate void GameEnded();
	public static event GameEnded OnGameEnd;

	private GameState CurrentState = GameState.Menu;
	private bool m_InGame = false;

	private void Awake()
	{
		//Singleton set up
		if (Instance == null)
			Instance = this;
		else
		{
			Debug.LogError("MULTIPLE GAME MANAGER FOUND");
			Destroy(this.gameObject);
			return;
		}

		DontDestroyOnLoad(this.gameObject);

		InputManager.Initialize();
        InputManager.inputActions.Movement.Pause.performed += PauseGame;
        InputManager.inputActions.UI.Pause.performed += PauseGame;
    }

	private void Start()
	{
		//TODO: Swicth to gamestate game when scene is loaded
		ChangeState(GameState.Game);
	}

	private void ChangeState(GameState newState)
	{
		switch (newState)
		{
			case GameState.Menu:
				Time.timeScale = 1;
				//TODO: Change State -> Menu
				break;

			case GameState.Game:
				//TODO: FIX
				if (!m_InGame)
				{
					m_InGame = true;
					OnNewGame?.Invoke();
				}
				Time.timeScale = 1;
				CurrentState = newState;
				break;

			case GameState.Pause:
				Time.timeScale = 0;
				CurrentState = newState;
				break;
		}
	}

	private void PauseGame(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		if (CurrentState == GameState.Menu)
			return;

		if (CurrentState == GameState.Game)
		{
			ChangeState(GameState.Pause);
			InputManager.MoveInputs(false);
			InputManager.UiInputs(true);
		}
		else
		{
			ChangeState(GameState.Game);
			InputManager.MoveInputs(true);
			InputManager.UiInputs(false);
		}
		OnGamePause?.Invoke();
	}

	private void CheckNoise()
	{
		if (ScoreManager.IsNoiseOnMax())
		{
			OnGameEnd?.Invoke();
		}
	}

	private void OnEnable()
	{
		ScoreManager.OnNoiseChanged += CheckNoise;
	}

	private void OnDisable()
	{
		InputManager.inputActions.UI.Pause.performed -= PauseGame;
        InputManager.inputActions.Movement.Pause.performed -= PauseGame;
        ScoreManager.OnNoiseChanged -= CheckNoise;
		OnNewGame -= OnNewGame;
		OnGamePause -= OnGamePause;
		OnGameEnd -= OnGameEnd;
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