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
    [Header("Breaking FX")]
    [SerializeField] public ParticleSystem Breaking_VFX;
    [SerializeField] public List<AudioClip> Breaking_SFX;
    [Header("Tipping FX")]
    [SerializeField] public AudioClip Tipping_SFX;
    [SerializeField, Range(0f, 1f)] private float PitchVariation_Tipping = 0f;
    [Header("Hitting Another Bottle FX")]
    [SerializeField] public AudioClip BottleHit_SFX;
    [SerializeField, Range(0f, 1f)] private float PitchVariation_BottleHit = 0f;

    /// <summary>
    /// Return an audioclip chosen randomly in a list
    /// </summary>
    /// <returns>AudioClip</returns>
    public AudioClip GetBreakingAudioClip()
    {
        return Breaking_SFX[UnityEngine.Random.Range(0, Breaking_SFX.Count)];
    }

    public float GetTippingPitch()
    {
        return 1+UnityEngine.Random.Range(-PitchVariation_Tipping, PitchVariation_Tipping);
    }

    public float GetBottleHitPitch()
    {
        return 1 + UnityEngine.Random.Range(-PitchVariation_BottleHit, PitchVariation_BottleHit);
    }

}
