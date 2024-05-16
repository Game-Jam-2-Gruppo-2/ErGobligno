using System;
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

public class MoveState : MovementStates
{
	public override MovementController Controller { get; set; }
	protected AnimationCurve usedCurve;
	protected float speed;
	protected float FromSpeed, ToSpeed;
	protected float timer = 0, lastTimer = 0;
	protected float timerIdle = 0, lastInputDuration;
	protected float duration;
	protected Vector3 vel, lastDir;
	protected bool HasToFinish;
	protected bool isIdle, isColliding;

	// public MoveState(float minSpeed, float maxSpeed, float duration, bool HasToFinish, AnimationCurve usedCurve)
	// {
	// 	this.minSpeed = minSpeed;
	// 	this.maxSpeed = maxSpeed;
	// 	this.duration = duration;
	// 	this.usedCurve = usedCurve;
	// 	this.HasToFinish = HasToFinish;
	// }


	#region shortcuts
	/// <summary>
	/// usedCurve.Evaluate(timer)
	/// </summary>
	protected float CurveValue => usedCurve.Evaluate(timer);

	/// <summary>
	/// Mathf.Lerp(minSpeed, maxSpeed, CurveValue)
	/// </summary>
	protected float GetSpeed => Mathf.Lerp(FromSpeed, ToSpeed, CurveValue);

	/// <summary>
	/// get => Controller.MoveDir; 
	/// set => Controller.MoveDir = value;
	/// </summary>
	protected Vector3 GetMoveDir { get => Controller.MoveDir; set => Controller.MoveDir = value; }

	/// <summary>
	/// InputManager.MovementDir
	/// </summary>
	protected Vector3 GetInputDir => InputManager.MovementDir;

	/// <summary>
	///  get => Controller.LastSpeed; set => Controller.LastSpeed = value;
	/// </summary>
	protected float GetLastSpeed { get => Controller.LastSpeed; set => Controller.LastSpeed = value; }

	/// <summary>
	/// Controller.MaxSpeed
	/// </summary>
	protected float GetMaxSpeed => Controller.MaxSpeed;

	/// <summary>
	/// get => Controller.LastDot;
	/// set => Controller.LastDot = value;
	/// </summary>
	protected float GetLastDot { get => Controller.LastDot; set => Controller.LastDot = value; }

	/// <summary>
	/// Controller.maxDotProduct
	/// </summary>
	protected float GetMaxDot => Controller.maxDotProduct;

	/// <summary>
	/// get => Controller.CollisionNormal; 
	/// set => Controller.CollisionNormal = value;
	/// </summary>
	protected Vector3 Normal { get => Controller.CollisionNormal; set => Controller.CollisionNormal = value; }
	#endregion

	public override void Enter(MovementController controller)
	{
		Controller = controller;

		if (GetMaxSpeed < GetLastSpeed)
		{
			ToSpeed = GetLastSpeed;
			FromSpeed = GetMaxSpeed;
			usedCurve = controller.DecelerationCurve;
		}
		else
		{
			ToSpeed = GetMaxSpeed;
			FromSpeed = GetLastSpeed;
			usedCurve = controller.AccelerationCurve;
		}

		timer = 0;
		duration = controller.AccelerationTime;
	}

	public override void FixedTick()
	{
		GetLastSpeed = speed;
		FixedChecks();
	}

