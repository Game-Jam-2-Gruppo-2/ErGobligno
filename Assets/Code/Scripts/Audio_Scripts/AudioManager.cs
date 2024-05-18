using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Settings")]
    [SerializeField] private AudioMixer m_AudioMixer;
    [SerializeField] private AudioManager_Settings m_AudioManager_Settings;
    private List<AudioData> m_AudioData_List;
    private static AudioMixer Mixer;

    [Header("SFX Manager Settings")]
    [SerializeField] private SFXManager_Settings m_SFX_Settings;
    //SFX Events
    public static Action<AudioClip, Vector3, float> Request3DSFX = (AudioClip clip, Vector3 pos, float pitch) => { };
    public static Action<AudioClip, Vector3> Request2DSFX = (AudioClip clip, Vector3 pos) => { };
    //SFX Lists
    private List<SFXEffect> Sources_3D = new List<SFXEffect>();
    private List<SFXEffect> Sources_2D = new List<SFXEffect>();

    private void Awake()
    {
        //Singleton set up
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("MULTIPLE AUDIO MANAGER FOUND");
            Destroy(this.gameObject);
            return;
        }
        
        SetUpSFX();
        Request3DSFX += Request3D_SFX;
        Request2DSFX += Request2D_SFX;
    }

    private void Start()
    {
        SetUpAudio();
    }

    #region Audio Manager
    private void SetUpAudio()
    {
        Mixer = m_AudioMixer;
        m_AudioData_List = m_AudioManager_Settings.AudioData_List;
        //Audio is saved in decimal scale an then converted
        float volume = 0f;
        for (int i = 0; i < m_AudioData_List.Count; i++)
        {
            volume = m_AudioData_List[i].DefaultVolume;
            //Player Prefs
            if (!PlayerPrefs.HasKey(m_AudioData_List[i].Type.ToString() + "Volume"))
                PlayerPrefs.SetFloat(m_AudioData_List[i].Type.ToString() + "Volume", m_AudioData_List[i].DefaultVolume);
            else
                volume = PlayerPrefs.GetFloat(m_AudioData_List[i].Type.ToString() + "Volume");

            //Convert audio volume
            volume = 20 * Mathf.Log10(volume);
            if (volume < -80f)
                volume = -80f;
            //Set Mixer volume
            Mixer.SetFloat(m_AudioData_List[i].Type.ToString() + "Volume", volume);
        }
    }

    /// <summary>
    /// Change audio volume and save it into prefs
    /// </summary>
    /// <param name="type"></param>
    /// <param name="volume"></param>
    public static void ChangeVolume(AudioChannel type, float volume)
    {
        //Update Player Prefs
        PlayerPrefs.SetFloat(type.ToString() + "Volume", volume);

        //Convert audio volume
        volume = 20 * Mathf.Log10(volume);
        if (volume < -80f)
            volume = -80f;
        //Set Mixer volume
        Mixer.SetFloat(type.ToString() + "Volume", volume);
    }
    #endregion

    #region SFX Manager
    private void SetUpSFX()
    {
        //Instanciate 3D SFX
        for (int i = 0; i < m_SFX_Settings.Max3DSFX; i++)
        {
            Sources_3D.Add(Instantiate(m_SFX_Settings.Source3D, transform));
            Sources_3D[i].gameObject.SetActive(false);
        }
        //Instanciate 2D SFX
        for (int i = 0; i < m_SFX_Settings.Max2DSFX; i++)
        {
            Sources_2D.Add(Instantiate(m_SFX_Settings.Source2D, transform));
            Sources_2D[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Place a game object containing a 3D audio source
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="position"></param>
    public void Request3D_SFX(AudioClip clip, Vector3 position, float pitch)
    {
        if (clip == null)
            return;

        for (int i = 0; i < m_SFX_Settings.Max3DSFX; i++)
        {
            if (!Sources_3D[i].gameObject.activeInHierarchy)
            {
                Sources_3D[i].gameObject.SetActive(true);
                Sources_3D[i].SetPitch(pitch);
                Sources_3D[i].PlaySound(position, clip);
                return;
            }
        }
    }

    /// <summary>
    /// Place a game object containing a 2D audio source
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="position"></param>
    public void Request2D_SFX(AudioClip clip, Vector3 position)
    {
        if (clip == null)
            return;

        for (int i = 0; i < m_SFX_Settings.Max2DSFX; i++)
        {
            if (!Sources_2D[i].gameObject.activeInHierarchy)
            {
                Sources_2D[i].gameObject.SetActive(true);
                Sources_2D[i].PlaySound(position, clip);
                return;
            }
        }
    }
    #endregion

    private void OnDestroy()
    {
        Request3DSFX -= Request3DSFX;
        Request2DSFX -= Request2DSFX;
    }
}

/// <summary>
/// Audio Enum Channels
/// </summary>
public enum AudioChannel
{
    Master,
    Music,
    SFX
}
