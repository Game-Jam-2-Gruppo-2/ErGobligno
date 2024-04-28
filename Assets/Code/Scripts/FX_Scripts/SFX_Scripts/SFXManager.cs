using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class SFXManager : MonoBehaviour
{
    public static Action<AudioClip, Vector3> Request3DSFX = (AudioClip clip, Vector3 pos) => { };
    public static Action<AudioClip, Vector3> Request2DSFX = (AudioClip clip, Vector3 pos) => { };

    [SerializeField] private SFXManager_Settings m_Settings;

    private List<SFXEffect> Sources_3D = new List<SFXEffect>();
    private List<SFXEffect> Sources_2D = new List<SFXEffect>();

    private void Awake()
    {
        Request3DSFX += Request3D_SFX;
        Request2DSFX += Request2D_SFX;
        SetUp();
    }

    private void SetUp()
    {
        //Instanciate 3D SFX
        for (int i = 0; i < m_Settings.Max3DSFX; i++)
        {
            Sources_3D.Add(Instantiate(m_Settings.Source3D, transform));
            Sources_3D[i].gameObject.SetActive(false);
        }
        //Instanciate 2D SFX
        for (int i = 0; i < m_Settings.Max2DSFX; i++)
        {
            Sources_2D.Add(Instantiate(m_Settings.Source2D, transform));
            Sources_2D[i].gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// Place a game object containing a 3D audio source
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="position"></param>
    public void Request3D_SFX(AudioClip clip, Vector3 position) 
    {
        if (clip == null)
            return;

        for (int i = 0;i < m_Settings.Max3DSFX; i++)
        {
            if (!Sources_3D[i].gameObject.activeInHierarchy)
            {
                Sources_3D[i].gameObject.SetActive(true);
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

        for (int i = 0; i < m_Settings.Max2DSFX; i++)
        {
            if (!Sources_2D[i].gameObject.activeInHierarchy)
            {
                Sources_2D[i].gameObject.SetActive(true);
                Sources_2D[i].PlaySound(position, clip);
                return;
            }
        }
    }

    private void OnDestroy()
    {
        Request3DSFX -= Request3DSFX;
        Request2DSFX -= Request2DSFX;
    }
}
