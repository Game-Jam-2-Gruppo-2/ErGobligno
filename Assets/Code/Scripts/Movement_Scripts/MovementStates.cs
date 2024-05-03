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
public class AccelerationState : MovementStates
{
	float accelerationTimer = 0;
	float speed = 0;
	Vector3 velocity;
	public override void Enter(MovementController controller)
	{

	}

	public override void Exit(MovementController controller, MovementStates newState)
	{
		controller.LastSpeed = speed;
		controller.ChangeState(newState);
	}

	public override void FixedTick(MovementController controller)
	{
		float LerpSpeed(float curve) => Mathf.Lerp(controller.LastSpeed, controller.MaxSpeed, curve);
		float curveValue() => controller.AccelerationCurve.Evaluate(accelerationTimer);

		// get values:

		// check if last speed is higher then current max speed then it lerps it 


		if (controller.LastSpeed > controller.MaxSpeed) // if we are going from RunSpeed to WalkSpeed
		{
			if (accelerationTimer > 0)
				accelerationTimer -= Time.fixedDeltaTime * (1 / controller.AccelerationTime);
			else
				accelerationTimer = 0;

			speed = LerpSpeed(curveValue());
		}
		else // we are going from Idle to Acceleration or from WalkSpeed to RunSpeed
		{
			if (accelerationTimer < controller.AccelerationTime)
				accelerationTimer += Time.fixedDeltaTime * (1 / controller.AccelerationTime);
			else
				accelerationTimer = controller.AccelerationTime;

			speed = LerpSpeed(curveValue());
		}


		//Calculate velocity Scale
		velocity = speed * Time.fixedDeltaTime * controller.MoveDir;
		velocity.y = controller.Rb.velocity.y;

		controller.Rb.velocity = velocity;


	}

	public override void Tick(MovementController controller)
	{
		if (InputManager.MovementDir == Vector3.zero)
		{
			Exit(controller, new IdleState());
			return;
		}

		controller.MoveDir = InputManager.MovementDir;
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
		// Debug.Log(decelerationTimer);
		// Debug.Log("Q = " + enterScalar + "\nC = " + c);
		// Debug.Log("S= " + controller.VelocityScale + "\nV= " + controller.Rb.velocity.magnitude);
	}

	public override void Tick(MovementController controller)
	{
		if (InputManager.MovementDir != Vector3.zero)
		{
			Exit(controller, new AccelerationState());
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
			lerpedPos = Vector3.Lerp(startpos, endPos, timer / controller.ClimbDuration);
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
	float timer;
	float dur = 0.5f;
	public override void Enter(MovementController controller)
	{
		controller.IsAirBorne = true;
		Debug.Log("inside Falling");
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