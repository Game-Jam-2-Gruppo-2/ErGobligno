using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
	// 	[HideInInspector] public Rigidbody Rb;
	// 	[HideInInspector] public Collider MyCollider;
	// 	//|------------------------------------------------------------------------------------------|

	// 	//|------------------------------------------------------------------------------------------|

	// 	[Header("Falling Settings")]//|------------------------------------------------------------------------------------------|
	// 	[SerializeField] public LayerMask LayerPlayer;
	// 	[SerializeField] public float GroundCheckLenght;
	[SerializeField] float ClimbCheckLenght;
	[SerializeField] float ClimbCheckHight;
	[SerializeField] LayerMask ClimbLayer;
	[HideInInspector] public RaycastHit HitObject;

	ClimbState climbState;
	JumpState jumpState;
	MovementStates movementState;
	IdleState idleState;

	State CurrentState;
	private void Awake()
	{
		idleState = GetComponent<IdleState>();
		movementState = GetComponent<MovementStates>();
		jumpState = GetComponent<JumpState>();
		climbState = GetComponent<ClimbState>();
		ChangeState(States.IDLE);
	}

	public void ChangeState(States state)
	{
		switch (state)
		{
			case States.IDLE:
				movementState.enabled = false;
				jumpState.enabled = false;
				climbState.enabled = false;
				idleState.enabled = true;
				break;

			case States.JUMP:
				movementState.enabled = false;
				jumpState.enabled = true;
				climbState.enabled = false;
				idleState.enabled = false;
				break;

			case States.MOVE:
				movementState.enabled = true;
				jumpState.enabled = false;
				climbState.enabled = false;
				idleState.enabled = false;
				break;

			case States.CLIMB:
				movementState.enabled = false;
				jumpState.enabled = false;
				climbState.enabled = true;
				idleState.enabled = false;
				break;

			default:
				movementState.enabled = false;
				jumpState.enabled = false;
				climbState.enabled = false;
				idleState.enabled = true;
				break;
		}
		Debug.Log(state.ToString());
	}
	public bool CheckClimb() => Physics.Raycast(transform.position + Vector3.up * ClimbCheckHight, transform.forward, out HitObject, ClimbCheckLenght, ClimbLayer);

	// 	//|------------------------------------------------------------------------------------------|
	// 	[HideInInspector] public State CurrentState;
	// 	[HideInInspector] public float MaxSpeed, LastDot, LastSpeed;
	// 	[HideInInspector] public Vector3 MoveDir, CollisionNormal, CollisionDir;
	// 	[HideInInspector] public Collider ClimbableObject;
	// 	[HideInInspector] public RaycastHit Hit;
	// 	[HideInInspector] public bool IsAirborne, isPaused, isRunning, isClimbing;
	// 	bool CheckLedge(Vector3 pos, Vector3 dir) => Physics.Raycast(pos, dir, out Hit, RaycastDetectionLenght, ClimableLayers);
	// 	private void Awake()
	// 	{
	// 		InputManager.Initialize();
	// 		Rb = GetComponent<Rigidbody>();
	// 		MyCollider = GetComponent<Collider>();
	// 		MaxSpeed = WalkMaxSpeed;
	// 		ChangeState(new MovingState());
	// 	}

	// 	private void OnEnable()
	// 	{
	// 		InputManager.inputActions.Movement.Jump.performed += OnJump;
	// 		InputManager.inputActions.Movement.Run.performed += OnRun;
	// 		InputManager.inputActions.Movement.Pause.performed += OnPause;
	// 		InputManager.inputActions.UI.Pause.performed += OnPause;
	// 	}
	// 	private void OnDisable()
	// 	{
	// 		InputManager.inputActions.Movement.Jump.performed -= OnJump;
	// 		InputManager.inputActions.Movement.Run.performed -= OnRun;
	// 		InputManager.inputActions.Movement.Pause.performed -= OnPause;
	// 		InputManager.inputActions.UI.Pause.performed -= OnPause;
	// 	}

	// 	private void FixedUpdate()
	// 	{
	// 		if (isClimbing == false && CheckLedge(transform.position + Vector3.up * RaycastDetectionHeight, transform.forward))
	// 		{
	// 			ClimbableObject = Hit.collider;
	// 			ChangeState(new ClimbState());
	// 		}
	// 		CurrentState.FixedTick();
	// 	}
	// 	void Update()
	// 	{
	// 		CurrentState.Tick();
	// 	}

	// 	private void OnTriggerEnter(Collider other)
	// 	{
	// 		if (other.transform.TryGetComponent(out ICollectible collectable))
	// 		{
	// 			collectable.Collect();
	// 		}
	// 	}
	// 	private void OnCollisionEnter(Collision other)
	// 	{
	// 		CurrentState.Collision(other);
	// 	}
	// 	private void OnCollisionExit(Collision other)
	// 	{
	// 		CurrentState.CollisionExit(other);
	// 		if (Physics.SphereCast(transform.position, 0.5f, Vector3.down, out _, RaycastDetectionLenght, LayerPlayer))
	// 			ChangeState(new FallingState());
	// 	}

	// 	private void OnPause(UnityEngine.InputSystem.InputAction.CallbackContext context)
	// 	{
	// 		if (isPaused)
	// 		{
	// 			// unpause
	// 			isPaused = false;
	// 		}
	// 		else
	// 		{
	// 			// pause
	// 			isPaused = true;
	// 		}
	// 	}

	// 	private void OnRun(UnityEngine.InputSystem.InputAction.CallbackContext context)
	// 	{
	// 		if (IsAirborne || isClimbing)
	// 			return;

	// 		MaxSpeed = isRunning ? RunMaxSpeed : WalkMaxSpeed;
	// 		isRunning = !isRunning;
	// 	}

	// 	private void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext context)
	// 	{
	// 		if (IsAirborne || isClimbing)
	// 			return;

	// 		IsAirborne = true;
	// 		ChangeState(new JumpState());
	// 	}

	// 	public void ChangeState(State newState)
	// 	{
	// 		if (CurrentState != null)
	// 			CurrentState.Exit();

	// 		CurrentState = newState;
	// 		CurrentState.Enter(this);
	// 	}

	// 	public IEnumerator CheckLedgeCooldown()
	// 	{
	// 		isClimbing = true;
	// 		yield return new WaitForSeconds(CooldownAfterEnd);
	// 		isClimbing = false;
	// 	}
}
public enum States
{
	IDLE,
	JUMP,
	MOVE,
	CLIMB
}