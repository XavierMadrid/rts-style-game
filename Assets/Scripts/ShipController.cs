using System;
using System.Collections;
using System.Collections.ObjectModel;
using UnityEngine.InputSystem;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    private PlayerControls playerControls;
    private InputAction mousePosition;
    private InputAction leftClick;
    
    private readonly float clickSelectionRange = 15f;
    private bool mouseHeldDown;
    private bool unsubscribed;
    private GameObject selectionCircleClone;
    private Camera mainCam;
    
    private Vector3 circlePoint1;
    private Vector3 circlePoint2;

    private Collider2D[] shipColliders = new Collider2D[10];
    
    [SerializeField] private Color selectedCircleColor;
    [SerializeField] private GameObject selectionCircle = null;
    [SerializeField] private GameObject shipUnit = null;

    public ObservableCollection<GameObject> ShipUnits = new();
    
    private ContactFilter2D shipsFilter;
    
    // Start is called before the first frame update
    void Awake()
    {
        shipsFilter = new ContactFilter2D() {useLayerMask = true, layerMask = LayerMask.GetMask("ShipUnits") };
        
        playerControls = new PlayerControls();
    }

    private void Start()
    {
        mainCam = ManagerReferences.Instance.MainCamera;
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

        // change to event to prevent this check every frame
        if (ManagerReferences.Instance.HexBuilder.BuildMode)
        {
            if (!unsubscribed)
            {
                leftClick.performed -= BeginShipSelecting;
                leftClick.canceled -= ChooseShipSelectionProcess;
                unsubscribed = true;
            }

            return;
        }

        if (mouseHeldDown)
        {
            circlePoint2 = mainCam.ScreenToWorldPoint(mousePosition.ReadValue<Vector2>());
            Vector3 midpoint = circlePoint1 + (circlePoint2 - circlePoint1) * .5f;
            float radius = Vector2.Distance(circlePoint1, circlePoint2) * .5f;
            selectionCircleClone.transform.position = midpoint;
            selectionCircleClone.transform.localScale = new Vector3(radius, radius, 1);
        }

        leftClick.performed += BeginShipSelecting;
        leftClick.canceled += ChooseShipSelectionProcess;
        unsubscribed = false;
    }

    private void BeginShipSelecting(InputAction.CallbackContext context)
    {
        circlePoint1 = mainCam.ScreenToWorldPoint(mousePosition.ReadValue<Vector2>());
        circlePoint1 = new Vector3(circlePoint1.x, circlePoint1.y, -3);
        selectionCircleClone = Instantiate(selectionCircle, circlePoint1, Quaternion.identity);
        mouseHeldDown = true;
    }

    private void ChooseShipSelectionProcess(InputAction.CallbackContext context)
    {
        mouseHeldDown = false;

        circlePoint2 = mainCam.ScreenToWorldPoint(mousePosition.ReadValue<Vector2>());
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
            Physics2D.OverlapCircle(midpoint, clickSelectionRange, shipsFilter, shipColliders);
            SelectClosestShip(shipColliders, circlePoint1);
        }
    }
    
    private void SelectClosestShip(Collider2D[] shipColliders, Vector3 clickPos)
    {
        float shortestDist = clickSelectionRange * clickSelectionRange + 5; // because 15^2 is 225
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

    private void OnEnable()
    {
        mousePosition = playerControls.ShipControlActions.MousePosition;
        leftClick = playerControls.ShipControlActions.SelectShip;
        playerControls.ShipControlActions.Enable();
    }

    private void OnDisable()
    {
        playerControls.ShipControlActions.Disable();;
    }
}