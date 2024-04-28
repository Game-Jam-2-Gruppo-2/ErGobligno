using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioManager_Settings m_Settings;

    private void Awake()
    {
        SetUpAudio();
    }

    private void SetUpAudio()
    {
        //Audio is saved in decimal scale an then converted
        float volume = 0f;
        for (int i = 0; i < m_Settings.AudioData_List.Count; i++)
        {
            volume = m_Settings.AudioData_List[i].DefaultVolume;

            if (!PlayerPrefs.HasKey(m_Settings.AudioData_List[i].Type.ToString() + "Volume"))
                PlayerPrefs.SetFloat(m_Settings.AudioData_List[i].Type.ToString() + "Volume", m_Settings.AudioData_List[i].DefaultVolume);
            else
                volume = PlayerPrefs.GetFloat(m_Settings.AudioData_List[i].Type.ToString() + "Volume");
            
            //Convert audio volume
            volume = 20*Mathf.Log10(volume);
            if (volume < -80f)
                volume = -80f;
            
            m_Settings.AudioData_List[i].SubMixer.SetFloat(m_Settings.AudioData_List[i].Type.ToString() + "Volume", volume);
        }
    }
}
