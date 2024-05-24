using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Score Manager", menuName = "Settings/Managers/Score Manager")]
public class ScoreManager_Settings : ScriptableObject
{
    [Tooltip("Max Time")]
    [SerializeField, Min(0f)] public float StartingTime = 15f;
    [SerializeField, Min(0f)] public float IncreaseTimeOnCoin = 15f;
    [Tooltip("Amount Noise Settings")]
    [SerializeField, Min(0f)] public float MaxNoise = 1.0f;
    [SerializeField, Min(0f)] public List<float> DecreaseTime = new List<float> { 5f };
    [Tooltip("Amount of scores saved in player prefs")]
    [SerializeField, Min(1)] public int SavedScoreCount = 5;
}
