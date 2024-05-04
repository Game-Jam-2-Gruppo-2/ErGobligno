using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIScreen : MonoBehaviour
{
    public Action OnClose;

    [Header("UI Screen Settings")]
    [SerializeField] protected GameObject DefaultSelected;

    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);
    public void QuitGame() => Application.Quit();
    public void ResetSelected() => Enable();
    private void OnEnable() => Enable();
    private void OnDisable() => Disable();
    protected void Enable()
    {
        if (DefaultSelected == null)
            return;

        if (InputManager.UsingController)
            EventSystem.current.SetSelectedGameObject(DefaultSelected);
    }
    protected void Disable() => OnClose?.Invoke();
    private void OnDestroy() => OnClose -= OnClose;
}
