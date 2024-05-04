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

	public static PlayerInputs inputActions;

    public static bool UsingController { get; private set; }

	public static void Initialize()
	{
		inputActions = new();
		//Movement Events
		inputActions.Movement.Walk.performed += WalkInput;
		inputActions.Movement.Walk.performed += CheckInputDevice;
		inputActions.Movement.Walk.canceled += StopWalkInput;
		inputActions.Movement.Walk.canceled += CheckInputDevice;
        inputActions.Movement.Jump.performed += JumpInput;
        inputActions.Movement.Jump.performed += CheckInputDevice;
		inputActions.Movement.Run.performed += RunInput;
		inputActions.Movement.Run.performed += CheckInputDevice;
		//Input Game Events
		inputActions.Movement.Pause.performed += PauseInput;
		inputActions.Movement.Pause.performed += CheckInputDevice;
		inputActions.UI.Pause.performed += PauseInput;
		inputActions.UI.Pause.performed += CheckInputDevice;
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

	/// <summary>
	/// Check if a controller is used for the input action
	/// </summary>
	/// <param name="context"></param>
	private static void CheckInputDevice(InputAction.CallbackContext context)
	{
        Gamepad gamepad = Gamepad.current;

		if(gamepad == null)//Disconnected
        {
            UsingController = false;
            Debug.LogWarning("Controller Disconnected");
        }
        else//Connected
        {
            UsingController = true;
            Debug.LogWarning("Connected");
        }
    }

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