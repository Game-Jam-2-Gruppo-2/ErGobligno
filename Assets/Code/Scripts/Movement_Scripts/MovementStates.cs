using System;
using UnityEngine;

public class MovementStates : State
{
	[Header("settings:")]

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

	AnimationCurve usedCurve;
	float speed;
	float timer = 0, lastTimer = 0;
	float duration;
	Vector3 vel, lastDir, MoveDir;
	private void Awake()
	{
		base.Awake();
	}

	private void OnEnable()
	{
		if (InputManager.inputActions == null)
			InputManager.Initialize();
		InputManager.inputActions.Movement.Jump.performed += Jump;
	}

	private void Jump(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		controller.ChangeState(States.JUMP);

	}

	private void OnDisable()
	{
		InputManager.inputActions.Movement.Jump.performed -= Jump;
	}

	private void Update()
	{
		MoveDir = InputManager.MovementDir;
		if (controller.CheckClimb())
			controller.ChangeState(States.CLIMB);
	}
	private void FixedUpdate()
	{
		vel = Time.fixedDeltaTime * WalkMaxSpeed * MoveDir;
		vel.y = Rb.velocity.y;
		vel = vel.x * transform.right + transform.forward * vel.z;
		Rb.velocity = vel;
	}
	private void OnCollisionEnter(Collision other)
	{

	}
	private void OnCollisionExit(Collision other)
	{

	}
}
