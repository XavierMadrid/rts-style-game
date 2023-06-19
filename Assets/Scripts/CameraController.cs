using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] private float screenScrollSpeed = 35f;

    private Camera cam;

    private float scrollSpeedScale = .1f;
    private float lastScrollVal;
    
    [SerializeField] private GameObject backgroundImage = null;

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

        float camSize = cam.orthographicSize;
        camSize += Input.mouseScrollDelta.y * scrollSpeedScale;
        cam.orthographicSize = Mathf.Clamp(camSize, 15f, 30f);

        if (Input.GetKeyDown(KeyCode.C))
        {
            transform.position = new Vector3(0, 0, -10);
            backgroundImage.transform.position = new Vector3(0, 0, 1);
        }
    }
}
