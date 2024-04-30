using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEditor;
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

	public override void Enter(MovementController controller)
	{

	}

	public override void Exit(MovementController controller, MovementStates newState)
	{
		controller.ChangeState(newState);
	}

	public override void FixedTick(MovementController controller)
	{
		controller.VelocityScale = controller.AccelerationCurve.Evaluate(controller.MomentumCounter);
		if (controller.MomentumCounter < 1)
		{
			controller.MomentumCounter += Time.fixedDeltaTime * (1 / controller.AccelerationTime);
			Debug.Log(controller.MomentumCounter);
		}
		else
		{
			controller.MomentumCounter = 1;
		}

		if (controller.isRunning)
			controller.Rb.velocity = controller.VelocityScale * controller.RunMaxSpeed * Time.fixedDeltaTime * controller.MoveDir;
		else
			controller.Rb.velocity = controller.VelocityScale * controller.MaxSpeed * Time.fixedDeltaTime * controller.MoveDir;
	}

	public override void Tick(MovementController controller)
	{

	}
}

public class IdleState : MovementStates
{
	public override void Enter(MovementController controller)
	{

	}
	public override void Exit(MovementController controller, MovementStates newState)
	{

	}

	public override void FixedTick(MovementController controller)
	{
		controller.VelocityScale = controller.DecelerationCurve.Evaluate(controller.MomentumCounter);

		if (controller.MomentumCounter > 0)
		{
			controller.MomentumCounter -= Time.fixedDeltaTime * (1 / controller.DecelerationSpeed);
		}
		else
		{
			controller.MomentumCounter = 0;
		}

		if (controller.isRunning)
		{
			controller.Rb.velocity = controller.VelocityScale * controller.RunMaxSpeed * Time.fixedDeltaTime * controller.MoveDir;
		}
		else
			controller.Rb.velocity = controller.VelocityScale * controller.MaxSpeed * Time.fixedDeltaTime * controller.MoveDir;
	}

	public override void Tick(MovementController controller)
	{


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