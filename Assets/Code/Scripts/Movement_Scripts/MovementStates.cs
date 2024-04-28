using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class MovementStates
{
	public abstract void Enter(MovementController controller);
	public abstract void Tick(MovementController controller);
	public abstract void FixedTick(MovementController controller);
	public abstract void Exit(MovementController controller);
}


public class WalkState : MovementStates
{

	public override void Enter(MovementController controller)
	{

	}

	public override void Exit(MovementController controller)
	{

	}

	public override void FixedTick(MovementController controller)
	{
		//controller.Rb.velocity *= controller.VelocityScale * controller.MaxSpeed* Time.DeltaTime* the Vector3 Movement;
	}

	public override void Tick(MovementController controller)
	{
		controller.VelocityScale = controller.AccelerationCurve.Evaluate(controller.MomentumCounter);

		if (controller.MomentumCounter < 1)
		{
			controller.MomentumCounter += Time.deltaTime * controller.AccelerationSpeed;
		}
		else
		{
			controller.MomentumCounter = 1;
		}
	}
}

public class IdleState : MovementStates
{
	public override void Enter(MovementController controller)
	{

	}

	public override void Exit(MovementController controller)
	{

	}

	public override void FixedTick(MovementController controller)
	{
		//controller.Rb.velocity *= controller.VelocityScale * controller.MaxSpeed* Time.DeltaTime* the Vector3 Movement;
	}

	public override void Tick(MovementController controller)
	{
		controller.VelocityScale = controller.DecelerationCurve.Evaluate(controller.MomentumCounter);

		if (controller.MomentumCounter > 0)
		{
			controller.MomentumCounter -= Time.deltaTime * controller.DecelerationSpeed;
		}
		else
		{
			controller.MomentumCounter = 0;
		}
	}
}
public class JumpState : MovementStates
{
	public override void Enter(MovementController controller)
	{

	}

	public override void Exit(MovementController controller)
	{

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

	public override void Exit(MovementController controller)
	{

	}

	public override void FixedTick(MovementController controller)
	{

	}

	public override void Tick(MovementController controller)
	{

	}
}