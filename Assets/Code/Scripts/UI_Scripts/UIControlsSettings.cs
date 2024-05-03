using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
        m_MouseSensitivity_Slider.value = m_SensitivitySettings.GetMouseSens();
        //Controller V sens slider
        m_ControllerVertical_SensitivitySlider.maxValue = m_SensitivitySettings.Sensitivity_Max;
        m_ControllerVertical_SensitivitySlider.minValue = m_SensitivitySettings.Sensitivity_Min;
        m_ControllerVertical_SensitivitySlider.value = m_SensitivitySettings.GetControllerSens_Vertical();
        //Controller H sens slider
        m_ControllerHorizontalSensitivity_Slider.maxValue = m_SensitivitySettings.Sensitivity_Max;
        m_ControllerHorizontalSensitivity_Slider.minValue = m_SensitivitySettings.Sensitivity_Min;
        m_ControllerHorizontalSensitivity_Slider.value = m_SensitivitySettings.GetControllerSens_Horizontal();
    }

    public void UpdateMouseSensitivity() => m_SensitivitySettings.SetMouseSens(m_MouseSensitivity_Slider.value);

    public void UpdateControllerVSensitivity() => m_SensitivitySettings.SetControllerVSens(m_ControllerVertical_SensitivitySlider.value);

    public void UpdateControllerHSensitivity() => m_SensitivitySettings.SetControllerHSens(m_ControllerHorizontalSensitivity_Slider.value);

    public void SetControllerUsage(bool value) => InputManager.SetController(value);

    private void OnEnable()
    {
        m_MouseSensitivity_Slider.value = m_SensitivitySettings.GetMouseSens();
        m_ControllerVertical_SensitivitySlider.value = m_SensitivitySettings.GetControllerSens_Vertical();
        m_ControllerHorizontalSensitivity_Slider.value = m_SensitivitySettings.GetControllerSens_Horizontal();

        if (InputManager.UsingController)
            EventSystem.current.SetSelectedGameObject(m_MouseSensitivity_Slider.gameObject);
    }
    private void OnDisable()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
