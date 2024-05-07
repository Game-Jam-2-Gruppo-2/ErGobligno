using UnityEngine;

public abstract class MovementStates
{
	public abstract void Enter(MovementController controller);
	public abstract void Tick(MovementController controller);
	public abstract void FixedTick(MovementController controller);
	public abstract void Exit(MovementController controller, MovementStates newState);
}

//-------------------------------------------------------------------------------------------------------------//

public class MovingState : MovementStates
{
	Vector3 dir;
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
	//-------------------------------------------------------------------------------------------------------------//
	public override void Exit(MovementController controller, MovementStates newState)
	{
		controller.LastSpeed = controller.Momentumstate.speed;
		controller.ChangeState(newState);
	}
	//-------------------------------------------------------------------------------------------------------------//
	public override void FixedTick(MovementController controller)
	{
		dir = controller.MoveDir;
		Debug.LogWarning(controller.CollisionDir.x + "\n" + controller.CollisionDir.z);

		if (controller.CollisionDir.x != 0) // if you are hitting an object
		{
			dir.x = 0;
			Debug.LogWarning("x is 0 = " + dir);
		}
		else if (controller.CollisionDir.z != 0)
		{
			dir.z = 0;
			Debug.LogWarning("z is 0 = " + dir);
		}

		controller.Momentumstate.dir = dir.normalized;
		controller.Momentumstate.FixedTick(controller);
	}
	//-------------------------------------------------------------------------------------------------------------//
	public override void Tick(MovementController controller)
	{
		if (controller.CollisionDir != InputManager.MovementDir)
		{
			controller.CollisionDir = Vector3.zero;
		}

		controller.Momentumstate.Tick(controller);
	}
}

//-------------------------------------------------------------------------------------------------------------//

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

//-------------------------------------------------------------------------------------------------------------//

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
	}

	public override void Tick(MovementController controller)
	{

	}
}

//-------------------------------------------------------------------------------------------------------------//

public class ClimbState : MovementStates
{
	Vector3 startpos;
	Vector3 endPos;
	Vector3 lerpedPos;
	float timer;
	public override void Enter(MovementController controller)
	{
		controller.MyCollider.enabled = false;
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
		controller.MyCollider.enabled = true;
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

//-------------------------------------------------------------------------------------------------------------//

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

	}

	public override void Tick(MovementController controller)
	{

	}
}