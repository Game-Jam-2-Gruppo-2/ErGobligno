using Cinemachine;
using UnityEngine;

public class CinemachinePOVExtention : CinemachineExtension
{
    [SerializeField] private float m_ClampAngle = 70f;
    [SerializeField] private Transform m_Target;

    private Vector3 m_StartingRotation;
    bool m_ForFix = false;

    
    protected override void Awake()
    {
        base.Awake();
       
    }

    private void Start()
    {
        m_StartingRotation = transform.eulerAngles;
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (SensitivityManager.Instance == null)
            return;

        if (vcam.Follow)
        {
            if(stage == CinemachineCore.Stage.Aim)
            {
                Vector2 deltaInput = InputManager.GetCameraDelta();
                m_StartingRotation.x += deltaInput.x * SensitivityManager.GetSensitivityValue().x * Time.deltaTime;
                m_StartingRotation.y += deltaInput.y * SensitivityManager.GetSensitivityValue().y * Time.deltaTime;
                m_StartingRotation.y = Mathf.Clamp(m_StartingRotation.y, -m_ClampAngle, m_ClampAngle);
                state.RawOrientation = Quaternion.Euler(-m_StartingRotation.y, m_StartingRotation.x + 180, 0f);
                m_Target.rotation = Quaternion.Euler(0f, m_StartingRotation.x + 180, 0f);
            }
        }
    }
}
