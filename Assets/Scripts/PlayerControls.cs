//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/Scripts/PlayerControls.inputactions
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

public partial class @PlayerControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""HexBuildingActions"",
            ""id"": ""c7dcb1c7-74e8-489f-8aea-bdd894c70ed0"",
            ""actions"": [
                {
                    ""name"": ""SelectGateHex"",
                    ""type"": ""Button"",
                    ""id"": ""3859ec15-999a-4713-8211-9f4f83bc249e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SelectPowerHex"",
                    ""type"": ""Button"",
                    ""id"": ""797043bd-8d22-459c-bdb3-38193d469274"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SelectWorkshopHex"",
                    ""type"": ""Button"",
                    ""id"": ""633b6ee8-9aa6-4faa-b55a-d939dbf9a991"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SelectGoldmineHex"",
                    ""type"": ""Button"",
                    ""id"": ""863db267-94a8-4703-a84b-5ad560be3695"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SelectStarCollectorHex"",
                    ""type"": ""Button"",
                    ""id"": ""70ac3ebe-7c49-4d61-8a7e-057d835e4420"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ExitBuildMode"",
                    ""type"": ""Button"",
                    ""id"": ""042e5a42-743a-4f0d-b710-5c207fea225e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PlaceSelectedHex"",
                    ""type"": ""Button"",
                    ""id"": ""2b6aec9c-f029-41d5-875b-d831c0a527a5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""0c0a4cec-4f87-4d11-b03f-15fde8ac2bff"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""96451cc6-4130-4e96-9fab-460ee9bb0fc2"",
                    ""path"": ""<Keyboard>/#(1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectGateHex"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ba2db49f-63cb-4a4d-9021-d5bfefdcc912"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectPowerHex"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""70c5e083-2d21-4837-8f72-2c0859c60904"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectWorkshopHex"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""573e7c2d-4b8e-49bd-bc6a-ca9a1d8569a7"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectGoldmineHex"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""832bf788-39a4-4af2-a32a-ecc87d6b3739"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectStarCollectorHex"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bb764716-f24a-40af-ab18-d7acb12931ef"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ExitBuildMode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c87bdb40-1da0-46a6-bb28-7dea2fbddf7c"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlaceSelectedHex"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""01eb14dd-36ae-4c04-8609-70d2467c1bd8"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""ShipControlActions"",
            ""id"": ""9a504cc4-c6f6-4b1d-86a9-9cab0a9e7236"",
            ""actions"": [
                {
                    ""name"": ""SelectShip"",
                    ""type"": ""Button"",
                    ""id"": ""559dcd53-c0f9-4460-ad4c-0c95a837dd64"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""DragSelectShips"",
                    ""type"": ""Button"",
                    ""id"": ""1570c5e9-c84d-425a-8e7e-3f677ea75d9c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""a9b93236-012b-4482-acaa-2b26c5cb8797"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5d4a49ff-fff1-4d5e-9476-68864f68536c"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectShip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2ba8a93f-a071-49b2-a63f-355711f56ac0"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DragSelectShips"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5a4698ce-0080-45f1-9e53-1818ceacc47d"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UIActions"",
            ""id"": ""d5022dcd-4e94-41f3-ab32-b5061e94d527"",
            ""actions"": [
                {
                    ""name"": ""DisplayExtraInfo"",
                    ""type"": ""Button"",
                    ""id"": ""e39f0a55-dd0f-49e6-9195-ffb5d26192c0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""e73543cf-ceba-4ae8-bf22-eafa564fc291"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9d862d4e-7fe0-4d05-bdcf-e2f6e2c72773"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DisplayExtraInfo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a547f300-485a-4c7b-b0c8-11346ae202d0"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // HexBuildingActions
        m_HexBuildingActions = asset.FindActionMap("HexBuildingActions", throwIfNotFound: true);
        m_HexBuildingActions_SelectGateHex = m_HexBuildingActions.FindAction("SelectGateHex", throwIfNotFound: true);
        m_HexBuildingActions_SelectPowerHex = m_HexBuildingActions.FindAction("SelectPowerHex", throwIfNotFound: true);
        m_HexBuildingActions_SelectWorkshopHex = m_HexBuildingActions.FindAction("SelectWorkshopHex", throwIfNotFound: true);
        m_HexBuildingActions_SelectGoldmineHex = m_HexBuildingActions.FindAction("SelectGoldmineHex", throwIfNotFound: true);
        m_HexBuildingActions_SelectStarCollectorHex = m_HexBuildingActions.FindAction("SelectStarCollectorHex", throwIfNotFound: true);
        m_HexBuildingActions_ExitBuildMode = m_HexBuildingActions.FindAction("ExitBuildMode", throwIfNotFound: true);
        m_HexBuildingActions_PlaceSelectedHex = m_HexBuildingActions.FindAction("PlaceSelectedHex", throwIfNotFound: true);
        m_HexBuildingActions_MousePosition = m_HexBuildingActions.FindAction("MousePosition", throwIfNotFound: true);
        // ShipControlActions
        m_ShipControlActions = asset.FindActionMap("ShipControlActions", throwIfNotFound: true);
        m_ShipControlActions_SelectShip = m_ShipControlActions.FindAction("SelectShip", throwIfNotFound: true);
        m_ShipControlActions_DragSelectShips = m_ShipControlActions.FindAction("DragSelectShips", throwIfNotFound: true);
        m_ShipControlActions_MousePosition = m_ShipControlActions.FindAction("MousePosition", throwIfNotFound: true);
        // UIActions
        m_UIActions = asset.FindActionMap("UIActions", throwIfNotFound: true);
        m_UIActions_DisplayExtraInfo = m_UIActions.FindAction("DisplayExtraInfo", throwIfNotFound: true);
        m_UIActions_Pause = m_UIActions.FindAction("Pause", throwIfNotFound: true);
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

    // HexBuildingActions
    private readonly InputActionMap m_HexBuildingActions;
    private List<IHexBuildingActionsActions> m_HexBuildingActionsActionsCallbackInterfaces = new List<IHexBuildingActionsActions>();
    private readonly InputAction m_HexBuildingActions_SelectGateHex;
    private readonly InputAction m_HexBuildingActions_SelectPowerHex;
    private readonly InputAction m_HexBuildingActions_SelectWorkshopHex;
    private readonly InputAction m_HexBuildingActions_SelectGoldmineHex;
    private readonly InputAction m_HexBuildingActions_SelectStarCollectorHex;
    private readonly InputAction m_HexBuildingActions_ExitBuildMode;
    private readonly InputAction m_HexBuildingActions_PlaceSelectedHex;
    private readonly InputAction m_HexBuildingActions_MousePosition;
    public struct HexBuildingActionsActions
    {
        private @PlayerControls m_Wrapper;
        public HexBuildingActionsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @SelectGateHex => m_Wrapper.m_HexBuildingActions_SelectGateHex;
        public InputAction @SelectPowerHex => m_Wrapper.m_HexBuildingActions_SelectPowerHex;
        public InputAction @SelectWorkshopHex => m_Wrapper.m_HexBuildingActions_SelectWorkshopHex;
        public InputAction @SelectGoldmineHex => m_Wrapper.m_HexBuildingActions_SelectGoldmineHex;
        public InputAction @SelectStarCollectorHex => m_Wrapper.m_HexBuildingActions_SelectStarCollectorHex;
        public InputAction @ExitBuildMode => m_Wrapper.m_HexBuildingActions_ExitBuildMode;
        public InputAction @PlaceSelectedHex => m_Wrapper.m_HexBuildingActions_PlaceSelectedHex;
        public InputAction @MousePosition => m_Wrapper.m_HexBuildingActions_MousePosition;
        public InputActionMap Get() { return m_Wrapper.m_HexBuildingActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(HexBuildingActionsActions set) { return set.Get(); }
        public void AddCallbacks(IHexBuildingActionsActions instance)
        {
            if (instance == null || m_Wrapper.m_HexBuildingActionsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_HexBuildingActionsActionsCallbackInterfaces.Add(instance);
            @SelectGateHex.started += instance.OnSelectGateHex;
            @SelectGateHex.performed += instance.OnSelectGateHex;
            @SelectGateHex.canceled += instance.OnSelectGateHex;
            @SelectPowerHex.started += instance.OnSelectPowerHex;
            @SelectPowerHex.performed += instance.OnSelectPowerHex;
            @SelectPowerHex.canceled += instance.OnSelectPowerHex;
            @SelectWorkshopHex.started += instance.OnSelectWorkshopHex;
            @SelectWorkshopHex.performed += instance.OnSelectWorkshopHex;
            @SelectWorkshopHex.canceled += instance.OnSelectWorkshopHex;
            @SelectGoldmineHex.started += instance.OnSelectGoldmineHex;
            @SelectGoldmineHex.performed += instance.OnSelectGoldmineHex;
            @SelectGoldmineHex.canceled += instance.OnSelectGoldmineHex;
            @SelectStarCollectorHex.started += instance.OnSelectStarCollectorHex;
            @SelectStarCollectorHex.performed += instance.OnSelectStarCollectorHex;
            @SelectStarCollectorHex.canceled += instance.OnSelectStarCollectorHex;
            @ExitBuildMode.started += instance.OnExitBuildMode;
            @ExitBuildMode.performed += instance.OnExitBuildMode;
            @ExitBuildMode.canceled += instance.OnExitBuildMode;
            @PlaceSelectedHex.started += instance.OnPlaceSelectedHex;
            @PlaceSelectedHex.performed += instance.OnPlaceSelectedHex;
            @PlaceSelectedHex.canceled += instance.OnPlaceSelectedHex;
            @MousePosition.started += instance.OnMousePosition;
            @MousePosition.performed += instance.OnMousePosition;
            @MousePosition.canceled += instance.OnMousePosition;
        }

        private void UnregisterCallbacks(IHexBuildingActionsActions instance)
        {
            @SelectGateHex.started -= instance.OnSelectGateHex;
            @SelectGateHex.performed -= instance.OnSelectGateHex;
            @SelectGateHex.canceled -= instance.OnSelectGateHex;
            @SelectPowerHex.started -= instance.OnSelectPowerHex;
            @SelectPowerHex.performed -= instance.OnSelectPowerHex;
            @SelectPowerHex.canceled -= instance.OnSelectPowerHex;
            @SelectWorkshopHex.started -= instance.OnSelectWorkshopHex;
            @SelectWorkshopHex.performed -= instance.OnSelectWorkshopHex;
            @SelectWorkshopHex.canceled -= instance.OnSelectWorkshopHex;
            @SelectGoldmineHex.started -= instance.OnSelectGoldmineHex;
            @SelectGoldmineHex.performed -= instance.OnSelectGoldmineHex;
            @SelectGoldmineHex.canceled -= instance.OnSelectGoldmineHex;
            @SelectStarCollectorHex.started -= instance.OnSelectStarCollectorHex;
            @SelectStarCollectorHex.performed -= instance.OnSelectStarCollectorHex;
            @SelectStarCollectorHex.canceled -= instance.OnSelectStarCollectorHex;
            @ExitBuildMode.started -= instance.OnExitBuildMode;
            @ExitBuildMode.performed -= instance.OnExitBuildMode;
            @ExitBuildMode.canceled -= instance.OnExitBuildMode;
            @PlaceSelectedHex.started -= instance.OnPlaceSelectedHex;
            @PlaceSelectedHex.performed -= instance.OnPlaceSelectedHex;
            @PlaceSelectedHex.canceled -= instance.OnPlaceSelectedHex;
            @MousePosition.started -= instance.OnMousePosition;
            @MousePosition.performed -= instance.OnMousePosition;
            @MousePosition.canceled -= instance.OnMousePosition;
        }

        public void RemoveCallbacks(IHexBuildingActionsActions instance)
        {
            if (m_Wrapper.m_HexBuildingActionsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IHexBuildingActionsActions instance)
        {
            foreach (var item in m_Wrapper.m_HexBuildingActionsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_HexBuildingActionsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public HexBuildingActionsActions @HexBuildingActions => new HexBuildingActionsActions(this);

    // ShipControlActions
    private readonly InputActionMap m_ShipControlActions;
    private List<IShipControlActionsActions> m_ShipControlActionsActionsCallbackInterfaces = new List<IShipControlActionsActions>();
    private readonly InputAction m_ShipControlActions_SelectShip;
    private readonly InputAction m_ShipControlActions_DragSelectShips;
    private readonly InputAction m_ShipControlActions_MousePosition;
    public struct ShipControlActionsActions
    {
        private @PlayerControls m_Wrapper;
        public ShipControlActionsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @SelectShip => m_Wrapper.m_ShipControlActions_SelectShip;
        public InputAction @DragSelectShips => m_Wrapper.m_ShipControlActions_DragSelectShips;
        public InputAction @MousePosition => m_Wrapper.m_ShipControlActions_MousePosition;
        public InputActionMap Get() { return m_Wrapper.m_ShipControlActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ShipControlActionsActions set) { return set.Get(); }
        public void AddCallbacks(IShipControlActionsActions instance)
        {
            if (instance == null || m_Wrapper.m_ShipControlActionsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ShipControlActionsActionsCallbackInterfaces.Add(instance);
            @SelectShip.started += instance.OnSelectShip;
            @SelectShip.performed += instance.OnSelectShip;
            @SelectShip.canceled += instance.OnSelectShip;
            @DragSelectShips.started += instance.OnDragSelectShips;
            @DragSelectShips.performed += instance.OnDragSelectShips;
            @DragSelectShips.canceled += instance.OnDragSelectShips;
            @MousePosition.started += instance.OnMousePosition;
            @MousePosition.performed += instance.OnMousePosition;
            @MousePosition.canceled += instance.OnMousePosition;
        }

        private void UnregisterCallbacks(IShipControlActionsActions instance)
        {
            @SelectShip.started -= instance.OnSelectShip;
            @SelectShip.performed -= instance.OnSelectShip;
            @SelectShip.canceled -= instance.OnSelectShip;
            @DragSelectShips.started -= instance.OnDragSelectShips;
            @DragSelectShips.performed -= instance.OnDragSelectShips;
            @DragSelectShips.canceled -= instance.OnDragSelectShips;
            @MousePosition.started -= instance.OnMousePosition;
            @MousePosition.performed -= instance.OnMousePosition;
            @MousePosition.canceled -= instance.OnMousePosition;
        }

        public void RemoveCallbacks(IShipControlActionsActions instance)
        {
            if (m_Wrapper.m_ShipControlActionsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IShipControlActionsActions instance)
        {
            foreach (var item in m_Wrapper.m_ShipControlActionsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ShipControlActionsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public ShipControlActionsActions @ShipControlActions => new ShipControlActionsActions(this);

    // UIActions
    private readonly InputActionMap m_UIActions;
    private List<IUIActionsActions> m_UIActionsActionsCallbackInterfaces = new List<IUIActionsActions>();
    private readonly InputAction m_UIActions_DisplayExtraInfo;
    private readonly InputAction m_UIActions_Pause;
    public struct UIActionsActions
    {
        private @PlayerControls m_Wrapper;
        public UIActionsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @DisplayExtraInfo => m_Wrapper.m_UIActions_DisplayExtraInfo;
        public InputAction @Pause => m_Wrapper.m_UIActions_Pause;
        public InputActionMap Get() { return m_Wrapper.m_UIActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActionsActions set) { return set.Get(); }
        public void AddCallbacks(IUIActionsActions instance)
        {
            if (instance == null || m_Wrapper.m_UIActionsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_UIActionsActionsCallbackInterfaces.Add(instance);
            @DisplayExtraInfo.started += instance.OnDisplayExtraInfo;
            @DisplayExtraInfo.performed += instance.OnDisplayExtraInfo;
            @DisplayExtraInfo.canceled += instance.OnDisplayExtraInfo;
            @Pause.started += instance.OnPause;
            @Pause.performed += instance.OnPause;
            @Pause.canceled += instance.OnPause;
        }

        private void UnregisterCallbacks(IUIActionsActions instance)
        {
            @DisplayExtraInfo.started -= instance.OnDisplayExtraInfo;
            @DisplayExtraInfo.performed -= instance.OnDisplayExtraInfo;
            @DisplayExtraInfo.canceled -= instance.OnDisplayExtraInfo;
            @Pause.started -= instance.OnPause;
            @Pause.performed -= instance.OnPause;
            @Pause.canceled -= instance.OnPause;
        }

        public void RemoveCallbacks(IUIActionsActions instance)
        {
            if (m_Wrapper.m_UIActionsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IUIActionsActions instance)
        {
            foreach (var item in m_Wrapper.m_UIActionsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_UIActionsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public UIActionsActions @UIActions => new UIActionsActions(this);
    public interface IHexBuildingActionsActions
    {
        void OnSelectGateHex(InputAction.CallbackContext context);
        void OnSelectPowerHex(InputAction.CallbackContext context);
        void OnSelectWorkshopHex(InputAction.CallbackContext context);
        void OnSelectGoldmineHex(InputAction.CallbackContext context);
        void OnSelectStarCollectorHex(InputAction.CallbackContext context);
        void OnExitBuildMode(InputAction.CallbackContext context);
        void OnPlaceSelectedHex(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
    }
    public interface IShipControlActionsActions
    {
        void OnSelectShip(InputAction.CallbackContext context);
        void OnDragSelectShips(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
    }
    public interface IUIActionsActions
    {
        void OnDisplayExtraInfo(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
}
