using System;
using System.Collections;
using UnityEngine;

public class MovementController : MonoBehaviour
{
	public static Action OnClimb;

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

	[Header("Speed Settings: ")]//|------------------------------------------------------------------------------------------|
	[SerializeField] public float WalkMaxSpeed;
	[SerializeField] public float RunMaxSpeed;
	[SerializeField] public float MinSpeed;
	[SerializeField] public float ChangeDirSpeedDivident;
	[SerializeField] public float AirborneSpeed;
	[SerializeField] public LayerMask MovableLayer;


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
	[HideInInspector] public Collider ClimbableCollider;
	[HideInInspector] public RaycastHit Hit;
	[HideInInspector] public bool IsAirborne, isPaused, isRunning, isClimbing;
	public static Action climb;
	public bool CheckLedge(Vector3 pos, Vector3 dir) => Physics.Raycast(pos, dir, out Hit, RaycastDetectionLenght, ClimableLayers);
	public bool GroundCheck => Physics.SphereCast(transform.position, MyCollider.bounds.extents.x, Vector3.down, out _, RaycastDetectionLenght, LayerPlayer);
	private void Awake()
	{
		Rb = GetComponent<Rigidbody>();
		MyCollider = GetComponent<Collider>();
		MaxSpeed = WalkMaxSpeed;
		ChangeState(new IdleState());
	}

	private void OnEnable()
	{
		InputManager.inputActions.Movement.Pause.performed += OnPause;
		InputManager.inputActions.UI.Pause.performed += OnPause;
	}
	private void OnDisable()
	{
		InputManager.inputActions.Movement.Pause.performed -= OnPause;
		InputManager.inputActions.UI.Pause.performed -= OnPause;
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


	public void ChangeState(MovementStates newState)
	{
		if (CurrentState != null)
		{
			if (CurrentState.GetType() == newState.GetType())
			{
				//Debug.Log("opsy.txt");
				return;
			}
		}

		if (CurrentState != null)
		{
			CurrentState.Exit();
			Debug.LogWarning("current state= " + CurrentState + "\nNewState= " + newState);
		}

		//Debug.LogWarning("current state= " + CurrentState + "\nNewState= " + newState);
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
