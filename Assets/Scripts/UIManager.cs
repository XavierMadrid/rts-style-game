using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    private bool holdForExtraInfo = false; // OPTION

    public bool ExtendInfoToggle { get; private set; } = false;
    public bool ShopGUIOpen { get; private set; } = false;

    public GameObject Canvas;
    
    private PlayerControls playerControls;
    private InputAction shift;

    [SerializeField] private TextMeshProUGUI gameTimeText = null;
    [SerializeField] private RectTransform shopButtonManager = null;
    
    public event Action<bool> OnExtendInfoRequested;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void Start()
    {
        ManagerReferences.Instance.EnemyHandler.OnGameTimeChanged += GameTimeChanged;
        shopButtonManager.GetComponent<GUIShopButtonBehavior>().OnShopGUIOpened += guiOpen =>
        {
            ShopGUIOpen = guiOpen;
            Debug.Log($"shop opened: {guiOpen}");
        };
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

    private void GameTimeChanged(int gameTime)
    {
        int hours = gameTime / 3600;
        int minutes = gameTime / 60 % 60;
        int seconds = gameTime % 60;

        // XD
        gameTimeText.text = hours == 0 ? seconds < 10 ? $"{minutes}:0{seconds}" : $"{minutes}:{seconds}" : minutes < 10 ? $"{hours}:0{minutes}:{seconds}" : $"{hours}:{minutes}:{seconds}";
    }

    private void OnDisable()
    {
        playerControls.UIActions.Disable();
        ManagerReferences.Instance.EnemyHandler.OnGameTimeChanged -= GameTimeChanged;
    }
}
