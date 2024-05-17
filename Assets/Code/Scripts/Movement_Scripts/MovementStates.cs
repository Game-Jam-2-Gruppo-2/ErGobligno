using System;
using UnityEngine;
using UnityEngine.InputSystem;

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

	Vector3 moveDir => InputManager.MovementDir;
	Vector3 vel;

	// AnimationCurve currentCurve;
	float movementTimer, movementTime;
	float maxSpeed, accelerationTimer, accelerationTime;
	AnimationCurve accelerationCurve;



	public override void Enter(MovementController controller)
	{
		Controller = controller;
		Controller.inputActions.Movement.Jump.performed += JumpExit;
		Controller.inputActions.Movement.Run.performed += Run;
	}

	private void Run(InputAction.CallbackContext context)
	{

	}

	public override void FixedTick()
	{
		if (Controller.Rb.velocity == Vector3.zero && moveDir == Vector3.zero)
			Controller.ChangeState(new IdleState());

		if (Controller.Rb.velocity.magnitude < maxSpeed)
		{
			//accelerate
		}

		vel = moveDir * Controller.WalkMaxSpeed;
		vel.y = Controller.Rb.velocity.y;
		Controller.Rb.velocity = vel;

		if (Controller.CheckLedge)
			Controller.ChangeState(new ClimbState());

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
		Controller.inputActions.Movement.Jump.performed -= JumpExit;
		Controller.inputActions.Movement.Run.performed -= Run;
	}
	private void JumpExit(InputAction.CallbackContext context)
	{
		Controller.ChangeState(new JumpState());
	}

}


public class IdleState : MovementStates
{
	public override MovementController Controller { get; set; }

	public override void Enter(MovementController controller)
	{
		Controller = controller;
		//reset
		Controller.MaxSpeed = Controller.WalkMaxSpeed;
		Controller.LastSpeed = 0;
		Controller.MoveDir = Vector3.zero;
		Controller.LastDot = 0;
		Controller.Rb.velocity = Vector3.zero;

		//inputs check
		Controller.inputActions.Movement.Walk.performed += MoveExit;
		Controller.inputActions.Movement.Jump.performed += JumpExit;
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
		Controller.inputActions.Movement.Walk.performed -= MoveExit;
		Controller.inputActions.Movement.Jump.performed -= JumpExit;
	}
	private void MoveExit(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		Controller.ChangeState(new MoveState());
	}

	private void JumpExit(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		Controller.ChangeState(new JumpState());
	}

}

public class JumpState : MovementStates
{
	public override MovementController Controller { get; set; }
	public override void Enter(MovementController controller)
	{
		Controller = controller;
		controller.Rb.AddForce(controller.transform.up * controller.JumpForce, ForceMode.Impulse);
		AudioManager.Request3DSFX?.Invoke(controller.Jump_SFX, controller.transform.position, controller.PitchVariation);
	}

	public override void FixedTick()
	{
		if (Controller.CheckLedge)
		{
			Controller.ClimbableCollider = Controller.Hit.collider;
			Controller.ChangeState(new ClimbState());
		}
	}

	public override void Tick()
	{

	}

	public override void Collision(Collision other)
	{
		Vector3 Normal = other.contacts[0].normal;

		if (Normal.y > 0)
		{
			Controller.ChangeState(new IdleState());
		}
	}

	public override void CollisionExit(Collision other)
	{

	}

	public override void Exit()
	{

	}
}

public class ClimbState : MovementStates
{
	public override MovementController Controller { get; set; }
	Vector3 startpos;
	Vector3 endPos;
	float climbTimer;

	public override void Enter(MovementController controller)
	{
		Controller = controller;

		Controller.MyCollider.enabled = false;
		Controller.Rb.useGravity = false;


		startpos = Controller.transform.position;
		endPos = startpos;
		endPos.y = Controller.ClimbableCollider.bounds.max.y + Controller.MyCollider.bounds.extents.y + Controller.ClimbOffsetY; // + controller.ClimbOffset
		endPos += controller.transform.forward * controller.ClimbOffsetZ;

		Controller.Rb.velocity = Vector3.zero;
		MovementController.OnClimb?.Invoke();
	}
	public override void FixedTick()
	{
		if (climbTimer < Controller.ClimbDuration)
		{
			climbTimer += Time.fixedDeltaTime;
			Controller.Rb.MovePosition(Vector3.Lerp(startpos, endPos, climbTimer / Controller.ClimbDuration));
		}
		else
		{
			Controller.Rb.MovePosition(endPos);
			Controller.ChangeState(new IdleState());
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
	}
}