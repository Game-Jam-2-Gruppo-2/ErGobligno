using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sensitivity Settings", menuName = "Settings/Other/Sensitivity")]
public class Sensitivity_Settings : ScriptableObject
{
    [SerializeField] private float Sensitivity_Default = 500f;

    public float Sensitivity_Min { private set; get; }
    [SerializeField, Min(0.1f)] private float _Sensitivity_Min = 0.1f;
    public float Sensitivity_Max { private set; get; } = 1000f;
    [SerializeField, Min(0.1f)] private float _Sensitivity_Max = 0.1f;

    private void OnEnable()
    {
        Sensitivity_Min = _Sensitivity_Min;
        Sensitivity_Max = _Sensitivity_Max;
    }

    public float GetMouseSens()
    {
        if (!PlayerPrefs.HasKey("MouseSens"))
            PlayerPrefs.SetFloat("MouseSens", Sensitivity_Default);

        return PlayerPrefs.GetFloat("MouseSens");
    }

    public float GetControllerSens_Vertical()
    {
        if (!PlayerPrefs.HasKey("ControllerVSens"))
            PlayerPrefs.SetFloat("ControllerVSens", Sensitivity_Default);

        return PlayerPrefs.GetFloat("ControllerVSens");
    }

    public float GetControllerSens_Horizontal()
    {
        if (!PlayerPrefs.HasKey("ControllerHSens"))
            PlayerPrefs.SetFloat("ControllerHSens", Sensitivity_Default);

        return PlayerPrefs.GetFloat("ControllerHSens");
    }

    public void SetMouseSens(float newSens) => PlayerPrefs.SetFloat("MouseSens", newSens);
    public void SetControllerVSens(float newSens) => PlayerPrefs.SetFloat("ControllerVSens", newSens);
    public void SetControllerHSens(float newSens) => PlayerPrefs.SetFloat("ControllerHSens", newSens);
}
