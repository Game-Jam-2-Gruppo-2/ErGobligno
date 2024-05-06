using System;
using System.Threading;
using UnityEngine;

public abstract class MovementStates
{
	public abstract void Enter(MovementController controller);
	public abstract void Tick(MovementController controller);
	public abstract void FixedTick(MovementController controller);
	public abstract void Exit(MovementController controller, MovementStates newState);
}

public abstract class MomentumStates
{
	protected abstract float timer { get; set; }
	public abstract float duration { get; set; }
	public abstract float speed { get; set; }
	public abstract float maxValue { get; set; }
	public abstract float minValue { get; set; }
	public abstract Vector3 velocity { get; set; }
	public abstract AnimationCurve curveUsed { get; set; }

	public abstract void Enter(MovementController controller);
	public abstract void Tick(MovementController controller);
	public abstract void FixedTick(MovementController controller);
	public abstract void Exit(MovementController controller, MomentumStates state);
}

public class AccelerationState : MomentumStates
{
	float LerpSpeed(float curve) => Mathf.Lerp(minValue, maxValue, curve);
	float curveValue => curveUsed.Evaluate(timer);
	public override float duration { get; set; }
	public override float speed { get; set; }
	public override float maxValue { get; set; }
	public override float minValue { get; set; }
	public override Vector3 velocity { get => myVel; set => myVel = value; }
	public Vector3 myVel;
	public override AnimationCurve curveUsed { get; set; }
	protected override float timer { get; set; }

	public override void Enter(MovementController controller)
	{
		Debug.LogWarning("hello a");
		timer = 0;
		curveUsed = controller.AccelerationCurve;
		duration = controller.AccelerationTime;
		maxValue = controller.MaxSpeed;
		minValue = controller.LastSpeed;

	}

	public override void Exit(MovementController controller, MomentumStates state)
	{

	}

	public override void FixedTick(MovementController controller)
	{
		if (timer < duration)
			timer += Time.fixedDeltaTime * (1 / duration);
		else
			timer = duration;

		speed = LerpSpeed(curveValue);

		//Calculate velocity Scale
		myVel = speed * Time.fixedDeltaTime * controller.MoveDir;
		myVel = myVel.z * controller.transform.forward + controller.transform.right * myVel.x;
		myVel.y = controller.Rb.velocity.y;
		controller.Rb.velocity = velocity;

	}
	//, MomentumStates state
	public override void Tick(MovementController controller)
	{
		controller.LastDot = Vector3.Dot(InputManager.MovementDir, controller.MoveDir);
		if (InputManager.MovementDir == Vector3.zero)
		{
			controller.CurrentState.Exit(controller, new IdleState());
			return;
		}
		else if (controller.LastDot < controller.maxDotProduct)
		{
			Debug.Log("last dot = " + controller.LastDot + "\n" + controller.CurrentState);
			controller.MoveDir = InputManager.MovementDir;
			controller.CurrentState.Exit(controller, new MovingState());
			return;
		}
		controller.MoveDir = InputManager.MovementDir;
	}
}

public class DecellerationState : MomentumStates
{
	public float LerpSpeed(float curve) => Mathf.Lerp(minValue, maxValue, curve);
	public float curveValue => curveUsed.Evaluate(timer);

	#region variables
	public override float duration { get; set; }
	public override float speed { get; set; }
	public override float maxValue { get; set; }
	public override float minValue { get; set; }
	public override Vector3 velocity { get => myVel; set => myVel = value; }
	public Vector3 myVel;
	public override AnimationCurve curveUsed { get; set; }
	protected override float timer { get; set; }

	#endregion variables
	public override void Enter(MovementController controller)
	{
		Debug.LogWarning("hi d");
		timer = 0;
		curveUsed = controller.DecelerationCurve;
		duration = controller.DecelerationTime;

		maxValue = controller.LastSpeed;
		minValue = controller.MaxSpeed;
	}

	public override void Exit(MovementController controller, MomentumStates state)
	{
		Debug.Log("decelerationFinished");
		controller.ChangeMomentum(new AccelerationState());
	}

	public override void FixedTick(MovementController controller)
	{
		if (timer < duration)
			timer += Time.fixedDeltaTime * (1 / duration);
		else
		{
			timer = duration;
			Exit(controller, new AccelerationState());
		}

		speed = LerpSpeed(curveValue);

		//Calculate velocity Scale
		myVel = speed * Time.fixedDeltaTime * controller.MoveDir;
		myVel = myVel.z * controller.transform.forward + controller.transform.right * myVel.x;
		myVel.y = controller.Rb.velocity.y;
		controller.Rb.velocity = velocity;
	}

