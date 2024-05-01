using System.Threading;
using UnityEngine;

public abstract class MovementStates
{
	public abstract void Enter(MovementController controller);
	public abstract void Tick(MovementController controller);
	public abstract void FixedTick(MovementController controller);
	public abstract void Exit(MovementController controller, MovementStates newState);
}


public class WalkState : MovementStates
{
	float MomentumCounter, MomentumTimer;
	float Bestspeed;
	public override void Enter(MovementController controller)
	{
		Debug.LogWarning("walking");
		MomentumCounter = 0;
		MomentumTimer = 0;
	}

	public override void Exit(MovementController controller, MovementStates newState)
	{
		controller.ChangeState(newState);
	}

	public override void FixedTick(MovementController controller)
	{
		controller.VelocityScale = controller.AccelerationCurve.Evaluate(MomentumCounter);

		controller.LastSpeed = controller.VelocityScale * Bestspeed;
		controller.Rb.velocity = controller.LastSpeed * Time.fixedDeltaTime * controller.MoveDir;
		Debug.Log(controller.Rb.velocity);

		if (MomentumTimer < controller.AccelerationTime)
		{
			MomentumTimer += Time.fixedDeltaTime;
			MomentumCounter = MomentumTimer / controller.AccelerationTime;
		}
		else
		{
			MomentumCounter = 1;
		}

		//Debug.Log(controller.MoveDir);

	}

	public override void Tick(MovementController controller)
	{
		InputManager.IsIdle(out controller.MoveDir);
		controller.MoveDir = InputManager.MovementDir;

		if (controller.isRunning)
			Bestspeed = controller.RunMaxSpeed;
		else
			Bestspeed = controller.MaxSpeed;
	}
}

public class IdleState : MovementStates
{
	float MomentumCounter, MomentumTimer;
	public override void Enter(MovementController controller)
	{
		MomentumCounter = 0;
		MomentumTimer = 0;
	}

	public override void Exit(MovementController controller, MovementStates newState)
	{

	}

	public override void FixedTick(MovementController controller)
	{
		controller.VelocityScale = controller.AccelerationCurve.Evaluate(MomentumCounter);
		controller.Rb.velocity = controller.VelocityScale * controller.LastSpeed * Time.fixedDeltaTime * controller.LastDirection;

		if (MomentumTimer < controller.DecelerationTime)
		{
			MomentumTimer += Time.fixedDeltaTime;
			MomentumCounter = MomentumTimer / controller.DecelerationTime;
		}
		else
		{
			MomentumCounter = 0;
			controller.Rb.velocity = Vector3.zero;
		}

	}

	public override void Tick(MovementController controller)
	{
		InputManager.IsMoving(out controller.MoveDir);
	}
}
public class JumpState : MovementStates
{
	public override void Enter(MovementController controller)
	{

	}

	public override void Exit(MovementController controller, MovementStates newState)
	{
		throw new System.NotImplementedException();
	}

	public override void FixedTick(MovementController controller)
	{

	}

	public override void Tick(MovementController controller)
	{

	}
}

public class ClimbState : MovementStates
{
	public override void Enter(MovementController controller)
	{

	}
	public override void Exit(MovementController controller, MovementStates newState)
	{
		throw new System.NotImplementedException();
	}

	public override void FixedTick(MovementController controller)
	{

	}

	public override void Tick(MovementController controller)
	{

	}
}