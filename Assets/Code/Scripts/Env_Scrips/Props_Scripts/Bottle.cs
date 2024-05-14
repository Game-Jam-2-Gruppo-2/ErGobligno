using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    [SerializeField] private Rigidbody m_Body;
    [SerializeField] private Bottle_Settings m_Settings;

    private void OnCollisionEnter(Collision collision)
    {
        //Brekaking Case
        if (collision.impulse.magnitude > m_Settings.BreakingForce)
            Break();
        //Tipping Case
        else
        {
            if (collision.gameObject.TryGetComponent<Bottle>(out Bottle CollidingBottle))
                AudioManager.Request3DSFX?.Invoke(m_Settings.BottleHit_SFX, transform.position, m_Settings.GetBottleHitPitch());
            else
                AudioManager.Request3DSFX?.Invoke(m_Settings.Tipping_SFX, transform.position, m_Settings.GetTippingPitch());
        }

        Debug.Log("Colliding with: "+collision.gameObject.name);
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Exiting with: " + collision.gameObject.name);
    }

    private void Break()
    {
        VFXManager.RequestVFX?.Invoke(m_Settings.Breaking_VFX, transform.position, transform.rotation);
        AudioManager.Request3DSFX?.Invoke(m_Settings.GetBreakingAudioClip(), transform.position, 1f);
        ScoreManager.IncreaseNoise?.Invoke(m_Settings.BreakingNoise);
        this.gameObject.SetActive(false);
    }
}