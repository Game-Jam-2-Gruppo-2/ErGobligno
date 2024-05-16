using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bottle : MonoBehaviour
{
    [SerializeField] private Rigidbody m_Body;
    [SerializeField] private Bottle_Settings m_Settings;

    private int ContactCounter = 0;
    private List<ContactPoint> ContactPoints = new List<ContactPoint>();

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
            return;

        //Breaking Case
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
    }

    private void OnCollisionStay(Collision collision)
    {
        //Stop if colliding with player
        if (collision.collider.IsPlayer())
            return;

        //Stop if colliding with bottle
        if (collision.gameObject.TryGetComponent<Bottle>(out Bottle CollidingBottle))
            return;

        //Get Contact Points
        ContactPoints = collision.contacts.ToList();

        //Tipping Case
        if (ContactPoints.Count > ContactCounter)
            AudioManager.Request3DSFX?.Invoke(m_Settings.Tipping_SFX, transform.position, m_Settings.GetTippingPitch());

        //Update Contact Counter
        ContactCounter = ContactPoints.Count;
        Debug.Log(ContactCounter + " collisions with: "+collision.gameObject.name);
    }

    private void Break()
    {
        VFXManager.RequestVFX?.Invoke(m_Settings.Breaking_VFX, transform.position, transform.rotation);
        AudioManager.Request3DSFX?.Invoke(m_Settings.GetBreakingAudioClip(), transform.position, 1f);
        ScoreManager.IncreaseNoise?.Invoke(m_Settings.BreakingNoise);
        this.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        if(ContactPoints.Count > 0)
        {
            Gizmos.color = Color.yellow;
            for(int i = 0; i< ContactPoints.Count; i++)
                Gizmos.DrawSphere(ContactPoints[i].point, 0.05f);
        }
    }
}

public static class CollisionExtention
{
    public static bool IsPlayer(this Collider collider)
    {
        return collider.gameObject.layer == 6;
    }
}