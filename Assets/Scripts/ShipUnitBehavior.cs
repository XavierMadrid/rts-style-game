using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class ShipUnitBehavior : ShipBehavior
{
    private Camera mainCam;
    private Collider2D col2D;

    private StarMovement lastActiveStarObject;
    private ShipController shipControllercs;
    public Vector3 Offset { get; set; }

    private SpriteRenderer sr;
    [SerializeField] private Color controlledColor = new(1, .941f, .588f, 1);
    [SerializeField] private Color busyColor = new(1, .741f, 0f, 1);
    [FormerlySerializedAs("normalColor")] [SerializeField] private Color idleColor = new(.49f, .443f, .173f, 1);


    private readonly float movementSpeedConstant = .001f;
    private float speed = 0.05f;
    private bool controlled;
    private bool cancelShipMoveTrigger = false;
    private int postLinkMoveCount = 2;

    public bool Controlled
    {
        get => controlled;
        set
        {
            if (value && !controlled)
            {
                sr.color = controlledColor;
            }

            controlled = value;
        }
    }
    
    private bool performingTask;
    private float collectionRange = 15f;

    private GameObject activeTaskObject;

    private ContactFilter2D noFilter;
    private Collider2D[] results = new Collider2D[10]; // could be too many spots

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        mainCam = ManagerReferences.Instance.MainCamera;
        col2D = GetComponent<Collider2D>();
        
        noFilter = new ContactFilter2D();

        sr.color = idleColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (!controlled) return;

        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector3(mousePos.x, mousePos.y, -1f);

        Vector3 dir = (mousePos - transform.position).normalized;
        ShipRotationRaw = Mathf.Atan2(dir.y, dir.x);

        transform.rotation = Quaternion.Euler(0, 0, ShipRotationRaw * Mathf.Rad2Deg - 90);
        
        if (Input.GetMouseButtonDown(1))
        {
            MoveCommand(mousePos);
        }
    }

    private void MoveCommand(Vector3 mousePos)
    {
        speed = .05f;
        
        controlled = false;
        cancelShipMoveTrigger = true;

        if (activeTaskObject is not null)
        {
            bool overrideNull = false;
            
            if (activeTaskObject.TryGetComponent<Planet>(out var planet))
            {
                planet.StopHarvest();
                planet.OnPlanetDepleted -= HarvestCommandCompleted;
            }
            else if (activeTaskObject.TryGetComponent<WorkshopHex>(out var workshopHex))
            {
                workshopHex.StopShipSpawning();
                workshopHex.OnShipCreated -= SpawnShipCommandCompleted;
            }
            else if (activeTaskObject.TryGetComponent<StarMovement>(out var star))
            {
                star.StartMoveStar(transform);
                speed = .1f;
                overrideNull = true;
            }
            
            if (!overrideNull) activeTaskObject = null;
        }

        Vector3 targetPos = new Vector3(mousePos.x - Offset.x, mousePos.y - Offset.y, -1); // wrong
        
        float dist = Vector3.Distance(transform.position, targetPos);

        StartCoroutine(MoveShip(targetPos, dist, speed));
    }

    private bool CheckForIdleTasks()
    {
        Array.Clear(results, 0, results.Length);
        Physics2D.OverlapCircle(transform.position, collectionRange, noFilter, results);
        foreach (var col in results)
        {
            if (col == null) continue;
            
            if (col.gameObject.TryGetComponent<Planet>(out var planet))
            {
                if (planet.TryHarvest())
                {
                    activeTaskObject = col.gameObject;
                    planet.OnPlanetDepleted += HarvestCommandCompleted;
                    return true;
                }
            }
            if (col.gameObject.TryGetComponent<WorkshopHex>(out var workshopHex))
            {
                if (workshopHex.TrySpawnShip())
                {
                    activeTaskObject = col.gameObject;
                    workshopHex.OnShipCreated += SpawnShipCommandCompleted;
                    return true;
                }
            }
            if (col.gameObject.TryGetComponent<StarMovement>(out var star) && postLinkMoveCount >= 1)
            {
                (bool success, float speed) = star.TryConnectToStar(col2D);
                if (success)
                {
                    activeTaskObject = col.gameObject;
                    lastActiveStarObject = star;
                    this.speed = speed;
                    postLinkMoveCount = 0;
                    return true;
                }
            }
        }

        return false;
    }

    private void HarvestCommandCompleted(object sender, EventArgs e)
    {
        activeTaskObject.GetComponent<Planet>().OnPlanetDepleted -= HarvestCommandCompleted;

        performingTask = CheckForIdleTasks();
        StartCoroutine(performingTask ? FadeToColor(busyColor) : FadeToColor(idleColor));
    }
    
    private void SpawnShipCommandCompleted(object sender, EventArgs e)
    {
        activeTaskObject.GetComponent<WorkshopHex>().OnShipCreated -= SpawnShipCommandCompleted;

        performingTask = CheckForIdleTasks();
        StartCoroutine(performingTask ? FadeToColor(busyColor) : FadeToColor(idleColor));
    }

    private void MoveCommandCompleted(bool pass = true)
    {
        if (cancelShipMoveTrigger) return; // redirecting ships does not require rechecking for tasks, hence the return
        
        postLinkMoveCount++;
        if (postLinkMoveCount == 1 && lastActiveStarObject is not null) lastActiveStarObject.StopMovingStar();
        
        performingTask = CheckForIdleTasks();
        StartCoroutine(performingTask ? FadeToColor(busyColor) : FadeToColor(idleColor));

        if (!pass)
        {
            throw new TargetException("Command not completed properly.");
        }
    }
    
    private IEnumerator MoveShip(Vector3 targetPos, float dist, float speed)
    {
        yield return null;
        cancelShipMoveTrigger = false;
        float endingSpeed = 1;
        
        float duration = dist * speed;
        float timeElapsed = 0;
        
        while (!cancelShipMoveTrigger)
        {
            Vector3 currentPos = transform.position;

            if (Vector3.SqrMagnitude(transform.position - targetPos) <= .00125f) // slowing at the end
            {
                transform.position = Vector3.Lerp(currentPos, targetPos, endingSpeed * movementSpeedConstant);
                endingSpeed -= 2 * Time.deltaTime;

                if (endingSpeed < 0) // stop
                {
                    transform.position = targetPos;
                    break;
                }
            }
            else // default lerp
            {
                transform.position = Vector3.Lerp(currentPos, targetPos, timeElapsed * movementSpeedConstant / duration);
            }
            
            timeElapsed += Time.deltaTime;
            yield return null;

            if (timeElapsed >= 30)
            {
                MoveCommandCompleted(false);
                break;
            }
        }

        MoveCommandCompleted();
    }

    private IEnumerator FadeToColor(Color targetColor)
    {
        float timeElapsed = 0;
        float lerpDuration = .5f;
        while (timeElapsed < lerpDuration)
        {
            sr.color = Color.Lerp(controlledColor, targetColor, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            if (Controlled) yield break;
            yield return null;
        }
        sr.color = targetColor;
    }

    private void OnDisable()
    {
        if (activeTaskObject != null)
        {
            if (activeTaskObject.TryGetComponent<Planet>(out var planet))
                planet.OnPlanetDepleted -= HarvestCommandCompleted;
            else if (activeTaskObject.TryGetComponent<WorkshopHex>(out var workshopHex))
                workshopHex.OnShipCreated -= SpawnShipCommandCompleted;
        }
    }
}
