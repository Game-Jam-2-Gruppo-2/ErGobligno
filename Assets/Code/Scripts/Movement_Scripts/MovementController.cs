using System;
using System.Collections;
using UnityEngine;

public class MovementController : MonoBehaviour
{
	[HideInInspector] public Rigidbody Rb;
	[HideInInspector] public Collider MyCollider;

	//|------------------------------------------------------------------------------------------|
	[Header("Climb Settings:")]
	[SerializeField] public LayerMask ClimableLayers;
	[Tooltip("how far from the pivot in y is the raycast to detect ledges")]
	[SerializeField] public float RaycastDetectionHeight;
	[Tooltip("how long is the raycast to detect ledges")]
	[SerializeField] public float RaycastDetectionLenght;
	[SerializeField] public float ClimbDuration;
	[SerializeField] public float CooldownAfterEnd;
	///<summary>the offset in y after you climed</summary>
	[Tooltip("the offset in y after you climed")]
	[SerializeField] public float ClimbOffsetY, ClimbOffsetZ;

	//|------------------------------------------------------------------------------------------|
	[Header("Jump Settings:")]
	[SerializeField] public float JumpForce;
	[SerializeField] public AudioClip Jump_SFX;
	[SerializeField, Range(0f, 1f)] public float PitchVariation;

	[Header("Falling Settings")]//|------------------------------------------------------------------------------------------|
	[SerializeField] public LayerMask LayerPlayer;
	[SerializeField] public float GroundCheckLenght;
	[SerializeField] public float GroundCheckHight;
	[SerializeField] public float GroundCheckRadius;

	[Header("Speed Settings: ")]//|------------------------------------------------------------------------------------------|
	[SerializeField] public float WalkMaxSpeed;
	[SerializeField] public float RunMaxSpeed;
	[SerializeField] public float AirborneSpeed;
	[SerializeField] public LayerMask MovableLayer;
	[SerializeField] public float IMpulseForce;
	//|------------------------------------------------------------------------------------------|
	[HideInInspector] public MovementStates CurrentState;
	[HideInInspector] public Vector3 MoveDir, CollisionNormal, CollisionDir;
	[HideInInspector] public Collider ClimbableCollider;
	[HideInInspector] public RaycastHit Hit;
	[HideInInspector] public PlayerInputs inputActions;
	public static Action OnClimb;
	public bool CheckLedge => Physics.Raycast(transform.position + Vector3.up * RaycastDetectionHeight, transform.forward, out Hit, RaycastDetectionLenght, ClimableLayers);
	public bool GroundCheck => Physics.SphereCast(transform.position + Vector3.up * GroundCheckHight, GroundCheckRadius, Vector3.down, out _, RaycastDetectionLenght, ~LayerPlayer);
	private void Awake()
	{
		Rb = GetComponent<Rigidbody>();
		MyCollider = GetComponent<Collider>();
		inputActions = InputManager.inputActions;
		ChangeState(new IdleState());
	}

	private void FixedUpdate()
	{
		CurrentState.FixedTick();
	}
	void Update()
	{
		CurrentState.Tick();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.TryGetComponent(out ICollectible collectable))
		{
			collectable.Collect();
		}
	}
	private void OnCollisionEnter(Collision other)
	{
		CurrentState.Collision(other);
	}
	private void OnCollisionExit(Collision other)
	{
		CurrentState.CollisionExit(other);
	}

	public void ChangeState(MovementStates newState)
	{
		if (CurrentState != null)
		{
			CurrentState.Exit();
			Debug.LogWarning("current state= " + CurrentState + "\nNewState= " + newState);
		}


		//Debug.LogWarning("current state= " + CurrentState + "\nNewState= " + newState);
		CurrentState = newState;
		CurrentState.Enter(this);
	}
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawRay(transform.position + (transform.up * GroundCheckHight), -transform.up * GroundCheckLenght);
		Gizmos.DrawWireSphere(transform.position + (transform.up * GroundCheckHight) - (transform.up * GroundCheckLenght), GroundCheckRadius);

		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position + Vector3.up * RaycastDetectionHeight, transform.forward * RaycastDetectionLenght);
	}
#endif
}
