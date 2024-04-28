using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SFX Manager", menuName = "Settings/Managers/SFX Manager")]
public class SFXManager_Settings : ScriptableObject
{
    [Header("Settings")]
    [SerializeField][Range(0, 20)] public int Max3DSFX = 3;
    [SerializeField][Range(0, 20)] public int Max2DSFX = 5;
    [Header("Referces")]
    [SerializeField] public SFXEffect Source3D = null;
    [SerializeField] public SFXEffect Source2D = null;
}
