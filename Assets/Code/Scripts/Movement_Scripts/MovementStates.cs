using System.Collections;
using System.Collections.Generic;
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