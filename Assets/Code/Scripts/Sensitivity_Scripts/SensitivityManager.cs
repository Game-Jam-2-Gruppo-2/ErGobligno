using UnityEngine;

public class SensitivityManager: MonoBehaviour
{
    [SerializeField] private Sensitivity_Settings m_Settings;

    private static Sensitivity_Settings Settings;

    private void Awake()
    {
        Settings = m_Settings;
    }

    public static Vector2 SensitivityValue()
    {
        if (InputManager.UsingController)
        {
            return new Vector2(Settings.GetControllerSens_Horizontal(), Settings.GetControllerSens_Vertical());
        }
        else
        {
            Debug.Log("USING MOUSE");
            return new Vector2(Settings.GetMouseSens(), Settings.GetMouseSens());
        }
    }

    public static void SetMouseSens(float newSens) => Settings.SetMouseSens(newSens);
    public static void SetControllerVSens(float newSens) => Settings.SetControllerVSens(newSens);
    public static void SetControllerHSens(float newSens) => Settings.SetControllerHSens(newSens);
}