	void FixedChecks()
	{

		if (GetLastDot < GetMaxDot && lastDir != Vector3.zero)
		{
			Debug.Log("Turn");
			timer = 0;
			FromSpeed = GetMaxSpeed;
			ToSpeed = GetLastSpeed / Controller.ChangeDirSpeedDivident;
			usedCurve = Controller.DecelerationCurve;
			duration = Controller.DecelerationTime;
		}
		else if (isIdle && lastDir != Vector3.zero)
		{
			Debug.Log("Idle");
			timer = 0;
			ToSpeed = GetLastSpeed;
			FromSpeed = 0;
			usedCurve = Controller.DecelerationCurve;
			duration = Controller.DecelerationTime;
			GetMoveDir = lastDir;
		}

		if (timer < duration)
		{
			timer += Time.deltaTime;
		}
		else
		{
			HasToFinish = false;
		}

		speed = GetSpeed;
		Controller.LastSpeed = speed;

		vel = speed * Time.fixedDeltaTime * GetMoveDir;
		vel = vel.x * Controller.transform.right + Controller.transform.forward * vel.z;
		vel.y = Controller.Rb.velocity.y;
		Controller.Rb.velocity = vel;
		if (Controller.Rb.velocity == Vector3.zero)
		{
			Controller.ChangeState(new IdleState());
		}
	}

	public override void Tick()
	{
		lastDir = GetMoveDir;
		GetLastDot = Vector3.Dot(GetInputDir, GetMoveDir);
		isIdle = GetInputDir == Vector3.zero;

		if (isIdle == false)
			GetMoveDir = GetInputDir;

		if (isColliding)
		{
			GetMoveDir = (GetMoveDir + Normal).normalized;
		}
	}

	public override void Exit()
	{
		GetLastSpeed = speed;
	}

	public override void Collision(Collision other)
	{
		Vector3 Normal = other.contacts[0].normal;

		if (Controller.IsAirborne && Normal.y > 0)
		{
			Debug.Log("Walk");
			timer = 0;
			Controller.IsAirborne = false;
			ToSpeed = Controller.MaxSpeed;
			FromSpeed = 0;
			duration = Controller.AccelerationTime;
			usedCurve = Controller.AccelerationCurve;
		}
		else if (Controller.IsAirborne && (Normal.x != 0 || Normal.z != 0) && other.gameObject.layer != Controller.MovableLayer)
		{
			isColliding = true;
		}
	}

	public override void CollisionExit(Collision other)
	{
		if (Controller.IsAirborne == false && Controller.GroundCheck == false)
		{
			Normal = Vector3.zero;
			isColliding = false;

			timer = 0;
			Controller.IsAirborne = true;

			ToSpeed = Controller.AirborneSpeed;
			FromSpeed = GetLastSpeed;
			usedCurve = Controller.AccelerationCurve;

			if (ToSpeed < GetLastSpeed)
			{
				usedCurve = Controller.DecelerationCurve;
				ToSpeed = FromSpeed;
				FromSpeed = Controller.AirborneSpeed;
			}

			duration = Controller.AccelerationAirborneTime;
		}
	}

}

public class IdleState : MovementStates
{
	public override MovementController Controller { get; set; }
	public override void Enter(MovementController controller)
	{
		Controller = controller;
		Controller.Rb.velocity = Vector3.zero;
		InputManager.inputActions.Movement.Walk.performed += MoveExit;
		InputManager.inputActions.Movement.Jump.performed += JumpExit;
	}

	private void MoveExit(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		Controller.ChangeState(new MoveState());
	}

	private void JumpExit(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		Controller.ChangeState(new JumpState());
	}

	public override void FixedTick()
	{

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
		Controller.LastSpeed = 0;
		Controller.MoveDir = Vector3.zero;
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
			//Controller.ChangeState(new MovingState());
		}
	}

	public override void Collision(Collision other)
	{
		Normal = other.contacts[0].normal;
		//if (Normal.y > 0)
		//Controller.ChangeState(new MovingState());
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
			//Controller.ChangeState(new MovingState());
		}
		else if (GetInputDir != Vector3.zero)
		{
			Controller.MaxSpeed = Controller.AirborneSpeed;

			//Controller.ChangeState(new MovingState());
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
		endPos += controller.transform.forward * controller.ClimbOffsetZ;

		Controller.Rb.velocity = Vector3.zero;
		MovementController.climb?.Invoke();
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
			//Controller.ChangeState(new MovingState());
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