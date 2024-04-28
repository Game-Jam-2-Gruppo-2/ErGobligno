using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bottle", menuName = "Settings/Env/Bottle")]
public class Bottle_Settings : ScriptableObject
{
    [Header("Bottle Settings")]
    [Tooltip("Force needed to break the bottle once it collide")]
    [SerializeField, Min(0.1f)] public float BreakingForce;
    [Tooltip("Noise created by breaking this oject, it will increase allert level of the tavern owner")]
    [SerializeField, Min(0f)] public float BreakingNoise = 0f;
    [Header("FX")]
    [SerializeField] public VFXObject Breaking_VFX;
    [SerializeField] public List<AudioClip> Breaking_SFX;

    /// <summary>
    /// Return an audioclip chosen randomly in a list
    /// </summary>
    /// <returns>AudioClip</returns>
    public AudioClip GetAudioClip()
    {
        return Breaking_SFX[UnityEngine.Random.Range(0, Breaking_SFX.Count)];
    }
}
