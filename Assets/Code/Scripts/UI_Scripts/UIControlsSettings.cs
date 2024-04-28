using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControlsSettings : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider m_MouseSensitivity_Slider;
    [SerializeField] private Slider m_ControllerVertical_SensitivitySlider;
    [SerializeField] private Slider m_ControllerHorizontalSensitivity_Slider;

    [SerializeField] private Sensitivity_Settings m_SensitivitySettings;

    private void Awake()
    {
        //Mouse sens slider
        m_MouseSensitivity_Slider.maxValue = m_SensitivitySettings.Sensitivity_Max;
        m_MouseSensitivity_Slider.minValue = m_SensitivitySettings.Sensitivity_Min;
        m_MouseSensitivity_Slider.value = m_SensitivitySettings.MouseSensitivity;
        //Controller V sens slider
        m_ControllerVertical_SensitivitySlider.maxValue = m_SensitivitySettings.Sensitivity_Max;
        m_ControllerVertical_SensitivitySlider.minValue = m_SensitivitySettings.Sensitivity_Min;
        m_ControllerVertical_SensitivitySlider.value = m_SensitivitySettings.ControllerSensitivity_Vertical;
        //Controller H sens slider
        m_ControllerHorizontalSensitivity_Slider.maxValue = m_SensitivitySettings.Sensitivity_Max;
        m_ControllerHorizontalSensitivity_Slider.minValue = m_SensitivitySettings.Sensitivity_Min;
        m_ControllerHorizontalSensitivity_Slider.value = m_SensitivitySettings.ControllerSensitivity_Horizontal;
    }

    public void UpdateMouseSensitivity() => m_SensitivitySettings.SetMouseSens(m_MouseSensitivity_Slider.value);

    public void UpdateControllerVSensitivity() => m_SensitivitySettings.SetControllerVSens(m_ControllerVertical_SensitivitySlider.value);

    public void UpdateControllerHSensitivity() => m_SensitivitySettings.SetControllerHSens(m_ControllerHorizontalSensitivity_Slider.value);

    private void OnEnable()
    {
        m_MouseSensitivity_Slider.value = m_SensitivitySettings.MouseSensitivity;
        m_ControllerVertical_SensitivitySlider.value = m_SensitivitySettings.ControllerSensitivity_Vertical;
        m_ControllerHorizontalSensitivity_Slider.value = m_SensitivitySettings.ControllerSensitivity_Horizontal;
    }
}
