using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    private bool holdForExtraInfo = false; // OPTION

    public bool ExtendInfoToggle { get; private set; } = false;


    public GameObject Canvas;
    
    private PlayerControls playerControls;
    private InputAction shift;

    public event Action<bool> OnExtendInfoRequested;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    void Update()
    {
        if (holdForExtraInfo)
        {
            shift.performed += context => OnExtendInfoRequested?.Invoke(true);
            shift.canceled += context => OnExtendInfoRequested?.Invoke(false);
        }
        else
        {
            shift.performed += RequestExtendedInfoToggle;
        }
    }

    private void RequestExtendedInfoToggle(InputAction.CallbackContext context)
    {
        ExtendInfoToggle = !ExtendInfoToggle;
        OnExtendInfoRequested?.Invoke(ExtendInfoToggle);
    }

    private void OnEnable()
    {
        shift = playerControls.UIActions.DisplayExtraInfo;
        playerControls.UIActions.Enable();
    }

    private void OnDisable()
    {
        playerControls.UIActions.Disable();
    }
}
