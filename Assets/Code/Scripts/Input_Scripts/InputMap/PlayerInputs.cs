//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Code/Scripts/Input_Scripts/InputMap/PlayerInputs.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputs: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputs"",
    ""maps"": [
        {
            ""name"": ""Movement"",
            ""id"": ""370a2a61-ef00-410b-b63b-283a43375119"",
            ""actions"": [
                {
                    ""name"": ""Walk"",
                    ""type"": ""Value"",
                    ""id"": ""3ad87fb9-627a-44d6-ac13-04d59070c965"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""d7b04915-de4d-4f50-a9e1-573e8c5df4e7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""067fbf38-274a-40bf-9aa5-01b205757948"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""e351dece-4a7e-41bd-abe9-abe1b6890c96"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""b94beae3-022c-4cba-be1b-3772f323810d"",
                    ""path"": ""3DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Forward"",
                    ""id"": ""65bf56ff-3f43-4de6-82ed-aa3716d66ee2"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Backward"",
                    ""id"": ""97c7638a-0266-4b32-a0c9-6c4ff2edb595"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left"",
                    ""id"": ""8ed0176e-d402-42ca-89e4-6e21cb7ea956"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Right"",
                    ""id"": ""f9c8be52-f8b4-4fe4-94e2-5aed5afe3020"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Controller_Analog"",
                    ""id"": ""acadc9c5-5a9b-4e67-b4d6-de87a849ebec"",
                    ""path"": ""3DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""4516cf63-d5d2-428c-b670-6d80688c10ee"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""74f26189-37e3-4f81-b28c-83bf4bc9131c"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""forward"",
                    ""id"": ""2e58b9f5-7f99-4f75-a1cd-a916eb676991"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""backward"",
                    ""id"": ""dc3efe58-6fb3-4ed0-8229-e3ac195350fe"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Controller_Dpad"",
                    ""id"": ""2085873a-14be-4c83-9a87-489d5f1e1daa"",
                    ""path"": ""3DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""aeaa644c-8703-443f-8ab9-13c0ee242d36"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""0ea22eac-c16c-4ede-960a-dad3a298cbcd"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""forward"",
                    ""id"": ""f9b942be-deed-46d3-bc90-5cebdf5da46f"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""backward"",
                    ""id"": ""35d8128d-c3d5-4a8e-bd0f-25c39a8c21ba"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""d8d4d226-28c0-4675-bb44-a6abb855899b"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8dd2f517-18db-4f50-ba8f-27c86b4378c0"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d45e0a54-92c6-4638-86b7-484befca3509"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7578facd-6b6b-43a9-b5a8-86e005486a14"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3ec6bfde-efdf-47d2-95aa-3a5d136633fd"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c863bcb7-4578-42e6-9bb6-5fb66c0820ac"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""a1615e8d-1e14-48f3-b437-434e8ac58416"",
            ""actions"": [
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""4ba11531-2f62-48ba-930b-379989179c8a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a9c9666a-46e0-472b-b5ca-ad6d83f2f9f3"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""35a32fd4-7977-4098-8e6a-3e9a95274fa5"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Climb"",
            ""id"": ""6e591a46-d107-4f31-8fb6-c29026805c0e"",
            ""actions"": [
                {
                    ""name"": ""Climb"",
                    ""type"": ""Value"",
                    ""id"": ""97470ded-ebd5-4c07-84ab-d5d7d9b3db25"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""6c50f1b0-2554-4f61-a6c3-80e0bea71a95"",
                    ""path"": ""1DAxis(whichSideWins=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Climb"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""6063fa69-24eb-459b-ae24-4faa8dd7d84f"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Climb"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""70671406-f37a-4d5e-b4d3-76e129fbd2e2"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Climb"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Controller_Analog"",
                    ""id"": ""0988b6cd-80b3-4d42-9e1a-9c5df8e1f152"",
                    ""path"": ""1DAxis(whichSideWins=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Climb"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""c4440e6f-41c3-4b12-95a4-53a4d3f521f3"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Climb"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""7886d69f-4df4-4e9f-94e1-f29b5cae642b"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Climb"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Controller_Dpad"",
                    ""id"": ""2b3c459e-5b09-4069-a41d-b278906ed2e0"",
                    ""path"": ""1DAxis(whichSideWins=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Climb"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""87b4b010-b2db-48d5-b1a5-e86bf4dcacbf"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Climb"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""ee9dc620-8f4f-42bf-ac47-a46ccdc4de5d"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Climb"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""ControllerCamera_Actions"",
            ""id"": ""8ce8561e-8bf4-4831-bd4f-049235910df6"",
            ""actions"": [
                {
                    ""name"": ""CameraX"",
                    ""type"": ""Value"",
                    ""id"": ""e351a130-7d0f-46d8-9c03-e507a13ba9f2"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""CameraY"",
                    ""type"": ""Value"",
                    ""id"": ""81d79536-9888-4e39-8ad4-1eeeed36626a"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c5be99dd-2564-496b-85b5-c0abb4a2301e"",
                    ""path"": ""<Gamepad>/rightStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1cb4f031-1928-43c8-82c2-6697090dec22"",
                    ""path"": ""<Gamepad>/rightStick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""MouseCamera_Actions"",
            ""id"": ""336a45c9-467f-46c3-936e-01e257e02aad"",
            ""actions"": [
                {
                    ""name"": ""CameraX"",
                    ""type"": ""Value"",
                    ""id"": ""61f96116-515a-4b6a-92d0-a786689a8a02"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""CameraY"",
                    ""type"": ""Value"",
                    ""id"": ""065dc477-46a8-4576-99b7-e98a42e851ff"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3bad426d-b57c-4886-b073-898a69187fc3"",
                    ""path"": ""<Mouse>/delta/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""70408e50-9ec3-4857-9c6e-fa62ee1d8d7f"",
                    ""path"": ""<Mouse>/delta/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Controller"",
            ""bindingGroup"": ""Controller"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Movement
        m_Movement = asset.FindActionMap("Movement", throwIfNotFound: true);
        m_Movement_Walk = m_Movement.FindAction("Walk", throwIfNotFound: true);
        m_Movement_Jump = m_Movement.FindAction("Jump", throwIfNotFound: true);
        m_Movement_Run = m_Movement.FindAction("Run", throwIfNotFound: true);
        m_Movement_Pause = m_Movement.FindAction("Pause", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Pause = m_UI.FindAction("Pause", throwIfNotFound: true);
        // Climb
        m_Climb = asset.FindActionMap("Climb", throwIfNotFound: true);
        m_Climb_Climb = m_Climb.FindAction("Climb", throwIfNotFound: true);
        // ControllerCamera_Actions
        m_ControllerCamera_Actions = asset.FindActionMap("ControllerCamera_Actions", throwIfNotFound: true);
        m_ControllerCamera_Actions_CameraX = m_ControllerCamera_Actions.FindAction("CameraX", throwIfNotFound: true);
        m_ControllerCamera_Actions_CameraY = m_ControllerCamera_Actions.FindAction("CameraY", throwIfNotFound: true);
        // MouseCamera_Actions
        m_MouseCamera_Actions = asset.FindActionMap("MouseCamera_Actions", throwIfNotFound: true);
        m_MouseCamera_Actions_CameraX = m_MouseCamera_Actions.FindAction("CameraX", throwIfNotFound: true);
        m_MouseCamera_Actions_CameraY = m_MouseCamera_Actions.FindAction("CameraY", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Movement
    private readonly InputActionMap m_Movement;
    private List<IMovementActions> m_MovementActionsCallbackInterfaces = new List<IMovementActions>();
    private readonly InputAction m_Movement_Walk;
    private readonly InputAction m_Movement_Jump;
    private readonly InputAction m_Movement_Run;
    private readonly InputAction m_Movement_Pause;
    public struct MovementActions
    {
        private @PlayerInputs m_Wrapper;
        public MovementActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Walk => m_Wrapper.m_Movement_Walk;
        public InputAction @Jump => m_Wrapper.m_Movement_Jump;
        public InputAction @Run => m_Wrapper.m_Movement_Run;
        public InputAction @Pause => m_Wrapper.m_Movement_Pause;
        public InputActionMap Get() { return m_Wrapper.m_Movement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MovementActions set) { return set.Get(); }
        public void AddCallbacks(IMovementActions instance)
        {
            if (instance == null || m_Wrapper.m_MovementActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MovementActionsCallbackInterfaces.Add(instance);
            @Walk.started += instance.OnWalk;
            @Walk.performed += instance.OnWalk;
            @Walk.canceled += instance.OnWalk;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Run.started += instance.OnRun;
            @Run.performed += instance.OnRun;
            @Run.canceled += instance.OnRun;
            @Pause.started += instance.OnPause;
            @Pause.performed += instance.OnPause;
            @Pause.canceled += instance.OnPause;
        }

        private void UnregisterCallbacks(IMovementActions instance)
        {
            @Walk.started -= instance.OnWalk;
            @Walk.performed -= instance.OnWalk;
            @Walk.canceled -= instance.OnWalk;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Run.started -= instance.OnRun;
            @Run.performed -= instance.OnRun;
            @Run.canceled -= instance.OnRun;
            @Pause.started -= instance.OnPause;
            @Pause.performed -= instance.OnPause;
            @Pause.canceled -= instance.OnPause;
        }

        public void RemoveCallbacks(IMovementActions instance)
        {
            if (m_Wrapper.m_MovementActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMovementActions instance)
        {
            foreach (var item in m_Wrapper.m_MovementActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MovementActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MovementActions @Movement => new MovementActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private List<IUIActions> m_UIActionsCallbackInterfaces = new List<IUIActions>();
    private readonly InputAction m_UI_Pause;
    public struct UIActions
    {
        private @PlayerInputs m_Wrapper;
        public UIActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Pause => m_Wrapper.m_UI_Pause;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void AddCallbacks(IUIActions instance)
        {
            if (instance == null || m_Wrapper.m_UIActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_UIActionsCallbackInterfaces.Add(instance);
            @Pause.started += instance.OnPause;
            @Pause.performed += instance.OnPause;
            @Pause.canceled += instance.OnPause;
        }

        private void UnregisterCallbacks(IUIActions instance)
        {
            @Pause.started -= instance.OnPause;
            @Pause.performed -= instance.OnPause;
            @Pause.canceled -= instance.OnPause;
        }

        public void RemoveCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IUIActions instance)
        {
            foreach (var item in m_Wrapper.m_UIActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_UIActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public UIActions @UI => new UIActions(this);

    // Climb
    private readonly InputActionMap m_Climb;
    private List<IClimbActions> m_ClimbActionsCallbackInterfaces = new List<IClimbActions>();
    private readonly InputAction m_Climb_Climb;
    public struct ClimbActions
    {
        private @PlayerInputs m_Wrapper;
        public ClimbActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Climb => m_Wrapper.m_Climb_Climb;
        public InputActionMap Get() { return m_Wrapper.m_Climb; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ClimbActions set) { return set.Get(); }
        public void AddCallbacks(IClimbActions instance)
        {
            if (instance == null || m_Wrapper.m_ClimbActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ClimbActionsCallbackInterfaces.Add(instance);
            @Climb.started += instance.OnClimb;
            @Climb.performed += instance.OnClimb;
            @Climb.canceled += instance.OnClimb;
        }

        private void UnregisterCallbacks(IClimbActions instance)
        {
            @Climb.started -= instance.OnClimb;
            @Climb.performed -= instance.OnClimb;
            @Climb.canceled -= instance.OnClimb;
        }

        public void RemoveCallbacks(IClimbActions instance)
        {
            if (m_Wrapper.m_ClimbActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IClimbActions instance)
        {
            foreach (var item in m_Wrapper.m_ClimbActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ClimbActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public ClimbActions @Climb => new ClimbActions(this);

    // ControllerCamera_Actions
    private readonly InputActionMap m_ControllerCamera_Actions;
    private List<IControllerCamera_ActionsActions> m_ControllerCamera_ActionsActionsCallbackInterfaces = new List<IControllerCamera_ActionsActions>();
    private readonly InputAction m_ControllerCamera_Actions_CameraX;
    private readonly InputAction m_ControllerCamera_Actions_CameraY;
    public struct ControllerCamera_ActionsActions
    {
        private @PlayerInputs m_Wrapper;
        public ControllerCamera_ActionsActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @CameraX => m_Wrapper.m_ControllerCamera_Actions_CameraX;
        public InputAction @CameraY => m_Wrapper.m_ControllerCamera_Actions_CameraY;
        public InputActionMap Get() { return m_Wrapper.m_ControllerCamera_Actions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ControllerCamera_ActionsActions set) { return set.Get(); }
        public void AddCallbacks(IControllerCamera_ActionsActions instance)
        {
            if (instance == null || m_Wrapper.m_ControllerCamera_ActionsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ControllerCamera_ActionsActionsCallbackInterfaces.Add(instance);
            @CameraX.started += instance.OnCameraX;
            @CameraX.performed += instance.OnCameraX;
            @CameraX.canceled += instance.OnCameraX;
            @CameraY.started += instance.OnCameraY;
            @CameraY.performed += instance.OnCameraY;
            @CameraY.canceled += instance.OnCameraY;
        }

        private void UnregisterCallbacks(IControllerCamera_ActionsActions instance)
        {
            @CameraX.started -= instance.OnCameraX;
            @CameraX.performed -= instance.OnCameraX;
            @CameraX.canceled -= instance.OnCameraX;
            @CameraY.started -= instance.OnCameraY;
            @CameraY.performed -= instance.OnCameraY;
            @CameraY.canceled -= instance.OnCameraY;
        }

        public void RemoveCallbacks(IControllerCamera_ActionsActions instance)
        {
            if (m_Wrapper.m_ControllerCamera_ActionsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IControllerCamera_ActionsActions instance)
        {
            foreach (var item in m_Wrapper.m_ControllerCamera_ActionsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ControllerCamera_ActionsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public ControllerCamera_ActionsActions @ControllerCamera_Actions => new ControllerCamera_ActionsActions(this);

    // MouseCamera_Actions
    private readonly InputActionMap m_MouseCamera_Actions;
    private List<IMouseCamera_ActionsActions> m_MouseCamera_ActionsActionsCallbackInterfaces = new List<IMouseCamera_ActionsActions>();
    private readonly InputAction m_MouseCamera_Actions_CameraX;
    private readonly InputAction m_MouseCamera_Actions_CameraY;
    public struct MouseCamera_ActionsActions
    {
        private @PlayerInputs m_Wrapper;
        public MouseCamera_ActionsActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @CameraX => m_Wrapper.m_MouseCamera_Actions_CameraX;
        public InputAction @CameraY => m_Wrapper.m_MouseCamera_Actions_CameraY;
        public InputActionMap Get() { return m_Wrapper.m_MouseCamera_Actions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MouseCamera_ActionsActions set) { return set.Get(); }
        public void AddCallbacks(IMouseCamera_ActionsActions instance)
        {
            if (instance == null || m_Wrapper.m_MouseCamera_ActionsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MouseCamera_ActionsActionsCallbackInterfaces.Add(instance);
            @CameraX.started += instance.OnCameraX;
            @CameraX.performed += instance.OnCameraX;
            @CameraX.canceled += instance.OnCameraX;
            @CameraY.started += instance.OnCameraY;
            @CameraY.performed += instance.OnCameraY;
            @CameraY.canceled += instance.OnCameraY;
        }

        private void UnregisterCallbacks(IMouseCamera_ActionsActions instance)
        {
            @CameraX.started -= instance.OnCameraX;
            @CameraX.performed -= instance.OnCameraX;
            @CameraX.canceled -= instance.OnCameraX;
            @CameraY.started -= instance.OnCameraY;
            @CameraY.performed -= instance.OnCameraY;
            @CameraY.canceled -= instance.OnCameraY;
        }

        public void RemoveCallbacks(IMouseCamera_ActionsActions instance)
        {
            if (m_Wrapper.m_MouseCamera_ActionsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMouseCamera_ActionsActions instance)
        {
            foreach (var item in m_Wrapper.m_MouseCamera_ActionsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MouseCamera_ActionsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MouseCamera_ActionsActions @MouseCamera_Actions => new MouseCamera_ActionsActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_ControllerSchemeIndex = -1;
    public InputControlScheme ControllerScheme
    {
        get
        {
            if (m_ControllerSchemeIndex == -1) m_ControllerSchemeIndex = asset.FindControlSchemeIndex("Controller");
            return asset.controlSchemes[m_ControllerSchemeIndex];
        }
    }
    public interface IMovementActions
    {
        void OnWalk(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnPause(InputAction.CallbackContext context);
    }
    public interface IClimbActions
    {
        void OnClimb(InputAction.CallbackContext context);
    }
    public interface IControllerCamera_ActionsActions
    {
        void OnCameraX(InputAction.CallbackContext context);
        void OnCameraY(InputAction.CallbackContext context);
    }
    public interface IMouseCamera_ActionsActions
    {
        void OnCameraX(InputAction.CallbackContext context);
        void OnCameraY(InputAction.CallbackContext context);
    }
}
