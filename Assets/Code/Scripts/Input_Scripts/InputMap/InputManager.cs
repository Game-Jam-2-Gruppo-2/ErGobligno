using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{
	public static PlayerInputs inputActions;
	public static bool UsingController { get; private set; }
	private static Vector2 CameraDelta;
	public static Vector3 MovementDir => inputActions.Movement.Walk.ReadValue<Vector3>();

	public static void Initialize()
	{
		inputActions = new();
		//Enable for Game
		MoveInputs(true);
		UiInputs(false);
		EnabledCameraInput(true);
		//Set Up Camera Delta
		CameraDelta = Vector2.zero;
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

	#region Camera Functions
	public static void EnabledCameraInput(bool x)
	{
		if (x)
		{
			inputActions.ControllerCamera_Actions.Enable();
			inputActions.MouseCamera_Actions.Enable();
		}
		else
		{
			inputActions.ControllerCamera_Actions.Disable();
			inputActions.MouseCamera_Actions.Disable();
		}
	}

	public static Vector2 GetCameraDelta()
	{
		CameraDelta = Vector2.zero;

		//ReadMouse delta
		Vector2 mouseDelta;
		mouseDelta.x = inputActions.MouseCamera_Actions.CameraX.ReadValue<float>();
		mouseDelta.y = inputActions.MouseCamera_Actions.CameraY.ReadValue<float>();
		if (mouseDelta != Vector2.zero)
		{
			UsingController = false;
			CameraDelta = mouseDelta;
		}

		//Read Controller delta
		Vector2 controllerDelta;
		controllerDelta.x = inputActions.ControllerCamera_Actions.CameraX.ReadValue<float>();
		controllerDelta.y = inputActions.ControllerCamera_Actions.CameraY.ReadValue<float>();

		if (controllerDelta != Vector2.zero)
		{
			CameraDelta = controllerDelta;
			UsingController = true;
		}

		return CameraDelta;
	}
	#endregion
}
