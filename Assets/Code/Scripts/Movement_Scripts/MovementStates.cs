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
	float LastSpeed;
	public override void Enter(MovementController controller)
	{

	}

	public override void Exit(MovementController controller, MovementStates newState)
	{
		controller.ChangeState(newState);
	}

	public override void FixedTick(MovementController controller)
	{
		controller.VelocityScale = controller.AccelerationCurve.Evaluate(MomentumCounter);
		if (MomentumTimer < controller.AccelerationTime)
		{
			MomentumTimer += Time.fixedDeltaTime;
			MomentumCounter = MomentumTimer / controller.AccelerationTime;
			//Debug.Log(MomentumCounter);
		}
		else
		{
			MomentumCounter = 1;
			Exit(controller, new IdleState());
		}

		//Debug.Log(controller.MoveDir);
		LastSpeed = controller.VelocityScale * Bestspeed;

		controller.Rb.velocity = controller.VelocityScale * Bestspeed * Time.fixedDeltaTime * controller.MoveDir;
	}

	public override void Tick(MovementController controller)
	{
		if (controller.isRunning)
			Bestspeed = controller.RunMaxSpeed;
		else
			Bestspeed = controller.MaxSpeed;
	}
}

public class IdleState : MovementStates
{
	float MomentumCounter, MomentumTimer;
	float Bestspeed;
	public override void Enter(MovementController controller)
	{
		Debug.Log("why am i here?");
	}
	public override void Exit(MovementController controller, MovementStates newState)
	{

	}

	public override void FixedTick(MovementController controller)
	{
		controller.VelocityScale = controller.AccelerationCurve.Evaluate(MomentumCounter);
		if (MomentumTimer > 0)
		{
			MomentumTimer -= Time.fixedDeltaTime;
			MomentumCounter = MomentumTimer / controller.DecelerationTime;
			Debug.Log(MomentumCounter);
		}
		else
		{
			MomentumCounter = 0;
		}
		controller.Rb.velocity = controller.VelocityScale * Bestspeed * Time.fixedDeltaTime * controller.MoveDir;

	}

	public override void Tick(MovementController controller)
	{
		if (controller.isRunning)
			Bestspeed = controller.RunMaxSpeed;
		else
			Bestspeed = controller.MaxSpeed;
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