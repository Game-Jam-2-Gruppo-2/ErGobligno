using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIVideoSettings : UIScreen
{
    [SerializeField] private TMP_Dropdown ScreenTypeDropDown;
    [SerializeField] private TMP_Dropdown ResolutionDropDown;

    private List<Resolution> m_ScreenResolutions;

    private void Awake()
    {
        //Setup prefs
        if (!PlayerPrefs.HasKey("FullScreenMode"))
            PlayerPrefs.SetInt("FullScreenMode", 0);

        if (!PlayerPrefs.HasKey("ScreenResolution"))
            PlayerPrefs.SetInt("ScreenResolution", 0);

        //Update screen resolution
        m_ScreenResolutions = new List<Resolution>();
        //Load all resolutions
        Resolution[] resolutions = Screen.resolutions;
        List<string> resolutionData = new List<string>();
        for (int i = resolutions.Length-1; i >= 0 ; i--)
        {
            //New Res String
            string newRes = resolutions[i].height+"x"+resolutions[i].width+"\t"+Mathf.RoundToInt((float)resolutions[i].refreshRateRatio.value)+" Hz";
            //Add to resolution list
            resolutionData.Add(newRes);
            //update Screen Resolution Array
            m_ScreenResolutions.Add(resolutions[i]);
        }
        //Add resolutions to dropdown
        ResolutionDropDown.AddOptions(resolutionData);
        //Set dropdown box value at current resolution
        ResolutionDropDown.value = PlayerPrefs.GetInt("ScreenResolution");
        //Set dropdown box value at current screen mode
        ScreenTypeDropDown.value = PlayerPrefs.GetInt("FullScreenMode");

        //Load Screen Resolution
        ChangeScreenResolution(PlayerPrefs.GetInt("ScreenResolution"), PlayerPrefs.GetInt("FullScreenMode"));
    }

    private FullScreenMode ChangeScreenMode(int mode)
    {
        switch (mode)
        {
            case 0: //Fullscreen
                PlayerPrefs.SetInt("FullScreenMode", mode);
                return FullScreenMode.ExclusiveFullScreen;
            case 1: //Borderless
                PlayerPrefs.SetInt("FullScreenMode", mode);
                return FullScreenMode.FullScreenWindow;
            case 2: //Windowed
                PlayerPrefs.SetInt("FullScreenMode", mode);
                return FullScreenMode.Windowed;
            default: //Default
                PlayerPrefs.SetInt("FullScreenMode", 0);
                return FullScreenMode.ExclusiveFullScreen;
        }
    }
    
    private void ChangeScreenResolution(int resolutionIndex, int modeIndex)
    {
        Screen.SetResolution(m_ScreenResolutions[resolutionIndex].width, m_ScreenResolutions[resolutionIndex].height, ChangeScreenMode(modeIndex), m_ScreenResolutions[resolutionIndex].refreshRateRatio);
        PlayerPrefs.SetInt("ScreenResolution", resolutionIndex);
    }

    public void ApplyResolution()
    {
        ChangeScreenResolution(ResolutionDropDown.value, ScreenTypeDropDown.value);
    }

    private void OnEnable()
    {
        base.Enable();
    }

    private void OnDisable()
    {
        base.Disable();
    }
}
