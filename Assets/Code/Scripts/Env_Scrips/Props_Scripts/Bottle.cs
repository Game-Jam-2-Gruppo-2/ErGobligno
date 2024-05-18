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
        if (collision.collider.IsPlayer())
            return;

        //Breaking Case
        if (collision.impulse.magnitude > m_Settings.BreakingForce)
            Break();
    }
    
    private void OnCollisionStay(Collision collision)
    {
        //Stop if colliding with player
        if (collision.collider.IsPlayer())
            return;
        //Discard Collision if velocity is less than a threshold
        if (m_Body.velocity.magnitude < 0.5f)
            return;

        //Get Contact Points
        ContactPoints = collision.contacts.ToList();

        //Stop if colliding with bottle
        if (collision.gameObject.TryGetComponent<Bottle>(out Bottle CollidingBottle))
        {
            AudioManager.Request3DSFX?.Invoke(m_Settings.GetGlassHitClip(), transform.position, m_Settings.GetGlassHitPitch());
            return;
        }

        //Tipping Case
        if (ContactPoints.Count > ContactCounter && m_Body.angularVelocity.magnitude > 0.25f)
        {
            if(Mathf.Abs(collision.GetAverageNormal().x) > 0.5f)
                AudioManager.Request3DSFX?.Invoke(m_Settings.GetSideTippingClip(), transform.position, m_Settings.GetSideTippingPitch());
            else
                AudioManager.Request3DSFX?.Invoke(m_Settings.GetBottomTippingClip(), transform.position, m_Settings.GetBottomTippingPitch());
        }
        
        //Update Contact Counter
        ContactCounter = ContactPoints.Count;
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

    public static Vector3 GetAverageNormal(this Collision collision)
    {
        Vector3 sum = Vector3.zero;
        for(int i = 0; i<collision.contacts.Length; i++)
        {
            sum += collision.contacts[i].point;
        }
        return sum/ collision.contacts.Length;
    }
}