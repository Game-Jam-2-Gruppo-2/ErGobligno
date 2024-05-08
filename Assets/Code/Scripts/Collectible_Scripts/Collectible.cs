using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour, ICollectible
{
    [SerializeField] private int m_ScoreValue = 1;
    public int ScoreValue { get; set; }

    private void Awake()
    {
        ScoreValue = m_ScoreValue;
    }

    public void Collect()
    {
        ScoreManager.IncreaseCoin(ScoreValue);
    }
}
