using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class ShipIndicatorCreator : MonoBehaviour
{
    [SerializeField] private GameObject shipIndicatorPrefab;

    private Camera cam;
    private GameObject shipIndicator;
    
    private bool indicatorExists;
    private bool shipOnScreen;
    private float dist;
    
    private Tuple<Vector3, float> values;
    private Vector3 indicatorPos;
    private float rot;

    private void Start()
    {
        cam = ManagerReferences.Instance.MainCamera;
    }

    private void Update()
    {
        dist = Vector3.SqrMagnitude(cam.transform.position - transform.position);
        
        if (dist <= 40000)
        {
            
            values = DetermineIndicatorPosition(transform.position, cam.transform.position);

            if (!shipOnScreen)
            {
                indicatorPos = values.Item1;
                rot = values.Item2;

                if (!indicatorExists)
                {
                    shipIndicator =
                        Instantiate(shipIndicatorPrefab, indicatorPos, Quaternion.Euler(0, 0, rot));
                    indicatorExists = true;
                }
                else
                {
                    indicatorPos = values.Item1;
                    rot = values.Item2;
            
                    shipIndicator.transform.position = indicatorPos;
                    shipIndicator.transform.rotation = Quaternion.Euler(0, 0, rot);
                }
            }
            else if (indicatorExists)
            {
                Destroy(shipIndicator);
                indicatorExists = false;
            }
        }
        else if (indicatorExists)
        {
            Destroy(shipIndicator);
            indicatorExists = false;
        }
    }

    private Tuple<Vector3, float> DetermineIndicatorPosition(Vector3 shipPos, Vector3 camPos)
    {
        float xCamOrthographicSize = cam.orthographicSize * Screen.width / Screen.height;
        float padding = .75f;
        
        float xPositiveCamBorder = camPos.x + xCamOrthographicSize;
        float xNegativeCamBorder = camPos.x - xCamOrthographicSize;
        float yPositiveCamBorder = camPos.y + cam.orthographicSize;
        float yNegativeCamBorder = camPos.y - cam.orthographicSize;
        
        float rotation = 0;

        Vector3 dir = shipPos - camPos;
        float dirSlope = dir.y / dir.x;
        
        Vector3 spawnPos = Vector3.zero;

        float slope = cam.orthographicSize / xCamOrthographicSize;

        bool Inequality(float xPos, float yPos, float signedSlope)
        {
            if (yPos >= signedSlope * (xPos - camPos.x) + camPos.y && yPos <= -signedSlope * (xPos - camPos.x) + camPos.y) return true;
            
            return false;
        }
        
        if (shipPos.x >= xPositiveCamBorder && Inequality(shipPos.x, shipPos.y, -slope))
        {
            rotation = -90;
            float yVal = dirSlope * (xPositiveCamBorder - padding - camPos.x) + camPos.y;
            spawnPos = new Vector3(xPositiveCamBorder - padding, yVal, -3f);
        }
        else if (shipPos.x <= xNegativeCamBorder && Inequality(shipPos.x, shipPos.y, slope))
        {
            rotation = 90;
            float yVal = dirSlope * (xNegativeCamBorder + padding - camPos.x) + camPos.y;
            spawnPos = new Vector3(xNegativeCamBorder + padding, yVal, -3f);
        }
        else if (shipPos.y >= yPositiveCamBorder)
        {
            rotation = 0;
            float xVal = 1 / dirSlope * (yPositiveCamBorder - padding - camPos.y) + camPos.x;
            spawnPos = new Vector3(xVal, yPositiveCamBorder - padding, -3f);
        }
        else if (shipPos.y <= yNegativeCamBorder)
        {
            rotation = 180;
            float xVal = 1 / dirSlope * (yNegativeCamBorder + padding - camPos.y) + camPos.x;
            spawnPos = new Vector3(xVal, yNegativeCamBorder + padding, -3f);
        }
        else
        {
            shipOnScreen = true;
            return new Tuple<Vector3, float>(Vector3.zero, 0);
        }

        shipOnScreen = false;
        return new Tuple<Vector3, float>(spawnPos, rotation);
    }
}
