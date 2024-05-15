using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
	private void Awake()
	{
		base.Awake();
	}


	private void OnEnable()
	{
		if (InputManager.inputActions == null)
			InputManager.Initialize();
		InputManager.inputActions.Movement.Walk.performed += MoveExit;
		InputManager.inputActions.Movement.Jump.performed += JumpExit;
		Rb.velocity = Vector3.zero;
	}

	private void JumpExit(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		controller.ChangeState(States.JUMP);
	}

	private void MoveExit(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		controller.ChangeState(States.MOVE);
	}

	private void OnDisable()
	{
		InputManager.inputActions.Movement.Walk.performed -= MoveExit;
		InputManager.inputActions.Movement.Jump.performed -= JumpExit;
	}

}
