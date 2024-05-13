using System;
using System.Collections;
using System.Collections.Generic;
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

	[Header("Falling Settings")]//|------------------------------------------------------------------------------------------|
	[SerializeField] public LayerMask LayerPlayer;
	[SerializeField] public float GroundCheckLenght;

	[Header("Speed Settings: ")]//|------------------------------------------------------------------------------------------|
	[SerializeField] public float WalkMaxSpeed;
	[SerializeField] public float RunMaxSpeed;
	[SerializeField] public float MinSpeed;
	[SerializeField] public float ChangeDirSpeedDivident;
	[SerializeField] public float AirborneSpeed;


	[Header("Momentum Settings:")]//|------------------------------------------------------------------------------------------|

	[Tooltip("this number checks if you are changing direction DRAMATICALLY from before\n(default: 0.7 witch is equal to 45° angle)")]
	//(have you moved direction? will you move direction? when will you move direction?)
	[SerializeField] public float maxDotProduct = 0.7f;

	[SerializeField] public AnimationCurve AccelerationCurve; // how fast you XLR8

	[Tooltip("how long you take to Accelerate")]
	[SerializeField] public float AccelerationTime;
	[SerializeField] public float AccelerationAirborneTime;

	[Tooltip("how you stop moving based on decelerationTime")]
	[SerializeField] public AnimationCurve DecelerationCurve;

	[Tooltip("how long you take to decelerate")]
	[SerializeField] public float DecelerationTime;
	[Tooltip("time that it takes to save the last given input")]
	[SerializeField] public float IdleInputTime;
	//|------------------------------------------------------------------------------------------|
	[HideInInspector] public MovementStates CurrentState;
	[HideInInspector] public float MaxSpeed, LastDot, LastSpeed;
	[HideInInspector] public Vector3 MoveDir, CollisionNormal, CollisionDir;
	[HideInInspector] public Collider ClimbableObject;
	[HideInInspector] public RaycastHit Hit;
	[HideInInspector] public bool IsAirborne, isPaused, isRunning, isClimbing;
	bool CheckLedge(Vector3 pos, Vector3 dir) => Physics.Raycast(pos, dir, out Hit, RaycastDetectionLenght, ClimableLayers);
	private void Awake()
	{
		InputManager.Initialize();
		Rb = GetComponent<Rigidbody>();
		MyCollider = GetComponent<Collider>();
		MaxSpeed = WalkMaxSpeed;
		ChangeState(new MovingState());
	}

	private void OnEnable()
	{
		InputManager.inputActions.Movement.Jump.performed += OnJump;
		InputManager.inputActions.Movement.Run.performed += OnRun;
		InputManager.inputActions.Movement.Pause.performed += OnPause;
		InputManager.inputActions.UI.Pause.performed += OnPause;
	}
	private void OnDisable()
	{
		InputManager.inputActions.Movement.Jump.performed -= OnJump;
		InputManager.inputActions.Movement.Run.performed -= OnRun;
		InputManager.inputActions.Movement.Pause.performed -= OnPause;
		InputManager.inputActions.UI.Pause.performed -= OnPause;
	}

	private void FixedUpdate()
	{
		if (isClimbing == false && CheckLedge(transform.position + Vector3.up * RaycastDetectionHeight, transform.forward))
		{
			ClimbableObject = Hit.collider;
			ChangeState(new ClimbState());
		}
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
		if (Physics.SphereCast(transform.position, 0.5f, Vector3.down, out _, RaycastDetectionLenght, LayerPlayer))
			ChangeState(new FallingState());
	}

	private void OnPause(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		if (isPaused)
		{
			// unpause
			isPaused = false;
		}
		else
		{
			// pause
			isPaused = true;
		}
	}

	private void OnRun(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		if (IsAirborne || isClimbing)
			return;

		MaxSpeed = isRunning ? RunMaxSpeed : WalkMaxSpeed;
		isRunning = !isRunning;
	}

	private void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		if (IsAirborne || isClimbing)
			return;

		IsAirborne = true;
		ChangeState(new JumpState());
	}

	public void ChangeState(MovementStates newState)
	{
		if (CurrentState != null)
			CurrentState.Exit();

		CurrentState = newState;
		CurrentState.Enter(this);
	}

	public IEnumerator CheckLedgeCooldown()
	{
		isClimbing = true;
		yield return new WaitForSeconds(CooldownAfterEnd);
		isClimbing = false;
	}
}
