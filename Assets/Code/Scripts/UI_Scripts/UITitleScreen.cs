using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UITitleScreen : MonoBehaviour
{
    [SerializeField] private Button DefaultSelectedOnController;

    public void LoadScene(int value)
    {
        SceneManager.LoadScene(value);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    private void OnEnable()
    {
        if(InputManager.UsingController)
        {
            EventSystem.current.SetSelectedGameObject(DefaultSelectedOnController.gameObject);
        }
    }

    private void OnDisable()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
