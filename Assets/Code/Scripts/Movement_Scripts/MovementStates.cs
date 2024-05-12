using System.Threading;
using UnityEngine;

public abstract class MovementStates
{
	public abstract MovementController Controller { get; set; }
	public abstract void Enter(MovementController controller);
	public abstract void Tick();
	public abstract void FixedTick();
	public abstract void Exit();
	public abstract void Collision(Collision other);
	public abstract void CollisionExit(Collision other);
}

public class MovingState : MovementStates
{
	public override MovementController Controller { get; set; }
	AnimationCurve usedCurve;
	float speed;
	float minSpeed, maxSpeed;
	float timer = 0, lastTimer = 0;
	float timerIdle = 0, lastInputDuration;
	float duration;
	Vector3 vel, lastDir;
	bool HasToFinish;

	public enum MomentumType
	{
		Airborne = 0,
		Idle = 1,
		RunToWalk = 2,
		Turn = 3,
		Accelleration = 4
	}
	MomentumType currentMomentum, lastMomentum;

	#region shortcuts
	/// <summary>
	/// usedCurve.Evaluate(timer)
	/// </summary>
	float CurveValue => usedCurve.Evaluate(timer);

	/// <summary>
	/// Mathf.Lerp(minSpeed, maxSpeed, CurveValue)
	/// </summary>
	float GetSpeed => Mathf.Lerp(minSpeed, maxSpeed, CurveValue);

	/// <summary>
	/// get => Controller.MoveDir; 
	/// set => Controller.MoveDir = value;
	/// </summary>
	Vector3 GetMoveDir { get => Controller.MoveDir; set => Controller.MoveDir = value; }

	/// <summary>
	/// InputManager.MovementDir
	/// </summary>
	Vector3 GetInputDir => InputManager.MovementDir;

	/// <summary>
	/// Controller.LastSpeed
	/// </summary>
	float GetLastSpeed => Controller.LastSpeed;

	/// <summary>
	/// Controller.MaxSpeed
	/// </summary>
	float GetMaxSpeed => Controller.MaxSpeed;

	/// <summary>
	/// get => Controller.LastDot;
	/// set => Controller.LastDot = value;
	/// </summary>
	float GetLastDot { get => Controller.LastDot; set => Controller.LastDot = value; }

	/// <summary>
	/// Controller.maxDotProduct
	/// </summary>
	float GetMaxDot => Controller.maxDotProduct;

	/// <summary>
	/// get => Controller.CollisionNormal; 
	/// set => Controller.CollisionNormal = value;
	/// </summary>
	Vector3 Normal { get => Controller.CollisionNormal; set => Controller.CollisionNormal = value; }
	#endregion
	public override void Enter(MovementController controller)
	{
		Debug.Log("i entered movingstate");
		Controller = controller;
		lastInputDuration = controller.IdleInputTime;
		IdleDecelerationSettings();
	}

	public override void FixedTick()
	{
		if (TimerUpdate(ref timerIdle, lastInputDuration))
		{
			timerIdle = 0;
			lastDir = GetMoveDir;
		}

		if (TimerUpdate(ref timer, duration, ref lastTimer))
		{
			timer = duration;
			lastTimer = timer;
			HasToFinish = false;
		}

		if (currentMomentum != MomentumType.Idle)
		{
			GetMoveDir = GetInputDir;
		}

		GetLastDot = Vector3.Dot(GetInputDir, GetMoveDir);

		speed = GetSpeed;
		Controller.LastSpeed = speed;

		vel = speed * Time.fixedDeltaTime * GetMoveDir;
		vel = vel.x * Controller.transform.right + Controller.transform.forward * vel.z;
		vel.y = Controller.Rb.velocity.y;
		if (Normal != Vector3.zero)
		{
			if (Normal.z != 0 && Mathf.Abs(Normal.z - vel.z) > Mathf.Abs(Normal.z))
				vel.z = 0;
			else if (Normal.x != 0 && Mathf.Abs(Normal.x - vel.x) > Mathf.Abs(Normal.x))
				vel.x = 0;
			Debug.LogWarning("Vel= " + vel);
		}
		Controller.Rb.velocity = vel;
	}


