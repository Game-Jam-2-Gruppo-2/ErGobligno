using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

	private PlayerInputs m_PlayerInputs = null;
	[Header("Scenes")]
	[SerializeField] private List<String> GameScenes;
    [SerializeField] private List<String> MenuScenes;

    [SerializeField] private String WinScreen_Scene;
    [SerializeField] private String LoseScreen_Scene;

	[Header("Debug")]
	[SerializeField] bool EnterOnGame = false;

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

		m_PlayerInputs = InputManager.inputActions;
        m_PlayerInputs.UI.Pause.performed += PauseGame;
        m_PlayerInputs.Movement.Pause.performed += PauseGame;

        SceneManager.sceneLoaded += OnSceneLoaded;
		OnGameEnd += FinishGame;
    }

	private void Start()
	{
        if (EnterOnGame)
            ChangeState(GameState.Game);
		else
            ChangeState(GameState.Menu);
    }

	private void ChangeState(GameState newState)
	{
		switch (newState)
		{
			case GameState.Menu:
                m_InGame = false;
                ScoreManager.OnNoiseChanged -= CheckNoise;
                ScoreManager.OnCoinChanged -= CheckCoin;
				Time.timeScale = 1;
                break;

			case GameState.Game:
				if (!m_InGame)
				{
					m_InGame = true;
					OnNewGame?.Invoke();
                    ScoreManager.OnNoiseChanged += CheckNoise;
                    ScoreManager.OnCoinChanged += CheckCoin;
                }
				Time.timeScale = 1;
				break;

			case GameState.Pause:
				Time.timeScale = 0;
				break;
		}
        CurrentState = newState;
    }

	private void PauseGame(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		if (CurrentState == GameState.Game)
		{
			m_PlayerInputs.Movement.Disable();
			m_PlayerInputs.UI.Enable();
			ChangeState(GameState.Pause);
        }
		else
		{
            m_PlayerInputs.Movement.Enable();
            m_PlayerInputs.UI.Disable();
			ChangeState(GameState.Game);
        }
		OnGamePause?.Invoke();
	}

	private void FinishGame()
	{
		if (ScoreManager.IsNoiseOnMax() || ScoreManager.GetGameTimeValue() <= 0)
            SceneManager.LoadScene(LoseScreen_Scene);
        else
			SceneManager.LoadScene(WinScreen_Scene);

        ScoreManager.OnNoiseChanged -= CheckNoise;
        ScoreManager.OnCoinChanged -= CheckCoin;
		ChangeState(GameState.Menu);
    }

	private void CheckNoise()
	{
		if (ScoreManager.IsNoiseOnMax())
		{
            OnGameEnd?.Invoke();
			FinishGame();
		}
	}

    private void CheckCoin()
    {
        if (ScoreManager.GetCoinLeft() <= 0)
		{
            OnGameEnd?.Invoke();
            FinishGame();
        }
    }

    private void CheckTimer()
    {
        if (ScoreManager.GetGameTimeValue() <= 0)
        {
            OnGameEnd?.Invoke();
            FinishGame();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
		for (int i = 0; i < GameScenes.Count; i++)
			if (GameScenes[i] == scene.name)
			{
				ChangeState(GameState.Game);
				return;
			}

        for (int i = 0; i < MenuScenes.Count; i++)
            if (MenuScenes[i] == scene.name)
			{
                ChangeState(GameState.Menu);
                return;
            }
    }

    private void OnEnable()
	{
		ScoreManager.OnNoiseChanged += CheckNoise;
		ScoreManager.OnCoinChanged += CheckCoin;
		ScoreManager.OnGameTimeChanged += CheckTimer;
	}

	private void OnDisable()
	{
        m_PlayerInputs.UI.Pause.performed -= PauseGame;
        m_PlayerInputs.Movement.Pause.performed -= PauseGame;
        ScoreManager.OnNoiseChanged -= CheckNoise;
        ScoreManager.OnCoinChanged -= CheckCoin;
        ScoreManager.OnGameTimeChanged -= CheckTimer;
        SceneManager.sceneLoaded -= OnSceneLoaded;
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