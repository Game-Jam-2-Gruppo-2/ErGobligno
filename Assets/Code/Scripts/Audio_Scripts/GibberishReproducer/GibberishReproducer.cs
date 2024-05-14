using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GibberishReproducer : MonoBehaviour
{
    [SerializeField] List<AudioClip> m_ListOfClips = new List<AudioClip>();
    private AudioSource m_Source;

    private void Awake()
    {
        m_Source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(ReproduceSound());
    }

    private IEnumerator ReproduceSound()
    {
        while (true)
        {
            if(!m_Source.isPlaying)
            {
                m_Source.clip = m_ListOfClips[UnityEngine.Random.Range(0, m_ListOfClips.Count)];
                m_Source.Play();
            }
            yield return null;
        }
    }
}
