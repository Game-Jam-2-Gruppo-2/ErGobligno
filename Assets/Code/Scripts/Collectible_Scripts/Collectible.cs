using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour, ICollectible
{
    [SerializeField] private int m_ScoreValue = 1;
    [SerializeField] AudioClip m_ClipOnCollect;
    [SerializeField, Range(-1f, 1f)] float m_PitchVariation = 0.2f;

    public int ScoreValue { get; set; }

    private void Awake()
    {
        ScoreValue = m_ScoreValue;
    }

    public void Collect()
    {
        ScoreManager.IncreaseCoin(ScoreValue);
        AudioManager.Request3DSFX?.Invoke(m_ClipOnCollect, transform.position, 1+UnityEngine.Random.Range(-m_PitchVariation, m_PitchVariation));
        this.gameObject.SetActive(false);
    }
}
