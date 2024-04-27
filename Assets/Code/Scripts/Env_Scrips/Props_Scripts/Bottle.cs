using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    [SerializeField] private Rigidbody m_Body;
    [SerializeField] private Bottle_Settings m_Settings;

    //force = velocity^2 * mass/2;

    private void Awake()
    {

    }

    private void Break()
    {
        VFXManager.RequestVFX?.Invoke(m_Settings.Breaking_VFX, transform.position, transform.rotation);
        this.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
       if(collision.impulse.magnitude > m_Settings.BreakingForce)
            Break();

        Debug.Log("Impulse Value = "+collision.impulse.magnitude+" to Break = "+ m_Settings.BreakingForce);
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
