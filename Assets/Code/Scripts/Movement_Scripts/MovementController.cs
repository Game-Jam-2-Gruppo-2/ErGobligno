using System.Collections;
using UnityEngine;

public class MovementController : MonoBehaviour
{
	[Header("Serialize:")]
	[SerializeField] public Rigidbody Rb;

	//|------------------------------------------------------------------------------------------|
	[Header("Climb Settings:")]
	[SerializeField] public LayerMask ClimableLayers;
	[Tooltip("how far from the center in y is the raycast to detect ledges")]
	[SerializeField] public float RaycastDetectionHeight;
	[Tooltip("how long is the raycast to detect ledges")]
	[SerializeField] public float RaycastDetectionLenght;
	[SerializeField] public float CheckCooldown = 0.3f;

	//|------------------------------------------------------------------------------------------|
	[Header("Jump Settings:")]
	[SerializeField] public float JumpForce;
	[SerializeField] public LayerMask GroundLayer;

	[Tooltip("how long is the raycast to detect the ground")]
	[SerializeField] public float GroundCheckLenght;
	[SerializeField] public float CooldownAfterEnd;

	//|------------------------------------------------------------------------------------------|
	[Header("Speed Settings: ")]
	[SerializeField] public float WalkMaxSpeed;
	[SerializeField] public float RunMaxSpeed;

	#region Momentum Settings:
	//|------------------------------------------------------------------------------------------|
	[Header("Momentum Settings:")]

	#region Acceleration:
	[SerializeField] public AnimationCurve AccelerationCurve; // how fast you XLR8

	[Tooltip("how long you take to Accelerate")]
	[SerializeField] public float AccelerationTime;
	#endregion
	//|------------------------------------------------------------------------------------------|
	#region Deceleration:
	[Tooltip("how you stop moving based on decelerationTime")]
	[SerializeField] public AnimationCurve DecelerationCurve;

	[Tooltip("how long you take to decelerate")]
	[SerializeField] public float DecelerationTime;
	#endregion

	#endregion

	//= the hidden variabiles 
	/*[HideInInspector]*/
	//public float MomentumCounter, MomentumTimer; // whare you are inside one of the curves
	[HideInInspector] public MovementStates CurrentState;
	/// <summary>
	/// Velocity scalar (S)
	/// </summary>
	[HideInInspector] public float VelocityScalar = 0;
	[HideInInspector] public float LastSpeed;
	[HideInInspector] public float MaxSpeed;
	[HideInInspector] public Vector3 MoveDir, LastDirection;
	[HideInInspector] public bool IsJumping, isRunning, isClimbing;
	[HideInInspector] public Collider ClimbableObject;

	private void Awake()
	{
		InputManager.MoveInputs(true);
		InputManager.UiInputs(false);
		InputManager.Inizialized();
		MaxSpeed = WalkMaxSpeed;
		isRunning = false;
	}

	private void OnEnable()
	{
		InputManager.OnJump += Jump;
		InputManager.OnRun += Running;
	}

	private void Running()
	{
		if (IsJumping)
			return;

		isRunning = !isRunning;
		MaxSpeed = isRunning ? RunMaxSpeed : WalkMaxSpeed;
		if (isRunning)
			ChangeState(new RunState());
		else
			ChangeState(new WalkState());
	}

	private void OnDisable()
	{
		InputManager.OnJump -= Jump;
		InputManager.OnRun -= Running;
	}

	void Start()
	{
		Rb = GetComponent<Rigidbody>();
		ChangeState(new IdleState());
	}

	private void Jump()
	{
		IsJumping = true;
		ChangeState(new JumpState());
	}

	void Update()
	{
		if (isClimbing == false && CheckLedge(transform.position + (Vector3.up * RaycastDetectionHeight), transform.forward))
		{
			ChangeState(new ClimbState());
		}

		CurrentState.Tick(this);
	}

	bool CheckLedge(Vector3 pos, Vector3 dir) => Physics.Raycast(pos, dir, RaycastDetectionLenght, ClimableLayers);

	private void FixedUpdate()
	{
		CurrentState.FixedTick(this);
	}

	public void ChangeState(MovementStates newState)
	{
		CurrentState = newState;
		CurrentState.Enter(this);
	}

	public IEnumerator CheckLedgeCooldown()
	{
		isClimbing = true;
		yield return new WaitForSeconds(CooldownAfterEnd);
		isClimbing = false;
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawRay(transform.position + Vector3.up * RaycastDetectionHeight, transform.forward * RaycastDetectionLenght);

		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, -transform.up * GroundCheckLenght);
	}
#endif
}