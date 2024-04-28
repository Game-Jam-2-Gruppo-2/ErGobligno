using UnityEngine;

public class MovementController : MonoBehaviour
{
	[Header("Serialize:")]
	[SerializeField] public Rigidbody Rb;
	[SerializeField] public LayerMask ClimableLayers;

	[Header("Max Speed Settings: ")]
	[SerializeField] public float MaxSpeed;
	[SerializeField] public float MaxJumpHight;

	#region Momentum Settings:
	[Header("Momentum Settings:")]

	#region Acceleration:
	[SerializeField] public AnimationCurve AccelerationCurve; // how fast you XLR8

	[Tooltip("how fast you Accelerate")]
	[SerializeField] public float AccelerationSpeed;
	#endregion

	#region Deceleration:
	[Tooltip("how you stop moving based on decelerationSpeed")]
	[SerializeField] public AnimationCurve DecelerationCurve;

	[Tooltip("how fast you decelerate over time")]
	[SerializeField] public float DecelerationSpeed;
	#endregion

	#endregion

	//= the hidden variabiles 
	[HideInInspector] public float MomentumCounter; // whare you are inside one of the curves
	[HideInInspector] public MovementStates CurrentState;
	[HideInInspector] public float MoveX, MoveZ, VelocityScale;

	void Start()
	{
		Rb = GetComponent<Rigidbody>();
		CurrentState = new WalkState();

		var startTime = Time.time;
		var x = AccelerationCurve.Evaluate(Time.time);
		if (Time.time - startTime <= 0)
		{
			Rb.velocity = MaxSpeed * x * Rb.velocity;
		}
		else
		{
			MomentumCounter += Time.deltaTime;
		}

		var duration = 4f;
		x = AccelerationCurve.Evaluate(MomentumCounter);
		if (MomentumCounter < duration)
		{
			MomentumCounter += Time.deltaTime;
			Rb.velocity = MaxSpeed * x * Rb.velocity;


			// if (no imput)
			// {
			// 	Exit();
			// }
		}
		else
		{
			MomentumCounter = 0;
		}
	}

	void Update()
	{
		CurrentState.Tick(this);
	}

	public void ChangeState(MovementStates newState)
	{
		CurrentState = newState;
	}
}