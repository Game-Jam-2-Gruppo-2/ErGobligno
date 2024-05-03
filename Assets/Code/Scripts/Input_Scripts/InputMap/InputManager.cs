using System;
using System.Linq;
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

	public delegate void InputSourceChanged();
	public static InputSourceChanged OnInputSourceChanged;

	private static Vector3 lastDir;

	private static PlayerInputs inputActions;
	public static bool UsingController { get; private set; }

	public static void Inizialize()
	{
		inputActions = new();
		inputActions.Movement.Walk.performed += WalkInput;
		inputActions.Movement.Walk.canceled += StopWalkInput;

        inputActions.Movement.Jump.performed += JumpInput;
		inputActions.Movement.Run.performed += RunInput;
		inputActions.Movement.Pause.performed += PauseInput;
		inputActions.UI.Pause.performed += PauseInput;
		MoveInputs(true);
		UiInputs(false);
    }

	public static void MoveInputs(bool ToActivate)
	{
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

	public static void SetController(bool x)
	{
		if(!x)
		{
            inputActions.controlSchemes.First(x => x.name == "Keyboard");
			Debug.LogWarning("Input changed to: Keyboard");
        }
		else
		{
            inputActions.controlSchemes.First(x => x.name == "Controller");
            Debug.LogWarning("Input changed to: Controller");
        }

		UsingController = x;
    }

	private static void PauseInput(InputAction.CallbackContext context) => OnPauseGame?.Invoke();
	private static void RunInput(InputAction.CallbackContext context) => OnRun?.Invoke();
	private static void JumpInput(InputAction.CallbackContext context) => OnJump?.Invoke();
	private static void StopWalkInput(InputAction.CallbackContext context) => OnStopMovement?.Invoke(lastDir);
	private static void WalkInput(InputAction.CallbackContext context)
	{
		lastDir = context.ReadValue<Vector3>();
		OnMovement?.Invoke(context.ReadValue<Vector3>());
	}
}