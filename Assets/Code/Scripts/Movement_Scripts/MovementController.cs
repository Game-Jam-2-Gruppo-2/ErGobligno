using System.Collections;
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

	[Tooltip("how long is the raycast to detect the ground")]
	[SerializeField] public float GroundCheckLenght;
	[SerializeField] public float GroundCheckRadius;
	[SerializeField] public float CooldownAfterEnd;

	//|------------------------------------------------------------------------------------------|
	[Header("Speed Settings: ")]
	[SerializeField] public float WalkMaxSpeed;
	[SerializeField] public float RunMaxSpeed;
	[SerializeField] public float ChangeDirSpeedDivident;

	#region Momentum Settings:
	//|------------------------------------------------------------------------------------------|
	[Header("Momentum Settings:")]

	[Tooltip("this number checks if you are changing direction DRAMATICALLY from before\n(default: 0.7 witch is equal to 45° angle)")]
	//(have you moved direction? will you move direction? when will you move direction?)
	[SerializeField] public float maxDotProduct = 0.7f;

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
	[HideInInspector] public Vector3 MoveDir, LastDirection, BoundsGroundCheck, BoundsWallCheck, CollisionMoveDir;
	[HideInInspector] public bool IsAirBorne, isRunning, isClimbing;
	[HideInInspector] public RaycastHit Hit;
	[HideInInspector] public Collider ClimbableObject;
	[HideInInspector] public MomentumStates Momentumstate;
	[HideInInspector] public float LastDot;
	[HideInInspector] public Ray GroundRay;
	[HideInInspector] public Vector3 CollisionDir = new();


	public void ChangeMomentum(MomentumStates newState)
	{
		Momentumstate = newState;
		Momentumstate.Enter(this);
	}

	private void Awake()
	{
		//InputManager.MoveInputs(true);
		//InputManager.UiInputs(false);
		GroundRay = new(transform.position, -transform.up);
		MaxSpeed = WalkMaxSpeed;
		isRunning = false;
		BoundsGroundCheck = new Vector3(MyCollider.bounds.extents.x * 2, 0.1f, MyCollider.bounds.extents.z * 2);
		BoundsWallCheck = new Vector3(0.1f, MyCollider.bounds.extents.y * 2, MyCollider.bounds.extents.z * 2);
	}

	private void OnEnable()
	{
		InputManager.OnJump += Jump;
		InputManager.OnRun += Running;
	}

	private void OnDisable()
	{
		InputManager.OnJump -= Jump;
		InputManager.OnRun -= Running;
	}

	private void Running()
	{
		if (IsAirBorne)
			return;

		isRunning = !isRunning;
		MaxSpeed = isRunning ? RunMaxSpeed : WalkMaxSpeed;
		CurrentState.Exit(this, new MovingState());
	}

	void Start()
	{
		Rb = GetComponent<Rigidbody>();
		ChangeState(new IdleState());
	}

	private void Jump()
	{
		if (IsAirBorne || isClimbing)
			return;

		IsAirBorne = true;
		ChangeState(new JumpState());
	}

	void Update()
	{
		CurrentState.Tick(this);
	}

	bool CheckLedge(Vector3 pos, Vector3 dir) => Physics.Raycast(pos, dir, out Hit, RaycastDetectionLenght, ClimableLayers);

	//public bool CheckGround => Physics.BoxCast(transform.position, BoundsGroundCheck, -transform.up, quaternion.identity, GroundCheckLenght, GroundLayer);
	private void FixedUpdate()
	{
		if (isClimbing == false && CheckLedge(transform.position + (Vector3.up * RaycastDetectionHeight), transform.forward))
		{
			ClimbableObject = Hit.transform.GetComponent<Collider>();
			ChangeState(new ClimbState());
		}

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

	public bool CheckGround => Physics.SphereCast(GroundRay, GroundCheckRadius, GroundCheckLenght);
	private void OnCollisionExit(Collision other)
	{
		if (IsAirBorne)
			return;

		if (CheckGround == false)
			ChangeState(new FallingState());
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.contacts[0].normal.x != 0 || other.contacts[0].normal.z != 0)
		{
			CollisionMoveDir = MoveDir;
			CollisionDir = other.contacts[0].normal;
			Debug.DrawRay(other.contacts[0].point, CollisionDir * 10, Color.yellow, 999);
		}
		else
			Debug.DrawRay(other.contacts[0].point, CollisionDir * 10, Color.red, 999);

		// if the direction that i'm moving in is the opposite form the object
		if (IsAirBorne && CollisionDir.y > 0)
		{
			CurrentState.Exit(this, new IdleState());
			Debug.Log("Stop Falling");
		}
	}

	// private void OnCollisionStay(Collision other)
	// {
	// 	if (other.contacts[0].normal != CollisionDir)
	// 	{
	// 		CollisionMoveDir = MoveDir;
	// 		CollisionDir = other.contacts[0].normal;
	// 		CollisionDir.y = 0;
	// 		Debug.Log("ColDir= " + CollisionDir.normalized + "\n" + (MoveDir.normalized + CollisionDir.normalized));
	// 	}
	// }

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawRay(transform.position + (Vector3.up * RaycastDetectionHeight), transform.forward * RaycastDetectionLenght);

		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, -transform.up * GroundCheckLenght);
		Gizmos.DrawWireSphere(transform.position - transform.up * GroundCheckLenght, GroundCheckRadius);
	}
#endif
}