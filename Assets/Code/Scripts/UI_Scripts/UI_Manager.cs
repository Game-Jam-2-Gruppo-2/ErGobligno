using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance;


    [SerializeField] private Canvas Game_Canvas;
    [SerializeField] private UI_Pause Pause_Canvas;

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

    public void PauseCanvas() => ChangeState(UI_State.Pause);
    public void GameCanvas() => ChangeState(UI_State.Game);

    private void OnEnable()
    {
        ChangeState(UI_State.Pause);
        Pause_Canvas.OnCloseMenu += () => { ChangeState(UI_State.Game); };
    }
}
