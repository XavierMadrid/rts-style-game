using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] private float screenScrollSpeed = 35f;
    [SerializeField] private GameObject backgroundImage = null;

    private readonly float scrollSpeedScale = .1f; // OPTION
    private float camSize;
    public float CamSize
    {
        get => camSize;
        private set
        {
            camSize = Mathf.Clamp(value, 15f, 50f);
            cam.orthographicSize = camSize;
        }
    }
    private bool paused = false;
    private PlayerControls playerControls;
    private InputAction scrollValue;
    private InputAction spaceBar;
    private InputAction escape;
    
    private Camera cam;


    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void Start()
    {
        cam = GetComponent<Camera>();
    }
    
    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        float yBorderHeight = Screen.height * 0.05f;
        if (mousePos.y >= Screen.height - yBorderHeight && transform.position.y <= 550)
        {
            float yCloseness = Mathf.Clamp01((mousePos.y - (Screen.height - yBorderHeight)) / yBorderHeight);
            transform.Translate(Time.deltaTime * screenScrollSpeed * yCloseness * Vector3.up, Space.World);
            backgroundImage.transform.Translate(Time.deltaTime * screenScrollSpeed * yCloseness * 0.95f * Vector3.up, Space.World);
        }
        else if (mousePos.y <= yBorderHeight && transform.position.y >= -550)
        {
            float yCloseness = Mathf.Clamp01((yBorderHeight - mousePos.y) / yBorderHeight);
            transform.Translate(Time.deltaTime * screenScrollSpeed * yCloseness * Vector3.down, Space.World);
            backgroundImage.transform.Translate(Time.deltaTime * screenScrollSpeed * yCloseness * 0.95f * Vector3.down, Space.World);
        }
        float xBorderWidth = Screen.width * 0.05f;
        if (mousePos.x >= Screen.width - xBorderWidth && transform.position.x <= 1000)
        {
            float xCloseness = Mathf.Clamp01((mousePos.x - (Screen.width - xBorderWidth)) / xBorderWidth);
            transform.Translate(Time.deltaTime * screenScrollSpeed * xCloseness * Vector3.right, Space.World);
            backgroundImage.transform.Translate(Time.deltaTime * screenScrollSpeed * xCloseness * 0.95f * Vector3.right, Space.World);
        }
        else if (mousePos.x <= xBorderWidth && transform.position.x >= -1000)
        {
            float xCloseness = Mathf.Clamp01((xBorderWidth - mousePos.x) / xBorderWidth);
            transform.Translate(Time.deltaTime * screenScrollSpeed * xCloseness * Vector3.left, Space.World);
            backgroundImage.transform.Translate(Time.deltaTime * screenScrollSpeed * xCloseness * 0.95f * Vector3.left, Space.World);
        }

        camSize = cam.orthographicSize;
        camSize += scrollValue.ReadValue<Vector2>().y * scrollSpeedScale;

        spaceBar.performed += CenterCamera;
        escape.performed += PauseGame;
    }

    private void PauseGame(InputAction.CallbackContext context) // this pause is terrible. make it disable all necessary inputs and actually work
    {
        if (!paused)
        {
            paused = true;
            Time.timeScale = 0;
        }
        else
        {
            paused = false;
            Time.timeScale = 1;
        }
    }
    
    private void CenterCamera(InputAction.CallbackContext context)
    {
        transform.position = new Vector3(0, 0, -10);
        backgroundImage.transform.position = new Vector3(0, 0, 1);
    }
    
    private void OnEnable()
    {
        scrollValue = playerControls.UIActions.FieldOfViewZoom;
        spaceBar = playerControls.UIActions.CenterCamera;
        escape = playerControls.UIActions.Pause;
        playerControls.UIActions.Enable();
    }

    private void OnDisable()
    {
        playerControls.UIActions.Disable();
    }
}
