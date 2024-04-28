using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Manager", menuName = "Settings/Managers/Audio Manager")]
public class AudioManager_Settings : ScriptableObject
{
    [Header("Audio Settings")]
    [SerializeField] public List<AudioData> AudioData_List;
}
