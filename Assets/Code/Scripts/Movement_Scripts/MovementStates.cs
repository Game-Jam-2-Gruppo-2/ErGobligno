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
	Vector3 inputMoveDir => InputManager.MovementDir;
	Vector3 vel, moveDir, normal;
	float maxSpeed;
	bool isRunning, isAirborne;
	Rigidbody rb;
	float normalX, normalZ;


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
		moveDir.x += normalX;
		moveDir.z += normalZ;
		moveDir = moveDir.normalized;

		vel.y = Controller.Rb.velocity.y;
		rb.AddForce(Controller.IMpulseForce * moveDir.z * Time.fixedDeltaTime * rb.transform.forward, ForceMode.Impulse);
		rb.AddForce(Controller.IMpulseForce * moveDir.x * Time.fixedDeltaTime * rb.transform.right, ForceMode.Impulse);

		vel.x = Controller.Rb.velocity.x;
		vel.z = Controller.Rb.velocity.z;
		vel.x = Mathf.Clamp(vel.x, -maxSpeed, maxSpeed);
		vel.z = Mathf.Clamp(vel.z, -maxSpeed, maxSpeed);
		Controller.Rb.velocity = vel;
		Debug.LogError("vel= " + vel + "\nNormal " + normal);


		if (vel == Vector3.zero && inputMoveDir == Vector3.zero)
			Controller.ChangeState(new IdleState());

	}

	public override void Tick()
	{

	}

	public override void Collision(Collision other)
	{
		if (isAirborne)
		{
			Debug.Log("it's airborne");
			if (other.contacts[0].normal.y > 0)
			{
				isAirborne = false;
				maxSpeed = Controller.WalkMaxSpeed;
				Debug.Log("grounded");
			}

			if (other.contacts[0].normal.x != 0 || other.contacts[0].normal.z != 0)
			{
				normal = other.contacts[0].normal;
				normalX = normal.x;
				normalZ = normal.z;
				Debug.Log("Normal z: " + normalZ + "\nnormal x " + normalX);
			}
		}
	}

	public override void CollisionExit(Collision other)
	{
		normalZ = normalX = 0;

		if (Controller.GroundCheck == false && isAirborne == false)
		{
			isAirborne = true;
			Debug.Log("i am become airborne");
			maxSpeed = Controller.AirborneSpeed;
		}
	}

	public override void Exit()
	{
		Controller.inputActions.Movement.Run.performed -= Run;
		Controller.inputActions.Movement.Jump.performed -= JumpExit;
	}
	private void JumpExit(InputAction.CallbackContext context)
	{
		if (isAirborne)
			return;

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