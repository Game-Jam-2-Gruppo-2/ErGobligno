using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour, ICollectible
{
    public int ScoreValue { get; set; }

    public void Collect()
    {
        ScoreManager.IncreaseCoin(ScoreValue);
    }
}