	public override void Tick(MovementController controller)
	{
		controller.LastDot = Vector3.Dot(InputManager.MovementDir, controller.MoveDir);
		if (InputManager.MovementDir == Vector3.zero)
		{
			controller.CurrentState.Exit(controller, new IdleState());
			return;
		}
		else if (controller.LastDot < controller.maxDotProduct)
		{
			Debug.Log("last dot = " + controller.LastDot + "\n" + controller.CurrentState);
			controller.MoveDir = InputManager.MovementDir;
			controller.CurrentState.Exit(controller, new MovingState());
			return;
		}
		controller.MoveDir = InputManager.MovementDir;
	}
}
public class MovingState : MovementStates
{

	public override void Enter(MovementController controller)
	{

		if (controller.LastSpeed > controller.MaxSpeed || controller.LastDot < controller.maxDotProduct)
		{
			controller.ChangeMomentum(new DecellerationState());
		}
		else
		{
			controller.ChangeMomentum(new AccelerationState());
		}
	}

	public override void Exit(MovementController controller, MovementStates newState)
	{
		controller.LastSpeed = controller.Momentumstate.speed;
		controller.ChangeState(newState);
	}

	public override void FixedTick(MovementController controller)
	{
		controller.Momentumstate.FixedTick(controller);
	}

	public override void Tick(MovementController controller)
	{
		controller.Momentumstate.Tick(controller);
	}
}
public class IdleState : MovementStates
{
	float decelerationTimer = 0;

	/// <summary>
	/// Q
	/// </summary>
	float speed = 0;

	Vector3 velocity;
	public override void Enter(MovementController controller)
	{

	}

	public override void Exit(MovementController controller, MovementStates newState)
	{
		controller.ChangeState(newState);
	}

	public override void FixedTick(MovementController controller)
	{
		if (controller.VelocityScalar == 0)
			return;

		// get values:
		if (decelerationTimer < controller.DecelerationTime)
			decelerationTimer += Time.fixedDeltaTime * (1 / controller.DecelerationTime);
		else
			decelerationTimer = controller.DecelerationTime;


		var c = controller.DecelerationCurve.Evaluate(decelerationTimer);

		//Calculate velocity Scale
		speed = Mathf.Lerp(0, controller.LastSpeed, c);

		velocity = controller.VelocityScalar * controller.MaxSpeed * Time.fixedDeltaTime * controller.MoveDir;
		velocity.y = controller.Rb.velocity.y;
		controller.Rb.velocity = velocity;
	}

	public override void Tick(MovementController controller)
	{
		if (InputManager.MovementDir != Vector3.zero)
		{
			Exit(controller, new MovingState());
		}
	}
}
public class JumpState : MovementStates
{
	float timer;
	float dur = 0.5f;
	public override void Enter(MovementController controller)
	{
		controller.Rb.AddForce(controller.transform.up * controller.JumpForce, ForceMode.Impulse);
		controller.IsAirBorne = true;
	}

	public override void Exit(MovementController controller, MovementStates newState)
	{
		controller.IsAirBorne = false;
		controller.ChangeState(newState);
	}

	public override void FixedTick(MovementController controller)
	{
		if (timer < dur)
		{
			timer += Time.fixedDeltaTime;
			return;
		}


		if (controller.CheckGround)
		{
			Exit(controller, new IdleState());
		}
	}

	public override void Tick(MovementController controller)
	{

	}
}
public class ClimbState : MovementStates
{
	Vector3 startpos;
	Vector3 endPos;
	Vector3 lerpedPos;
	float timer;
	public override void Enter(MovementController controller)
	{
		controller.Rb.useGravity = false;
		controller.isClimbing = true;
		controller.IsAirBorne = false;

		startpos = controller.transform.position;
		endPos = startpos;
		endPos.y = controller.ClimbableObject.bounds.max.y + controller.MyCollider.bounds.extents.y + controller.ClimbOffset; // + controller.ClimbOffset
		endPos.z = controller.ClimbableObject.transform.position.z;

		controller.Rb.velocity = Vector3.zero;
	}
	public override void Exit(MovementController controller, MovementStates newState)
	{
		controller.StartCoroutine(controller.CheckLedgeCooldown());
		controller.Rb.useGravity = true;

		//|---------------------------------New-State----------------------------------------|
		controller.ChangeState(newState);
	}

	public override void FixedTick(MovementController controller)
	{
		if (timer < controller.ClimbDuration)
		{
			timer += Time.fixedDeltaTime;
			lerpedPos = Vector3.Slerp(startpos, endPos, timer / controller.ClimbDuration);
			controller.Rb.MovePosition(lerpedPos);
		}
		else
		{
			controller.Rb.MovePosition(endPos);
			Exit(controller, new IdleState());
		}
	}

	public override void Tick(MovementController controller)
	{

	}
}
public class FallingState : MovementStates
{
	public override void Enter(MovementController controller)
	{
		controller.IsAirBorne = true;

	}

	public override void Exit(MovementController controller, MovementStates newState)
	{
		controller.IsAirBorne = false;
		controller.ChangeState(newState);
	}

	public override void FixedTick(MovementController controller)
	{
		if (controller.CheckGround)
		{
			Exit(controller, new IdleState());
		}
	}

	public override void Tick(MovementController controller)
	{

	}
}