using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bottle", menuName = "Settings/Env/Bottle")]
public class Bottle_Settings : ScriptableObject
{
    [Header("Bottle Settings")]
    [Tooltip("Force needed to break the bottle once it collide")]
    [SerializeField][Min(0.1f)] public float BreakingForce;
    [Header("FX")]
    [SerializeField] public ParticleSystem Breaking_VFX;
    [SerializeField] public AudioClip Breaking_SFX;
}
