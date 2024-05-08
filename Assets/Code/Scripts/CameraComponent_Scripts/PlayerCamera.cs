using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private PlayerCamera_Settings m_Settings;
    [SerializeField] private Transform m_FollowTarget;
    [SerializeField] private Vector3 m_CameraOffset = Vector3.up / 2;
    
    private Vector2 m_Direction;

    private bool m_Paused = false;

    private void Update()
    {
        RotateCamera();
        transform.position = m_FollowTarget.position + m_CameraOffset;
    }

    private void RotateCamera()
    {
        Vector2 delta = InputManager.GetCameraDelta();

        m_Direction.x = Mathf.Lerp(0f, delta.y, SensitivityManager.GetSensitivityValue().x * 1 / m_Settings.SmootTime * Time.deltaTime);
        m_Direction.y = Mathf.Lerp(0f, delta.x, SensitivityManager.GetSensitivityValue().y * 1 / m_Settings.SmootTime * Time.deltaTime);

        //Update Rotation
        Quaternion cameraRotation = Quaternion.Euler(-m_Direction.x, m_Direction.y, 0);
        transform.rotation *= cameraRotation;

        //Clamp Rotation
        Vector3 newRotation = transform.rotation.eulerAngles;
        if (newRotation.x < 360 - m_Settings.MaxAngleX && newRotation.x > 180)
        {
            newRotation.x = 360 - m_Settings.MaxAngleX;
            m_Direction.x = 0f;
        }
        else if (newRotation.x > m_Settings.MaxAngleX && newRotation.x < 180)
        {
            newRotation.x = m_Settings.MaxAngleX;
            m_Direction.x = 0f;
        }

        newRotation.z = 0;

        //Apply Clamped Rotation
        transform.rotation = Quaternion.Euler(newRotation);
        //Rotate Parent
        m_FollowTarget.rotation = Quaternion.Euler(transform.rotation.eulerAngles.y * Vector3.up);
    }

    private void StopRotation()
    {
        m_Paused = !m_Paused;
        InputManager.EnabledCameraInput(!m_Paused);
    }

    private void OnEnable()
    {
        GameManager.OnGamePause += StopRotation;
    }

    private void OnDisable()
    {
        GameManager.OnGamePause -= StopRotation;
    }
}