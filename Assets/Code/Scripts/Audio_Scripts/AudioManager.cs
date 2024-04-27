using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private List<AudioData> AudioData_List;

    private void Awake()
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

            if (!PlayerPrefs.HasKey(AudioData_List[i].Type.ToString() + "Volume"))
                PlayerPrefs.SetFloat(AudioData_List[i].Type.ToString() + "Volume", AudioData_List[i].DefaultVolume);
            else
                volume = PlayerPrefs.GetFloat(AudioData_List[i].Type.ToString() + "Volume");
            
            //Convert audio volume
            volume = 20*Mathf.Log10(volume);
            if (volume < -80f)
                volume = -80f;
            
            AudioData_List[i].SubMixer.SetFloat(AudioData_List[i].Type.ToString() + "Volume", volume);
        }
    }
}
