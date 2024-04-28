using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sensitivity Settings", menuName = "Settings/Other/Sensitivity")]
public class Sensitivity_Settings : ScriptableObject
{
    [SerializeField] public float Sensitivity_Min { private set; get; } = 0.1f;
    [SerializeField] private float Sensitivity_Default = 500f;
    [SerializeField] public float Sensitivity_Max { private set; get; } = 1000f;

    [HideInInspector] public float MouseSensitivity { private set; get; }
    [HideInInspector] public float ControllerSensitivity_Vertical { private set; get; }
    [HideInInspector] public float ControllerSensitivity_Horizontal { private set; get; }

    private void OnEnable()
    {
        //Setup Mouse sens value
        if (PlayerPrefs.HasKey("MouseSens"))
            MouseSensitivity = PlayerPrefs.GetFloat("MouseSens");
        else
            PlayerPrefs.SetFloat("MouseSens", Sensitivity_Default);

        //Setup Controller v sens value
        if (PlayerPrefs.HasKey("ControllerVSens"))
            ControllerSensitivity_Vertical = PlayerPrefs.GetFloat("ControllerVSens");
        else
            PlayerPrefs.SetFloat("ControllerVSens", ControllerSensitivity_Vertical);
        
        //Setup Controller h sens value
        if (PlayerPrefs.HasKey("ControllerHSens"))
            ControllerSensitivity_Horizontal = PlayerPrefs.GetFloat("ControllerHSens");
        else
            PlayerPrefs.SetFloat("ControllerHSens", ControllerSensitivity_Horizontal);
    }

    public void SetMouseSens(float newSens)
    {
        MouseSensitivity = newSens;
        PlayerPrefs.SetFloat("MouseSens", MouseSensitivity);
    }

    public void SetControllerVSens(float newSens)
    {
        ControllerSensitivity_Vertical = newSens;
        PlayerPrefs.SetFloat("ControllerVSens", ControllerSensitivity_Vertical);
    }

    public void SetControllerHSens(float newSens)
    {
        ControllerSensitivity_Horizontal = newSens;
        PlayerPrefs.SetFloat("ControllerHSens", ControllerSensitivity_Horizontal);
    }
}
