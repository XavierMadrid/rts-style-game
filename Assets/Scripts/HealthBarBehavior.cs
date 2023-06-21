using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HealthBarBehavior : MonoBehaviour
{
    [HideInInspector] public int MaxSegmentNumber;
    [HideInInspector] public int BarNumber;
    [HideInInspector] public Transform ShipToFollow;
    [HideInInspector] public bool ShipAssigned = false;
    
    private RectTransform rect;
    private bool changedSize = false;
    private float oldXScale = 0;
    private float adjustment = 0;
    
    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ShipToFollow == null)
        {
            Destroy(gameObject);
            return;
        }
        if (ShipAssigned) FollowShip();
    }

    private void FollowShip()
    {
        if (!changedSize)
        {
            oldXScale = Mathf.Abs(rect.localScale.x);
            rect.localScale = new Vector3(rect.localScale.x * 5 / MaxSegmentNumber,
                rect.localScale.y, rect.localScale.z);
            adjustment = (oldXScale - Mathf.Abs(rect.localScale.x)) * 10;
            changedSize = true;
        }
        // float xShift = ManagerReferences.Instance.UIManager.SCALAR;

        Vector3 pos = ShipToFollow.position;
        pos += new Vector3(-1.4f + BarNumber * .6f - adjustment * BarNumber, 1.3f, 0);
        
        transform.position = pos;

        // RectTransform rect = GetComponent<RectTransform>();
        //
        // Vector2 sizeDelta = rect.sizeDelta;
        //
        // Vector2 viewpointPos = ManagerReferences.Instance.MainCamera.WorldToViewportPoint(pos);
        // Vector2 viewpointAdjusted = new Vector2(
        //     sizeDelta.x * (viewpointPos.x - 0.5f),
        //     sizeDelta.y * (viewpointPos.y - 0.5f));
    }
}
