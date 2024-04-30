using UnityEngine;
using UnityEngine.InputSystem;

public static class InputManager
{
	public delegate void Vector3Delegate(Vector3 dir);
	public static event Vector3Delegate OnMovement;

	public delegate void EmptyDelegate();
	public static event EmptyDelegate OnStopMovement;
	public static event EmptyDelegate OnJump;
	public static event EmptyDelegate OnRun;
	public static event EmptyDelegate OnPauseGame;


	private static PlayerInputs inputActions;

	static InputManager()
	{

		inputActions = new();
		OnEnableInGame();
	}

	public static void OnEnableInGame()
	{
		inputActions.Movement.Walk.performed += WalkInput;
		inputActions.Movement.Walk.canceled += StopWalkInput;
		inputActions.Movement.Jump.performed += JumpInput;
		inputActions.Movement.Run.performed += RunInput;
		inputActions.UI.Pause.performed += PauseInput;
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

	private static void StopWalkInput(InputAction.CallbackContext context)
	{
		OnStopMovement?.Invoke();
	}

	private static void WalkInput(InputAction.CallbackContext context)
	{
		Debug.Log(context.ReadValue<Vector3>());
		OnMovement?.Invoke(context.ReadValue<Vector3>());
	}
	public static void InGameInputs()
	{
		inputActions.Movement.Enable();
		inputActions.UI.Disable();
	}
}