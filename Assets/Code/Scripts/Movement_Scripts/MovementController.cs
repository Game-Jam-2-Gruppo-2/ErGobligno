using System;
using UnityEngine;

public class MovementController : MonoBehaviour
{
	[Header("Serialize:")]
	[SerializeField] public Rigidbody Rb;
	[SerializeField] public LayerMask ClimableLayers;

	[Header("Max Speed Settings: ")]
	[SerializeField] public float MaxSpeed;
	[SerializeField] public float RunMaxSpeed;
	[SerializeField] public float MaxJumpHight;

	#region Momentum Settings:
	[Header("Momentum Settings:")]

	#region Acceleration:
	[SerializeField] public AnimationCurve AccelerationCurve; // how fast you XLR8

	[Tooltip("how long you take to Accelerate")]
	[SerializeField] public float AccelerationTime;
	#endregion

	#region Deceleration:
	[Tooltip("how you stop moving based on decelerationTime")]
	[SerializeField] public AnimationCurve DecelerationCurve;

	[Tooltip("how fast you decelerate over time")]
	[SerializeField] public float DecelerationTime;
	#endregion

	#endregion

	//= the hidden variabiles 
	/*[HideInInspector]*/
	//public float MomentumCounter, MomentumTimer; // whare you are inside one of the curves
	[HideInInspector] public MovementStates CurrentState;
	[HideInInspector] public float VelocityScale;
	[HideInInspector] public Vector3 MoveDir;
	[HideInInspector] public bool IsJumping, isRunning;


	private void Awake()
	{
		InputManager.MoveInputs(true);
		InputManager.UiInputs(false);
		InputManager.Inizialize();
	}
	private void OnEnable()
	{
		InputManager.OnMovement += Move;
		InputManager.OnJump += Jump;
		InputManager.OnStopMovement += StopMovement;
	}

	private void OnDisable()
	{
		InputManager.OnMovement -= Move;
		InputManager.OnJump -= Jump;
		InputManager.OnStopMovement -= StopMovement;
	}

	private void StopMovement(Vector3 dir)
	{
		MoveDir = dir;
		Debug.LogWarning("opsy");
		ChangeState(new IdleState());
	}

	private void Jump()
	{
		IsJumping = true;
		ChangeState(new JumpState());
	}

	private void Move(Vector3 dir)
	{
		MoveDir = dir;
		ChangeState(new WalkState());
	}

	void Start()
	{
		Rb = GetComponent<Rigidbody>();
		CurrentState = new IdleState();
	}

	void Update()
	{
		CurrentState.Tick(this);
	}

	private void FixedUpdate()
	{
		CurrentState.FixedTick(this);
	}

	public void ChangeState(MovementStates newState)
	{
		CurrentState = newState;
		CurrentState.Enter(this);
	}
}