	public override void Tick()
	{
		if (HasToFinish)
			return;


		CheckMomentumType();
	}
	public override void Collision(Collision other)
	{
		Normal = other.contacts[0].normal;
		Debug.DrawRay(other.contacts[0].point, Normal * 10, Color.yellow, 10f);
		Debug.DrawRay(Controller.transform.position, Controller.Rb.velocity.normalized * 5, Color.red, 10f);

		if (Normal.y > 0)
		{
			Controller.IsAirborne = false;
		}
	}
	public override void CollisionExit(Collision other)
	{
		Normal = Vector3.zero;
	}
	public override void Exit()
	{
		Controller.LastSpeed = speed;
	}

	/// <summary>
	/// returns true if the timer is higher or equalt then duration
	/// </summary>
	bool TimerUpdate(ref float timer, float duration)
	{
		if (timer < duration)
		{
			timer += Time.deltaTime;
			return false;
		}
		else
		{
			return true;
		}
	}
	/// <summary>
	/// returns true if the timer is higher or equalt then duration
	/// <para>
	///	also saves the last timer value 
	/// </para>
	/// </summary>
	bool TimerUpdate(ref float timer, float duration, ref float lastTimer)
	{
		if (timer < duration)
		{
			timer += Time.deltaTime;
			lastTimer = timer;
			return false;
		}
		else
		{
			return true;
		}
	}
	/// <summary>
	/// checks if you have to accelerate or decelerate
	/// and then it applaies the correct settings
	/// </summary>
	void CheckMomentumType()
	{
		lastMomentum = currentMomentum;
		if (Controller.IsAirborne)
		{
			// airborn 
			currentMomentum = MomentumType.Airborne;
			Debug.Log("airborne");
			AirborneAccelerationSettings();
		}
		else if (GetInputDir == Vector3.zero)
		{
			//idle 
			lastDir = GetMoveDir;
			Debug.Log("Idle");
			currentMomentum = MomentumType.Idle;
			IdleDecelerationSettings();
			return;
		}
		else if (GetLastSpeed > GetMaxSpeed)
		{
			// from run to walk 
			Debug.Log("Run to walk");
			currentMomentum = MomentumType.RunToWalk;
			RunDecelerationSettings();
		}
		else if (GetLastDot < GetMaxDot && speed != 0)
		{
			// from 1 dir to another 
			Debug.Log("turning decel");
			currentMomentum = MomentumType.Turn;
			HasToFinish = true;
			TurnDecelerationSettings();
		}
		else if (HasToFinish == false)
		{
			// acceleration
			Debug.Log("Acceleration");
			currentMomentum = MomentumType.Accelleration;
			AccelerationSettings();
		}

		if (lastMomentum == currentMomentum)
		{
			timer = lastTimer;
		}
	}

	void IdleDecelerationSettings()
	{
		Vector3 mDir = lastDir;
		if (Normal != Vector3.zero)
		{
			if (Normal.z != 0 && Mathf.Abs(Normal.z - GetMoveDir.z) > Mathf.Abs(Normal.z))
			{
				mDir.z = 0;
			}
			else if (Normal.x != 0 && Mathf.Abs(Normal.x - GetMoveDir.x) > Mathf.Abs(Normal.x))
			{
				mDir.x = 0;
			}
			Normal = Vector3.zero;
		}

		GetMoveDir = mDir;

		Debug.LogError("move dir= " + GetMoveDir);
		usedCurve = Controller.DecelerationCurve;
		duration = Controller.DecelerationTime;
		minSpeed = 0;
		maxSpeed = GetLastSpeed;
		if (currentMomentum == lastMomentum)
		{
			timer = lastTimer;
		}
		else
		{
			timer = 0;
		}
	}
	/// <summary>
	/// this settings are used when you are running and then go back to walking
	/// </summary>
	void RunDecelerationSettings()
	{
		timer = 0;
		usedCurve = Controller.DecelerationCurve;
		duration = Controller.DecelerationTime;
		minSpeed = GetMaxSpeed;
		maxSpeed = GetLastSpeed;
	}
	/// <summary>
	/// this settings are used when you change direction too quiclky
	/// </summary>
	void TurnDecelerationSettings()
	{
		RunDecelerationSettings();
		minSpeed = GetLastSpeed / Controller.ChangeDirSpeedDivident;
		if (minSpeed < Controller.MinSpeed)
		{
			minSpeed = Controller.MinSpeed;
		}
	}

	void AirborneAccelerationSettings()
	{
		AccelerationSettings();
		duration = Controller.AccelerationAirborneTime;
	}

