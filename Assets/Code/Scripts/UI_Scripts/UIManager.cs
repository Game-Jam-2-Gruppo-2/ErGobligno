using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private UIGame Game_Canvas;
    [SerializeField] private UIScreen Pause_Canvas;

    private bool IsPaused = false;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    private enum UI_State
    {
        Game,
        Pause,
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void ChangeState(UI_State state)
    {
        switch (state)
        {
            case UI_State.Game:
            {
                Game_Canvas.gameObject.SetActive(true);
                Pause_Canvas.gameObject.SetActive(false);
                break;
            }
            case UI_State.Pause:
            {
                Game_Canvas.gameObject.SetActive(false);
                Pause_Canvas.gameObject.SetActive(true);
                break;
            }
        }
    }

    private void SwichState()
    {
        if (IsPaused)
            GameCanvas();
        else
            PauseCanvas();
        IsPaused = !IsPaused;
    }

    public void PauseCanvas() => ChangeState(UI_State.Pause);
    public void GameCanvas() => ChangeState(UI_State.Game);

    private void OnEnable()
    {
        GameCanvas();
        GameManager.OnGamePause += SwichState;
    }

    private void OnDisable()
    {
        GameManager.OnGamePause -= SwichState;
    }
}
