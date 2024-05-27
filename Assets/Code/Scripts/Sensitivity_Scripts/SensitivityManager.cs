using UnityEngine;

public class SensitivityManager: MonoBehaviour
{
    public static SensitivityManager Instance;

    [SerializeField] private Sensitivity_Settings m_Settings;

    private static Sensitivity_Settings Settings;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Settings = m_Settings;
        Instance = this;
    }

    public static Vector2 GetSensitivityValue()
    {
        if (InputManager.UsingController)
            return new Vector2(Settings.GetControllerSens_Horizontal(), Settings.GetControllerSens_Vertical());
        else
            return new Vector2(Settings.GetMouseSens(), Settings.GetMouseSens());
    }

    public static void SetMouseSens(float newSens) => Settings.SetMouseSens(newSens);
    public static void SetControllerVSens(float newSens) => Settings.SetControllerVSens(newSens);
    public static void SetControllerHSens(float newSens) => Settings.SetControllerHSens(newSens);
}