	void AccelerationSettings()
	{
		timer = 0;
		usedCurve = Controller.AccelerationCurve;
		duration = Controller.AccelerationTime;

		minSpeed = GetLastSpeed;
		if (minSpeed < Controller.MinSpeed)
			minSpeed = Controller.MinSpeed;

		maxSpeed = GetMaxSpeed;
		//Debug.Log("last speed= " + GetLastSpeed + "\nMaxSpeed = " + GetMaxSpeed);
	}
}

public class JumpState : MovementStates
{
	public override MovementController Controller { get; set; }

	/// <summary>
	/// get => Controller.CollisionNormal; 
	/// set => Controller.CollisionNormal = value;
	/// </summary>
	Vector3 Normal { get => Controller.CollisionNormal; set => Controller.CollisionNormal = value; }
	public override void Enter(MovementController controller)
	{
		Controller = controller;
		controller.Rb.AddForce(controller.transform.up * controller.JumpForce, ForceMode.Impulse);
		controller.IsAirborne = true;
	}

	public override void FixedTick()
	{

	}

	public override void Tick()
	{
		if (Normal.y > 0)
		{
			Controller.ChangeState(new MovingState());
		}
	}

	public override void Collision(Collision other)
	{
		Normal = other.contacts[0].normal;
		if (Normal.y > 0)
			Controller.ChangeState(new MovingState());
	}

	public override void CollisionExit(Collision other)
	{

	}

	public override void Exit()
	{
		Controller.IsAirborne = false;
	}
}

public class FallingState : MovementStates
{
	public override MovementController Controller { get; set; }

	/// <summary>
	/// get => Controller.CollisionNormal; 
	/// set => Controller.CollisionNormal = value;
	/// </summary>
	Vector3 Normal { get => Controller.CollisionNormal; set => Controller.CollisionNormal = value; }

	/// <summary>
	/// InputManager.MovementDir
	/// </summary>
	Vector3 GetInputDir => InputManager.MovementDir;

	public override void Enter(MovementController controller)
	{
		Controller = controller;
		controller.IsAirborne = true;
	}

	public override void FixedTick()
	{

	}

	public override void Tick()
	{
		if (Normal.y > 0)
		{
			Controller.IsAirborne = false;
			Controller.ChangeState(new MovingState());
		}
		else if (GetInputDir != Vector3.zero)
		{
			Controller.MaxSpeed = Controller.AirborneSpeed;

			Controller.ChangeState(new MovingState());
		}
	}

	public override void Collision(Collision other)
	{
		Normal = other.contacts[0].normal;
	}

	public override void CollisionExit(Collision other)
	{
		Normal = new();
	}

	public override void Exit()
	{

	}
}

public class ClimbState : MovementStates
{
	public override MovementController Controller { get; set; }
	Vector3 startpos;
	Vector3 endPos = new();
	Vector3 lerpedPos;
	float timer;

	public override void Enter(MovementController controller)
	{
		Controller = controller;
		Controller.MyCollider.enabled = false;
		Controller.Rb.useGravity = false;
		Controller.isClimbing = true;
		Controller.IsAirborne = false;

		startpos = Controller.transform.position;
		endPos = startpos;
		endPos.y = Controller.ClimbableObject.bounds.max.y + Controller.MyCollider.bounds.extents.y + Controller.ClimbOffsetY; // + controller.ClimbOffset
		endPos.z = controller.transform.position.z + Controller.ClimbOffsetZ;

		Controller.Rb.velocity = Vector3.zero;
	}
	public override void FixedTick()
	{
		if (timer < Controller.ClimbDuration)
		{
			timer += Time.fixedDeltaTime;
			lerpedPos = Vector3.Lerp(startpos, endPos, timer / Controller.ClimbDuration);
			Controller.Rb.MovePosition(lerpedPos);
		}
		else
		{
			Controller.Rb.MovePosition(endPos);
			Controller.ChangeState(new MovingState());
		}
	}

	public override void Tick()
	{

	}
	public override void Collision(Collision other)
	{

	}

	public override void CollisionExit(Collision other)
	{

	}


	public override void Exit()
	{
		Controller.MyCollider.enabled = true;
		Controller.Rb.useGravity = true;
		Controller.StartCoroutine(Controller.CheckLedgeCooldown());
	}

}