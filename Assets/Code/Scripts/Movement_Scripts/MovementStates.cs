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


public class WalkState : MovementStates
{
	float AccelerationTimer = 0;
	float counter;

	/// <summary>
	/// Q
	/// </summary>
	float enterScalar = 0;
	public override void Enter(MovementController controller)
	{
		enterScalar = controller.VelocityScalar;
	}

	public override void Exit(MovementController controller, MovementStates newState)
	{
		controller.ChangeState(newState);
	}

	public override void FixedTick(MovementController controller)
	{
		// get values:
		AccelerationTimer += Time.fixedDeltaTime * (1 / controller.AccelerationTime);

		var c = controller.AccelerationCurve.Evaluate(AccelerationTimer);

		//Calculate velocity Scale
		controller.VelocityScalar = enterScalar + (c * (1 - enterScalar));

		controller.Rb.velocity = controller.VelocityScalar * controller.MaxSpeed * Time.fixedDeltaTime * controller.MoveDir;

		// Debug.Log("A= " + AccelerationTimer);
		// Debug.Log("Q = " + enterScalar + "\nC = " + c);

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

public class RunState : MovementStates
{
	float AccelerationTimer = 0;

	/// <summary>
	/// Q
	/// </summary>
	float enterScalar = 0;
	public override void Enter(MovementController controller)
	{
		enterScalar = controller.VelocityScalar;
	}

	public override void Exit(MovementController controller, MovementStates newState)
	{
		controller.ChangeState(newState);
	}

	public override void FixedTick(MovementController controller)
	{
		// get values:
		AccelerationTimer += Time.fixedDeltaTime * (1 / controller.AccelerationTime);

		var c = controller.AccelerationCurve.Evaluate(AccelerationTimer);

		//Calculate velocity Scale
		controller.VelocityScalar = enterScalar + (c * (1 - enterScalar));

		controller.Rb.velocity = controller.VelocityScalar * controller.MaxSpeed * Time.fixedDeltaTime * controller.MoveDir;

		// Debug.Log("A= " + AccelerationTimer);
		// Debug.Log("Q = " + enterScalar + "\nC = " + c);

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
	float enterScalar = 0;
	public override void Enter(MovementController controller)
	{
		enterScalar = controller.VelocityScalar;
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
		decelerationTimer += Time.fixedDeltaTime * (1 / controller.DecelerationTime);

		decelerationTimer = Mathf.Clamp01(decelerationTimer);

		var c = controller.DecelerationCurve.Evaluate(decelerationTimer);

		//Calculate velocity Scale
		controller.VelocityScalar = enterScalar * c;

		controller.Rb.velocity = controller.VelocityScalar * controller.MaxSpeed * Time.fixedDeltaTime * controller.MoveDir;
		// Debug.Log(decelerationTimer);
		// Debug.Log("Q = " + enterScalar + "\nC = " + c);
		// Debug.Log("S= " + controller.VelocityScale + "\nV= " + controller.Rb.velocity.magnitude);
	}

	public override void Tick(MovementController controller)
	{
		if (InputManager.MovementDir != Vector3.zero)
		{
			Exit(controller, new WalkState());
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
		controller.IsJumping = true;
	}

	public override void Exit(MovementController controller, MovementStates newState)
	{
		controller.IsJumping = false;
		controller.ChangeState(newState);
	}

	public override void FixedTick(MovementController controller)
	{

	}

	public override void Tick(MovementController controller)
	{
		if (timer < dur)
		{
			timer += Time.deltaTime;
			return;
		}


		if (Physics.Raycast(controller.transform.position, -controller.transform.up, controller.GroundCheckLenght, controller.GroundLayer))
		{
			Exit(controller, new IdleState());
		}
	}
}

public class ClimbState : MovementStates
{
	public override void Enter(MovementController controller)
	{
		InputManager.MoveInputs(false);
		InputManager.ClimbInputs(true);
		controller.Rb.useGravity = false;
		controller.isClimbing = true;
		controller.IsJumping = false;
	}
	public override void Exit(MovementController controller, MovementStates newState)
	{
		InputManager.MoveInputs(true);
		InputManager.ClimbInputs(false);
		controller.StartCoroutine(controller.CheckLedgeCooldown());
		//|------------------------------------------------------------------------------------------|
		controller.ChangeState(newState);
	}

	public override void FixedTick(MovementController controller)
	{

	}

	public override void Tick(MovementController controller)
	{
		if (InputManager.ClimbUp > 0)
		{
			Vector3 pos = controller.transform.position;
			pos.y = controller.ClimbableObject.bounds.extents.y;
			pos.z = controller.ClimbableObject.transform.position.z;
			pos.x = controller.ClimbableObject.transform.position.x;
		}
		else
		{
			Exit(controller, new IdleState());
		}
	}
}