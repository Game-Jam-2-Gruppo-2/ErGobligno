using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

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

	public static PlayerInputs inputActions;

    public static bool UsingController { get; private set; }

	private static Vector2 CameraDelta;

	public static void Initialize()
	{
		inputActions = new();
		//Movement Events
		inputActions.Movement.Walk.performed += WalkInput;
		inputActions.Movement.Walk.canceled += StopWalkInput;
        inputActions.Movement.Jump.performed += JumpInput;
		inputActions.Movement.Run.performed += RunInput;
		//Input Game Events
		inputActions.Movement.Pause.performed += PauseInput;
		inputActions.UI.Pause.performed += PauseInput;
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

	public static void EnabledCameraInput(bool x)
	{
		if(x)
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

    #endregion

    public static Vector3 MovementDir => inputActions.Movement.Walk.ReadValue<Vector3>();
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