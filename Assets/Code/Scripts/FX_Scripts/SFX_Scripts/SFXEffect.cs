using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXEffect : MonoBehaviour
{
    [SerializeField] private AudioSource Source;

    private void Awake()
    {
        Source = GetComponent<AudioSource>();
    }

    public void PlaySound(Vector3 pos, AudioClip clip)
    {
        transform.position = pos;
        Source.clip = clip;
        Source.Play();
        StartCoroutine(EffectIEnum(clip.length));
    }

    public void SetPitch(float pitch)
    {
        if(Source == null)
            Source = GetComponent<AudioSource>();
        Source.pitch = pitch;
    }

    private IEnumerator EffectIEnum(float clipLenght)
    {
        yield return new WaitForSeconds(clipLenght);
        this.gameObject.SetActive(false);
    }
}
