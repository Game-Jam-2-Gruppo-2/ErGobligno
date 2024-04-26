using System;
using UnityEngine;

public class UIPause : MonoBehaviour
{
    public Action OnCloseMenu = ()=> { };

    [SerializeField] private Canvas Select_Canvas;
    [SerializeField] private Canvas VolumeSettings_Canvas;
    [SerializeField] private Canvas ControlsSettings_Canvas;
    [SerializeField] private Canvas VideoSettings_Canvas;

    private UI_State UI_CurrentState = UI_State.Default;

    private enum UI_State
    {
        Default,
        Select,
        VolumeSettings,
        ControlsSettings,
        VideoSettings,
    }
    
    private void ChangeState(UI_State state)
    {
        switch (state)
        {
            case UI_State.Select:
            {
                if (UI_CurrentState == state)
                    OnCloseMenu();
                Select_Canvas.gameObject.SetActive(true);
                VolumeSettings_Canvas.gameObject.SetActive(false);
                ControlsSettings_Canvas.gameObject.SetActive(false);
                VideoSettings_Canvas.gameObject.SetActive(false);
                break;
            }
            case UI_State.VolumeSettings:
            {
                Select_Canvas.gameObject.SetActive(false);
                VolumeSettings_Canvas.gameObject.SetActive(true);
                ControlsSettings_Canvas.gameObject.SetActive(false);
                VideoSettings_Canvas.gameObject.SetActive(false);
                break;
            }
            case UI_State.ControlsSettings:
            {
                Select_Canvas.gameObject.SetActive(false);
                VolumeSettings_Canvas.gameObject.SetActive(false);
                ControlsSettings_Canvas.gameObject.SetActive(true);
                VideoSettings_Canvas.gameObject.SetActive(false);
                break;
            }
            case UI_State.VideoSettings:
            {
                Select_Canvas.gameObject.SetActive(false);
                VolumeSettings_Canvas.gameObject.SetActive(false);
                ControlsSettings_Canvas.gameObject.SetActive(false);
                VideoSettings_Canvas.gameObject.SetActive(true);
                break;
            }
        }
        UI_CurrentState = state;
    }

    public void OnSelect() => ChangeState(UI_State.Select);
    public void OnAudioSettings() => ChangeState(UI_State.VolumeSettings);
    public void OnControlsSettings() => ChangeState(UI_State.ControlsSettings);
    public void OnVideoSettings() => ChangeState(UI_State.VideoSettings);

    private void OnEnable()
    {
        ChangeState(UI_State.Select);
    }

    private void OnDisable()
    {
        OnCloseMenu -= OnCloseMenu;
    }
}
