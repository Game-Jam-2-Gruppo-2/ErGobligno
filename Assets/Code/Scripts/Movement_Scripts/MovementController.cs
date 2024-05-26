using System;
using UnityEngine;

public class MovementController : MonoBehaviour
{
	[HideInInspector] public Rigidbody Rb;
	[HideInInspector] public Collider MyCollider;

	//|------------------------------------------------------------------------------------------|
	[Header("Climb Settings:")]
	[Tooltip("how far from the pivot in y is the raycast to detect ledges")]
	[SerializeField] public float LedgeCheckHeight = 1f;
	[Tooltip("how long is the raycast to detect ledges")]
	[SerializeField] public float LedgeCheckLenght = 0.6f;
	[SerializeField] public float LedgeCheckRadius = 0.4f;
	[SerializeField] public float ClimbDuration = 0.5f;
	///<summary>the offset in y after you climed</summary>
	[Tooltip("the offset in y after you climed")]
	[SerializeField] public float ClimbOffsetY = 0, ClimbOffsetZ = 0.6f;
	[SerializeField] public AudioClip Climb_SFX;
	[SerializeField, Range(-1f, 1f)] public float ClimbPitchVariation = 0.2f;

	//|------------------------------------------------------------------------------------------|
	[Header("Jump Settings:")]
	[SerializeField] public float JumpForce = 4f;
	[SerializeField] public AudioClip Jump_SFX;
	[SerializeField, Range(-1f, 1f)] public float JumpPitchVariation = 0.2f;
	[Tooltip("it's the time before i start applaying gravity (this is so that )")]
	[SerializeField] public float AirTimeBeforeGravity = 0.4f;

	[Header("Falling Settings")]//|------------------------------------------------------------------------------------------|
	[SerializeField] public LayerMask LayerPlayer;
	[Tooltip("default = 0.4f")]
	[SerializeField] public float GroundCheckRadius = 0.4f;
	[SerializeField] public float WallCheckHight = 1f;
	[SerializeField] public float WallCheckLenght = 0.6f;
	[SerializeField] public float Gravity = 9.8f;
	[SerializeField] public float GravityMaxDuration;
	[HideInInspector] public float GravityTimer;
	[HideInInspector] public float OldMaxSpeed;

	[Header("Speed Settings: ")]//|------------------------------------------------------------------------------------------|
	[SerializeField] public float WalkMaxSpeed = 10;
	[SerializeField] public float RunMaxSpeed = 15;
	[SerializeField] public LayerMask MovableLayer;
	[SerializeField] public float ImpulseForce = 60;
	//|------------------------------------------------------------------------------------------|
	// general stuff:
	[SerializeField, Range(-1f, 2f)] public float StartingPitch = 1.2f;
	[HideInInspector] public MovementStates CurrentState;
	[HideInInspector] public Vector3 MoveDir, CollisionNormal, CollisionDir;
	[HideInInspector] public Collider ClimbableCollider;
	[HideInInspector] public RaycastHit Hit;
	[HideInInspector] public PlayerInputs inputActions;
	[HideInInspector] public bool IsAirborne, IsClimbing;


	public static Action OnClimb;
	public bool CheckLedge => Physics.SphereCast(transform.position + Vector3.up * LedgeCheckHeight, LedgeCheckRadius, transform.forward, out Hit, LedgeCheckLenght);
	/// <summary>
	/// return Physics.OverlapSphere(transform.position, GroundCheckRadius, ~LayerPlayer) == null;
	/// </summary>
	public bool AirborneCheck => Physics.CheckSphere(transform.position, GroundCheckRadius, ~LayerPlayer) == false;

	private void Awake()
	{
		Rb = GetComponent<Rigidbody>();
		MyCollider = GetComponent<Collider>();
		inputActions = InputManager.inputActions;
		ChangeState(new IdleState());
	}

	private void FixedUpdate()
	{
		CurrentState?.FixedTick();
	}
	void Update()
	{
		CurrentState?.Tick();
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
		CurrentState?.Collision(other);
	}

	public void ChangeState(MovementStates newState)
	{
		CurrentState?.Exit();
		CurrentState = newState;
		CurrentState.Enter(this);
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;

		Gizmos.DrawWireSphere(transform.position, GroundCheckRadius);

		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position + Vector3.up * LedgeCheckHeight, transform.forward * LedgeCheckLenght);
		Gizmos.DrawWireSphere(transform.position + Vector3.up * LedgeCheckHeight + transform.forward * LedgeCheckLenght, LedgeCheckRadius);

		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(transform.position + Vector3.up * WallCheckHight + transform.forward * WallCheckLenght, 0.2f);
		Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.3f + transform.forward * WallCheckLenght, 0.2f);
		Gizmos.DrawRay(transform.position + Vector3.up * WallCheckHight, transform.forward * WallCheckLenght);
		Gizmos.DrawRay(transform.position + Vector3.up * 0.3f, transform.forward * WallCheckLenght);
	}
#endif
}
