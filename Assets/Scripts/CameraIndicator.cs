using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIndicator : MonoBehaviour
{
    [SerializeField] private GameObject cameraIndicatorPrefab;
    private GameObject cameraIndicatorClone;
    private Camera cam;
    
    private bool indicatorExists;
    private bool centerOnScreen;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        (Vector3 pos, float rotation) = DetermineIndicatorPosition(transform.position);

        if (!centerOnScreen)
        {
            if (!indicatorExists)
            {
                cameraIndicatorClone = Instantiate(cameraIndicatorPrefab, pos, Quaternion.Euler(0,0, rotation));
                indicatorExists = true;
            }
            else
            {
                cameraIndicatorClone.transform.position = pos;
                cameraIndicatorClone.transform.rotation = Quaternion.Euler(0, 0, rotation);
            }
        }
        else
        {
            if (indicatorExists)
            {
                Destroy(cameraIndicatorClone);
                indicatorExists = false;
            }
        }
    }

    private (Vector3, float) DetermineIndicatorPosition(Vector3 camPos)
    {
        float yCamOrthographicSize = cam.orthographicSize;
        float xCamOrthographicSize = yCamOrthographicSize * Screen.width / Screen.height;
        float padding = 1.25f;

        float rotation = 0;
        
        Vector3 dir = camPos;
        float dirSlope = dir.y / dir.x;
        
        Vector3 spawnPos = Vector3.zero;

        float slope = cam.orthographicSize / xCamOrthographicSize;

        bool Inequality(float xPos, float yPos, float signedSlope)
        {
            if (yPos >= signedSlope * xPos && yPos <= -signedSlope * xPos) return true;
            
            return false;
        }
        
        if (camPos.x > xCamOrthographicSize && Inequality(camPos.x, camPos.y, -slope))
        {
            rotation = 90f;
            float yVal = dirSlope * (padding - xCamOrthographicSize);
            spawnPos = new Vector3(padding - xCamOrthographicSize, yVal, -4f);
        }
        else if (camPos.x <= -xCamOrthographicSize && Inequality(camPos.x, camPos.y, slope))
        {
            rotation = -90f;
            float yVal = dirSlope * (xCamOrthographicSize - padding);
            spawnPos = new Vector3(xCamOrthographicSize - padding, yVal, -4f);
        }
        else if (camPos.y >= yCamOrthographicSize)
        {
            rotation = 180f;
            float xVal = 1 / dirSlope * (padding - yCamOrthographicSize);
            spawnPos = new Vector3(xVal, padding - yCamOrthographicSize, -4f);
        }
        else if (camPos.y <= -yCamOrthographicSize)
        {
            rotation = 0f;
            float xVal = 1 / dirSlope * (yCamOrthographicSize - padding);
            spawnPos = new Vector3(xVal, yCamOrthographicSize - padding, -4f);
        }
        else
        {
            centerOnScreen = true;
            return (Vector3.zero, 0);
        }

        centerOnScreen = false;
        spawnPos = new Vector3(spawnPos.x + camPos.x, spawnPos.y + camPos.y, -4f);
        return (spawnPos, rotation);
    }
}
