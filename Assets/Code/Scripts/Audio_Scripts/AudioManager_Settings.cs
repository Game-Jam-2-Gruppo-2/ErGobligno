using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Manager", menuName = "Settings/Managers/Audio Manager")]
public class AudioManager_Settings : ScriptableObject
{
    [Header("Audio Settings")]
    [SerializeField] public List<AudioData> AudioData_List;

    private void OnEnable()
    {
        SetUpAudio();
    }

    private void SetUpAudio()
    {
        //Audio is saved in decimal scale an then converted
        float volume = 0f;
        for (int i = 0; i < AudioData_List.Count; i++)
        {
            volume = AudioData_List[i].DefaultVolume;
            //Player Prefs
            if (!PlayerPrefs.HasKey(AudioData_List[i].Type.ToString() + "Volume"))
                PlayerPrefs.SetFloat(AudioData_List[i].Type.ToString() + "Volume", AudioData_List[i].DefaultVolume);
            else
                volume = PlayerPrefs.GetFloat(AudioData_List[i].Type.ToString() + "Volume");

            //Convert audio volume
            volume = 20 * Mathf.Log10(volume);
            if (volume < -80f)
                volume = -80f;
            //Set Mixer volume
            AudioData_List[i].SubMixer.SetFloat(AudioData_List[i].Type.ToString() + "Volume", volume);
        }
    }
}
