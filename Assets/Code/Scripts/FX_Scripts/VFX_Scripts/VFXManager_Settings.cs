using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New VFX Manager", menuName = "Settings/Managers/VFX Manager")]
public class VFXManager_Settings : ScriptableObject
{
    [SerializeField] public int MaxVFX;
    [SerializeField] public ParticleSystem DefaultParticleSystem;
}
