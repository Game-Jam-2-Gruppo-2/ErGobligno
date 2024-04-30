using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Score Manager", menuName = "Settings/Managers/Score Manager")]
public class ScoreManager_Settings : ScriptableObject
{
    [SerializeField, Min(0f)] public float MaxNoise = 1.0f;
    [Tooltip("Amount of scores saved in player prefs")]
    [SerializeField, Min(1)] public int SavedScoreCount = 5;
}
