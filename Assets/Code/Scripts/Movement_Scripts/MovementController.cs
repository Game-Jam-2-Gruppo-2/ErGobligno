using System.Collections;
using Unity.Mathematics;
using UnityEngine;
public class MovementController : MonoBehaviour
{
	[Header("Serialize:")]
	[SerializeField] public Rigidbody Rb;
	[SerializeField] public Collider MyCollider;

	//|------------------------------------------------------------------------------------------|
	[Header("Climb Settings:")]
	[SerializeField] public LayerMask ClimableLayers;
	[Tooltip("how far from the pivot in y is the raycast to detect ledges")]
	[SerializeField] public float RaycastDetectionHeight;
	[Tooltip("how long is the raycast to detect ledges")]
	[SerializeField] public float RaycastDetectionLenght;
	[SerializeField] public float ClimbDuration;

	///<summary>the offset in y after you climed</summary>
	[Tooltip("the offset in y after you climed")]
	[SerializeField] public float ClimbOffset;

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
	/// <summary>Velocity scalar (S)</summary>
	[HideInInspector] public float VelocityScalar = 0;
	[HideInInspector] public float LastSpeed;
	[HideInInspector] public float MaxSpeed;
	[HideInInspector] public Vector3 MoveDir, LastDirection, Bounds;
	[HideInInspector] public bool IsJumping, isRunning, isClimbing;
	[HideInInspector] public RaycastHit Hit;
	[HideInInspector] public Collider ClimbableObject;


	private void Awake()
	{
		InputManager.MoveInputs(true);
		InputManager.UiInputs(false);
		InputManager.Inizialized();
		MaxSpeed = WalkMaxSpeed;
		isRunning = false;
		Bounds = new Vector3(MyCollider.bounds.extents.x * 2, 0.1f, MyCollider.bounds.extents.z * 2);
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
			ClimbableObject = Hit.transform.GetComponent<Collider>();
			ChangeState(new ClimbState());
		}

		CurrentState.Tick(this);
	}

	bool CheckLedge(Vector3 pos, Vector3 dir) => Physics.Raycast(pos, dir, out Hit, RaycastDetectionLenght, ClimableLayers);

	public bool CheckGround => Physics.BoxCast(transform.position, Bounds, -transform.up, quaternion.identity, GroundCheckLenght, GroundLayer);

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

	private void OnCollisionExit(Collision other)
	{
		if (IsJumping)
			return;

		if (!CheckGround)
		{
			ChangeState(new FallingState());
		}
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawRay(transform.position + (Vector3.up * RaycastDetectionHeight), transform.forward * RaycastDetectionLenght);

		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, -transform.up * GroundCheckLenght);
		Gizmos.DrawWireCube(transform.position - transform.up * GroundCheckLenght, new Vector3(MyCollider.bounds.extents.x * 2, 0.1f, MyCollider.bounds.extents.z * 2));
	}
#endif
}