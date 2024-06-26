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
    [SerializeField] private List<AudioClip> Breaking_SFX;
    [Header("Tipping FX")]
    [Header("Tipping On side FX")]
    [SerializeField] private List<AudioClip> SideTipping_SFX;
    [SerializeField, Range(0f, 1f)] private float PitchVariation_SideTipping = 0f;
    [Header("Tipping on bottom FX")]
    [SerializeField] private List<AudioClip> BottomTipping_SFX;
    [SerializeField, Range(0f, 1f)] private float PitchVariation_BottomTipping = 0f;
    [Header("Hitting Another Bottle FX")]
    [SerializeField] private List<AudioClip> GlassHit_SFX;
    [SerializeField, Range(0f, 1f)] private float PitchVariation_BottleHit = 0f;
    
    /// <summary>
    /// Return an audioclip chosen randomly in the braking AudioClip list
    /// </summary>
    /// <returns>AudioClip</returns>
    public AudioClip GetBreakingAudioClip()
    {
        return Breaking_SFX[UnityEngine.Random.Range(0, Breaking_SFX.Count)];
    }

    public AudioClip GetSideTippingClip()
    {
        return SideTipping_SFX[UnityEngine.Random.Range(0, SideTipping_SFX.Count)];
    }

    public AudioClip GetBottomTippingClip()
    {
        return BottomTipping_SFX[UnityEngine.Random.Range(0, BottomTipping_SFX.Count)];
    }

    public AudioClip GetGlassHitClip()
    {
        return GlassHit_SFX[UnityEngine.Random.Range(0, GlassHit_SFX.Count)];
    }

    public float GetSideTippingPitch()
    {
        return 1+UnityEngine.Random.Range(-PitchVariation_SideTipping, PitchVariation_SideTipping);
    }

    public float GetBottomTippingPitch()
    {
        return 1 + UnityEngine.Random.Range(-PitchVariation_BottomTipping, PitchVariation_BottomTipping);
    }

    public float GetGlassHitPitch()
    {
        return 1 + UnityEngine.Random.Range(-PitchVariation_BottleHit, PitchVariation_BottleHit);
    }
}