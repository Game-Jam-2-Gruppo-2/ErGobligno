using UnityEngine;

public abstract class MomentumStates
{
	protected abstract float timer { get; set; }
	public abstract float duration { get; set; }
	public abstract float speed { get; set; }
	public abstract float maxValue { get; set; }
	public abstract float minValue { get; set; }
	public abstract Vector3 velocity { get; set; }
	public abstract Vector3 dir { get; set; }
	public abstract AnimationCurve curveUsed { get; set; }

	public abstract void Enter(MovementController controller);
	public abstract void Tick(MovementController controller);
	public abstract void FixedTick(MovementController controller);
	public abstract void Exit(MovementController controller, MomentumStates state);
}

//-------------------------------------------------------------------------------------------------------------//

public class AccelerationState : MomentumStates
{
	float LerpSpeed(float curve) => Mathf.Lerp(minValue, maxValue, curve);
	float curveValue => curveUsed.Evaluate(timer);
	public override float duration { get; set; }
	public override float speed { get; set; }
	public override float maxValue { get; set; }
	public override float minValue { get; set; }
	public override Vector3 velocity { get => myVel; set => myVel = value; }
	public override Vector3 dir { get => moveDir; set => moveDir = value; }
	public Vector3 myVel, moveDir;
	public override AnimationCurve curveUsed { get; set; }
	protected override float timer { get; set; }

	public override void Enter(MovementController controller)
	{
		timer = 0;
		curveUsed = controller.AccelerationCurve;
		duration = controller.AccelerationTime;
		maxValue = controller.MaxSpeed;
		minValue = controller.LastSpeed;
	}

	public override void Exit(MovementController controller, MomentumStates state)
	{

	}

	public override void FixedTick(MovementController controller)
	{
		Debug.Log(moveDir);
		if (timer < duration)
			timer += Time.fixedDeltaTime * (1 / duration);
		else
			timer = duration;

		speed = LerpSpeed(curveValue);

		//Calculate velocity Scale
		myVel = speed * Time.fixedDeltaTime * moveDir;
		myVel = myVel.z * controller.transform.forward + controller.transform.right * myVel.x;
		myVel.y = controller.Rb.velocity.y;
		controller.Rb.velocity = myVel;
	}
	public override void Tick(MovementController controller)
	{
		controller.LastDot = Vector3.Dot(InputManager.MovementDir, controller.MoveDir);

		if (InputManager.MovementDir == Vector3.zero)
		{
			controller.CurrentState.Exit(controller, new IdleState());
			return;
		}
		else if (controller.LastDot < controller.maxDotProduct)
		{
			controller.MoveDir = InputManager.MovementDir;
			controller.CurrentState.Exit(controller, new MovingState());
			return;
		}
		controller.MoveDir = InputManager.MovementDir;
	}
}

//-------------------------------------------------------------------------------------------------------------//

public class DecellerationState : MomentumStates
{
	public float LerpSpeed(float curve) => Mathf.Lerp(minValue, maxValue, curve);
	public float curveValue => curveUsed.Evaluate(timer);

	#region variables
	public override float duration { get; set; }
	public override float speed { get; set; }
	public override float maxValue { get; set; }
	public override float minValue { get; set; }
	public override Vector3 velocity { get => myVel; set => myVel = value; }
	public override Vector3 dir { get => moveDir; set => moveDir = value; }
	public Vector3 myVel, moveDir;
	public override AnimationCurve curveUsed { get; set; }
	protected override float timer { get; set; }

	#endregion variables
	public override void Enter(MovementController controller)
	{

		timer = 0;
		curveUsed = controller.DecelerationCurve;
		duration = controller.DecelerationTime;

		maxValue = controller.LastSpeed;
		if (controller.LastDot < controller.maxDotProduct)
			minValue = controller.LastSpeed / controller.ChangeDirSpeedDivident;
		else
			minValue = controller.MaxSpeed;
	}

	public override void Exit(MovementController controller, MomentumStates state)
	{

		controller.LastSpeed = minValue;
		controller.ChangeMomentum(new AccelerationState());
	}

	public override void FixedTick(MovementController controller)
	{
		if (timer < duration)
			timer += Time.fixedDeltaTime * (1 / duration);
		else
		{
			timer = duration;
			Exit(controller, new AccelerationState());
		}

		speed = LerpSpeed(curveValue);

		Debug.Log(moveDir);
		//Calculate velocity Scale
		myVel = speed * Time.fixedDeltaTime * moveDir;
		myVel = myVel.z * controller.transform.forward + controller.transform.right * myVel.x;
		myVel.y = controller.Rb.velocity.y;
		controller.Rb.velocity = myVel;
	}

	public override void Tick(MovementController controller)
	{
		controller.LastDot = Vector3.Dot(InputManager.MovementDir, controller.MoveDir);

		if (InputManager.MovementDir == Vector3.zero)
		{
			controller.CurrentState.Exit(controller, new IdleState());
			return;
		}
		else if (controller.LastDot < controller.maxDotProduct)
		{
			controller.MoveDir = InputManager.MovementDir;
			controller.CurrentState.Exit(controller, new MovingState());
			return;
		}
		controller.MoveDir = InputManager.MovementDir;
	}
}
