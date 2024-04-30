using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    [SerializeField] private Rigidbody m_Body;
    [SerializeField] private Bottle_Settings m_Settings;

    //force = velocity^2 * mass/2;

    private void OnCollisionEnter(Collision collision)
    {
       if(collision.impulse.magnitude > m_Settings.BreakingForce)
            Break();
    }

    private void Break()
    {
        VFXManager.RequestVFX?.Invoke(m_Settings.Breaking_VFX, transform.position, transform.rotation);
        AudioManager.Request3DSFX?.Invoke(m_Settings.GetAudioClip(), transform.position);
        this.gameObject.SetActive(false);
    }
}

/*
 //Force
        float f;
        //Squared Velocity
        float v_sqrt = m_Body.velocity.magnitude * m_Body.velocity.magnitude;
        //Force Value
        f = v_sqrt * (m_Body.mass/2);

        if (f < 0.5f)
            return;
*/
