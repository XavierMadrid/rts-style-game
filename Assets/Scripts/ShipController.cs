using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.U2D.Path;
using UnityEngine;
using UnityEngine.Serialization;

public class ShipController : MonoBehaviour
{
    public static bool ExtendStats = false;
    
    private bool mouseHeldDown;
    private GameObject selectionCircleClone;
    
    private Vector3 circlePoint1;
    private Vector3 circlePoint2;

    private Collider2D[] shipColliders = new Collider2D[10];
    
    [SerializeField] private Color selectedCircleColor;
    
    [SerializeField] private GameObject selectionCircle = null;
    [SerializeField] private GameObject shipUnit = null;
    [SerializeField] private Camera mainCam = null;
    [SerializeField] private HexBuilder hexBuildercs = null;

    public ObservableCollection<GameObject> ShipUnits = new();
    
    public Camera MainCam { get; private set; }
    
    private ContactFilter2D shipsFilter;
    
    // Start is called before the first frame update
    void Awake()
    {
        MainCam = mainCam;
        shipsFilter = new ContactFilter2D() {useLayerMask = true, layerMask = LayerMask.GetMask("ShipUnits") };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) // VERY TEMPORARY!
        {
            Vector3 pos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            pos = new Vector3(pos.x, pos.y, -1);
            GameObject shipUnitClone = Instantiate(shipUnit, pos, Quaternion.identity);
            
            ShipUnits.Add(shipUnitClone);
        }

        if (hexBuildercs.BuildMode) return; // change to event to prevent this check every frame

        if (mouseHeldDown)
        {
            circlePoint2 = mainCam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 midpoint = circlePoint1 + (circlePoint2 - circlePoint1) * .5f;
            float radius = Vector2.Distance(circlePoint1, circlePoint2) * .5f;
            selectionCircleClone.transform.position = midpoint;
            selectionCircleClone.transform.localScale = new Vector3(radius, radius, 1);
        }

        if (Input.GetMouseButtonDown(0))
        {
            circlePoint1 = mainCam.ScreenToWorldPoint(Input.mousePosition);
            circlePoint1 = new Vector3(circlePoint1.x, circlePoint1.y, -3);
            selectionCircleClone = Instantiate(selectionCircle, circlePoint1, Quaternion.identity);
            mouseHeldDown = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            mouseHeldDown = false;

            circlePoint2 = mainCam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 midpoint = circlePoint1 + (circlePoint2 - circlePoint1) * .5f;
            float radius = Vector2.Distance(circlePoint1, circlePoint2) * .5f;
            selectionCircleClone.transform.position = midpoint;
            selectionCircleClone.transform.localScale = new Vector3(radius, radius, 1);
            StartCoroutine(SelectionCircleVFX(selectionCircleClone));

            Array.Clear(shipColliders, 0, shipColliders.Length);

            if (radius >= 1)
            {
                Physics2D.OverlapCircle(midpoint, radius, shipsFilter, shipColliders);
                SelectShips(shipColliders, midpoint);
            }
            else // select nearest ship within 15 units
            {
                Physics2D.OverlapCircle(midpoint, 15f, shipsFilter, shipColliders);
                SelectClosestShip(shipColliders, circlePoint1);
            }
        }
    }

    private void SelectClosestShip(Collider2D[] shipColliders, Vector3 clickPos)
    {
        float shortestDist = 250f; // because 15^2 is 225
        GameObject closestShip = null;
        foreach (var shipCol in shipColliders)
        {
            if (shipCol is null) continue;
            float dist = Vector3.SqrMagnitude(shipCol.transform.position - clickPos);
            if (dist < shortestDist)
            {
                shortestDist = dist;
                closestShip = shipCol.gameObject;
            }
        }

        if (closestShip is null) return;
        
        ShipUnitBehavior sb = closestShip.GetComponent<ShipUnitBehavior>();
        sb.Offset = Vector3.zero;
        sb.Controlled = true;
    }
    
    private void SelectShips(Collider2D[] shipColliders, Vector3 selectionCircleCenter)
    {
        foreach (Collider2D shipCol in shipColliders)
        {
            if (shipCol == null) return;
            
            Vector3 offset = selectionCircleCenter - shipCol.transform.position;
            
            ShipUnitBehavior sb = shipCol.gameObject.GetComponent<ShipUnitBehavior>();
            sb.Offset = Vector3.ClampMagnitude(offset, 8);
            sb.Controlled = true;
        }
    }

    private IEnumerator SelectionCircleVFX(GameObject circle)
    {
        SpriteRenderer circleSr = circle.GetComponent<SpriteRenderer>();
        circleSr.color = selectedCircleColor;
        Color currentColor = circleSr.color;
        float startAlpha = currentColor.a;
        float timeElapsed = 0;
        float lerpDuration = .75f;
        while (timeElapsed < lerpDuration)
        {
            float a = Mathf.Lerp(startAlpha, 0, timeElapsed);
            circleSr.color = new Color(currentColor.r, currentColor.g, currentColor.b, a);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(circle);
    }
}