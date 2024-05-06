using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private PlayerCamera_Settings m_Settings;
    private CameraActions m_Actions;
    private Camera m_Camera;

    private Vector2 m_Speed;
    private Vector2 m_Direction;

    private bool m_Paused = false;

    private void Awake()
    {
        //Set up Camera
        m_Camera = GetComponent<Camera>();
        //Set Up Actions
        m_Actions = new CameraActions();
        m_Actions.Enable();

        m_Direction = Vector2.zero;
    }

    private void Update()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        float deltaX = m_Actions.CameraMovement.Direction_X.ReadValue<float>();
        float deltaY = m_Actions.CameraMovement.Direction_Y.ReadValue<float>();

        m_Direction.x = Mathf.Lerp(0f, deltaY, SensitivityManager.SensitivityValue().x * 1/m_Settings.SmootTime * Time.deltaTime);
        m_Direction.y = Mathf.Lerp(0f, deltaX, SensitivityManager.SensitivityValue().y * 1/m_Settings.SmootTime * Time.deltaTime);

        //Update Rotation
        Quaternion rotation = Quaternion.Euler(-m_Direction.x, m_Direction.y, 0);
        transform.rotation *= rotation;
        //Clamp Rotation
        Vector3 newRotation = transform.rotation.eulerAngles;

        if(newRotation.x < 360 - m_Settings.MaxAngleX && newRotation.x > 180)
        {
            newRotation.x = 360 - m_Settings.MaxAngleX;
            m_Direction.x = 0f;
            m_Speed.x = 0f;
        }
        else if (newRotation.x > m_Settings.MaxAngleX && newRotation.x < 180)
        {
            newRotation.x = m_Settings.MaxAngleX;
            m_Direction.x = 0f;
            m_Speed.x = 0f;
        }

        newRotation.z = 0;
        
        //Apply Clamped Rotation
        m_Camera.transform.rotation = Quaternion.Euler(newRotation);
    }

    private void ResumeRotation()
    {
        m_Actions.Enable();
    }

    private void StopRotation()
    {
        m_Actions.Disable();

        if(m_Paused)
            ResumeRotation();

        m_Paused = !m_Paused;
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