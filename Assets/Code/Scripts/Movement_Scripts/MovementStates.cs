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
}

public class MoveState : MovementStates
{
	public override MovementController Controller { get; set; }
	Vector3 inputMoveDir => InputManager.MovementDir;
	Vector3 vel, moveDir, normal;
	float maxSpeed;
	bool isRunning;
	Rigidbody rb;

	bool IsAirborne { get => Controller.IsAirborne; set => Controller.IsAirborne = value; }
#if UNITY_EDITOR
	Vector3 drawDir;
#endif
	Ray rayTop => new Ray(Controller.transform.position + Vector3.up * Controller.WallCheckHight, moveDir);
	Ray rayBot => new Ray(Controller.transform.position, moveDir);
	bool wallCheckTop => Physics.SphereCast(rayTop, 0.2f, Controller.WallCheckLenght);
	bool wallCheckBot => Physics.SphereCast(rayBot, 0.2f, Controller.WallCheckLenght);
	public override void Enter(MovementController controller)
	{
		Controller = controller;
		Controller.Rb.velocity = Vector3.zero;
		isRunning = false;
		maxSpeed = controller.WalkMaxSpeed;
		controller.inputActions.Movement.Run.performed += Run;
		Controller.inputActions.Movement.Jump.performed += JumpExit;
		rb = controller.Rb;
	}

	private void Run(InputAction.CallbackContext context)
	{
		isRunning = !isRunning;
		maxSpeed = isRunning ? Controller.RunMaxSpeed : Controller.WalkMaxSpeed;
	}

	public override void FixedTick()
	{
		moveDir = inputMoveDir;
		moveDir += normal;
		Vector3 dir = moveDir;
		dir.x = Controller.ImpulseForce * moveDir.x;
		dir.z = Controller.ImpulseForce * moveDir.z;

		rb.AddForce(Controller.ImpulseForce * moveDir.z * rb.transform.forward, ForceMode.Impulse);
		rb.AddForce(Controller.ImpulseForce * moveDir.x * rb.transform.right, ForceMode.Impulse);

		vel = rb.velocity;

		Debug.DrawRay(Controller.transform.position, vel, Color.red, Time.fixedDeltaTime);

		vel = Vector3.ClampMagnitude(vel, maxSpeed);

		rb.velocity = vel;

		if (vel == Vector3.zero && inputMoveDir == Vector3.zero)
			Controller.ChangeState(new IdleState());

		IsAirborne = Controller.AirborneCheck;
		if (IsAirborne)
		{
			if (Controller.CheckLedge)
			{
				if (Controller.Hit.transform.TryGetComponent(out IClimbable _))
				{
					Controller.ClimbableCollider = Controller.Hit.collider;
					Controller.ChangeState(new ClimbState());
				}
			}
			else if (wallCheckTop || wallCheckBot)
			{
				normal = -moveDir;
			}
			else
				normal = Vector3.zero;

			if (Controller.GravityTimer < Controller.GravityMaxDuration)
			{
				Controller.GravityTimer += Time.fixedDeltaTime;
			}

			float gravityPower = Mathf.Lerp(0, Controller.Gravity, Controller.GravityTimer / Controller.GravityMaxDuration);

			rb.velocity += Vector3.down * gravityPower;
		}
		else
		{
			Controller.GravityTimer = 0;
		}

#if UNITY_EDITOR

		drawDir = moveDir.z * Controller.transform.forward + (moveDir.x * Controller.transform.right);
		if (IsAirborne)
		{
			Debug.DrawRay(Controller.transform.position + Vector3.up * Controller.WallCheckHight, drawDir * Controller.WallCheckLenght, Color.cyan, Time.fixedDeltaTime);
			Debug.DrawRay(Controller.transform.position, drawDir * Controller.WallCheckLenght, Color.cyan, Time.fixedDeltaTime);
		}
#endif
	}

	public override void Tick()
	{ }

	public override void Collision(Collision other)
	{ }

	public override void Exit()
	{
		Controller.inputActions.Movement.Run.performed -= Run;
		Controller.inputActions.Movement.Jump.performed -= JumpExit;
	}
	private void JumpExit(InputAction.CallbackContext context)
	{
		if (IsAirborne)
			return;

		Controller.ChangeState(new JumpState());
	}
}


public class IdleState : MovementStates
{
	public override MovementController Controller { get; set; }
	bool IsAirborne { get => Controller.IsAirborne; set => Controller.IsAirborne = value; }
	public override void Enter(MovementController controller)
	{
		Controller = controller;
		//reset
		Controller.Rb.velocity = Vector3.zero;

		//inputs check
		Controller.inputActions.Movement.Walk.performed += MoveExit;
		Controller.inputActions.Movement.Jump.performed += JumpExit;
		if (InputManager.MovementDir != Vector3.zero)
			MoveExit();
	}

	public override void FixedTick()
	{
		IsAirborne = Controller.AirborneCheck;
		if (IsAirborne)
		{
			if (Controller.GravityTimer < Controller.GravityMaxDuration)
			{
				Controller.GravityTimer += Time.fixedDeltaTime;
			}

			float gravityPower = Mathf.Lerp(0, Controller.Gravity, Controller.GravityTimer / Controller.GravityMaxDuration);

			Controller.Rb.velocity += Vector3.down * gravityPower;
		}
		else
		{
			Controller.GravityTimer = 0;
		}
	}

	public override void Tick()
	{

	}

	public override void Collision(Collision other)
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
	void MoveExit()
	{
		Controller.ChangeState(new MoveState());
	}

	private void JumpExit(InputAction.CallbackContext context)
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
		Vector3 oldVel = controller.Rb.velocity;
		oldVel.y = 0;
		controller.Rb.AddForce(controller.transform.up * controller.JumpForce, ForceMode.Impulse);
		controller.Rb.velocity += oldVel;
		Debug.Log("vel= " + controller.Rb.velocity + "\noldVel= " + oldVel);
		AudioManager.Request2DSFX?.Invoke(controller.Jump_SFX, controller.transform.position, controller.StartingPitch, Random.Range(-controller.JumpPitchVariation, controller.JumpPitchVariation));
	}

	public override void FixedTick()
	{
		if (Controller.CheckLedge)
		{
			if (Controller.Hit.transform.TryGetComponent(out IClimbable _))
			{
				Controller.ClimbableCollider = Controller.Hit.collider;
				Controller.ChangeState(new ClimbState());
			}
		}


		if (Controller.GravityTimer < Controller.GravityMaxDuration)
		{
			Controller.GravityTimer += Time.fixedDeltaTime;
		}

		float gravityPower = Mathf.Lerp(0, Controller.Gravity, Controller.GravityTimer / Controller.GravityMaxDuration);

		Controller.Rb.velocity += Vector3.down * gravityPower;
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

	public override void Exit()
	{
		Controller.IsAirborne = false;
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

		startpos = Controller.transform.position;
		endPos = startpos;
		endPos.y = Controller.ClimbableCollider.bounds.max.y + Controller.MyCollider.bounds.extents.y + Controller.ClimbOffsetY; // + controller.ClimbOffset
		endPos += controller.transform.forward * controller.ClimbOffsetZ;

		Controller.Rb.velocity = Vector3.zero;
		MovementController.OnClimb?.Invoke();
		AudioManager.Request2DSFX?.Invoke(controller.Climb_SFX, controller.transform.position, controller.StartingPitch, Random.Range(-controller.ClimbPitchVariation, controller.ClimbPitchVariation));
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




	public override void Exit()
	{
		Controller.MyCollider.enabled = true;
		Controller.IsClimbing = false;
	}
}