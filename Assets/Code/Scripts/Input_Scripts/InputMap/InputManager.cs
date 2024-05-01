using System;
using UnityEngine;
using UnityEngine.InputSystem;

public static class InputManager
{
	public delegate void Vector3Delegate(Vector3 dir);
	public static event Vector3Delegate OnMovement;
	public static event Vector3Delegate OnStopMovement;

	public delegate void EmptyDelegate();
	public static event EmptyDelegate OnJump;
	public static event EmptyDelegate OnRun;
	public static event EmptyDelegate OnPauseGame;

	private static Vector3 lastDir;

	private static PlayerInputs inputActions;

	static InputManager()
	{

		inputActions = new();
		Inizialized();
	}

	public static void Inizialized()
	{
		// inputActions.Movement.Walk.performed += WalkInput;
		// inputActions.Movement.Walk.canceled += StopWalkInput;
		inputActions.Movement.Jump.performed += JumpInput;
		inputActions.Movement.Run.performed += RunInput;
		inputActions.UI.Pause.performed += PauseInput;
		inputActions.Movement.Pause.performed += PauseInput;
	}

	public static Vector3 MovementDir => inputActions.Movement.Walk.ReadValue<Vector3>();

	public static void MoveInputs(bool ToActivate)
	{
		Debug.LogError("i'm inside " + ToActivate);
		if (ToActivate)
			inputActions.Movement.Enable();
		else
			inputActions.Movement.Disable();
	}

	public static void UiInputs(bool ToActivate)
	{
		if (ToActivate)
			inputActions.UI.Enable();
		else
			inputActions.UI.Disable();
	}

	private static void PauseInput(InputAction.CallbackContext context)
	{
		OnPauseGame?.Invoke();
	}

	private static void RunInput(InputAction.CallbackContext context)
	{
		OnRun?.Invoke();
	}

	private static void JumpInput(InputAction.CallbackContext context)
	{
		OnJump?.Invoke();
	}



	/// <summary>
	/// checks every frame if you pressed a movement button 
	/// and then sends the Vector3 value with the OnMovement event
	/// </summary>
	public static void IsIdle(out Vector3 dir)
	{
		dir = MovementDir;

		if (dir == Vector3.zero)
		{
			OnStopMovement?.Invoke(lastDir);
		}
		else
		{
			lastDir = dir;
		}
	}

	/// <summary>
	/// Check if player is moving
	/// </summary>
	/// <param name="dir"> this will become the controller's movement dir </param>
	public static void IsMoving(out Vector3 dir)
	{
		dir = MovementDir;

		if (dir != Vector3.zero)
		{
			OnMovement?.Invoke(dir);
		}
	}

}