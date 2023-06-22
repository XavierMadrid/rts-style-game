using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorkshopHex : MonoBehaviour
{
    [SerializeField] protected GameObject shipUnit = null;
    protected SpriteRenderer sr;

    [SerializeField] private Color shipAliveColor = new(0, .8f, .067f, 1);
    
    private bool idleShipInRange;
    public bool IdleShipInRange
    {
        get => idleShipInRange;
        set
        {
            if (!value && idleShipInRange)
            {
                StartCoroutine(UndoTimeProgress());
            }
            
            idleShipInRange = value;
        }
    }
    
    protected bool shipAlive;
    public bool ShipAlive
    {
        get => shipAlive;
        set => shipAlive = ShipAliveSet(value);
    }
    
    private bool paid;
    private float time = 30f;

    ResourcePrice spawnPrice = new(ResourceManager.GREENERY, 15);

    public event EventHandler<EventArgs> OnShipCreated;

    protected void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    
    protected virtual bool ShipAliveSet(bool value)
    {
        if (!value)
        {
            paid = false;
            time = 30f;
            StartCoroutine(FadeToColor(shipAliveColor, HexBuilder.WORKSHOP_HEX.HexColor));
        }
        else
        {
            StartCoroutine(FadeToColor(HexBuilder.WORKSHOP_HEX.HexColor, shipAliveColor));
        }
        
        return value;
    }
    
    public virtual bool TrySpawnShip()
    {
        if (IdleShipInRange || ShipAlive) return false;
        
        IdleShipInRange = true;

        if (spawnPrice.TryPurchaseWithResourcePrice() && !paid)
        {
            ResourceManager.GREENERY.AddAmountToResource(-spawnPrice.Cost);
            paid = true;
        }
        
        if (paid)
        {
            StartCoroutine(SpawnShipAfterTime());
            return true;
        }

        return false;
    }

    private IEnumerator SpawnShipAfterTime()
    {
        while (IdleShipInRange && !ShipAlive)
        {
            time -= Time.deltaTime;

            if (time <= 0)
            {
                SpawnShipUnit(Random.Range(0f, 360f));
                
                OnShipCreated?.Invoke(this, EventArgs.Empty);
            }
            
            yield return null;
        }
    }

    private IEnumerator UndoTimeProgress()
    {
        yield return null;
        
        while (time < 30f && !IdleShipInRange)
        {
            time += Time.deltaTime;

            if (time > 30f) time = 30f;
            
            yield return null;
        }
    }
    
    protected void SpawnShipUnit(float randRotation)
    {
        GameObject shipUnitClone = Instantiate(shipUnit, transform.position, Quaternion.Euler(0, 0, randRotation - 90));
        ManagerReferences.Instance.ShipController.ShipUnits.Add(shipUnitClone);
        shipUnitClone.GetComponent<ShipUnit>().WorkshopHexHome = this;

        ShipAlive = true;

        Vector3 targetPos = new Vector3(4 * Mathf.Cos(randRotation * Mathf.Deg2Rad), 4 * Mathf.Sin(randRotation * Mathf.Deg2Rad), -1f);

        StartCoroutine(SpawnEjectionMovement(shipUnitClone, targetPos));
    }

    protected IEnumerator SpawnEjectionMovement(GameObject shipUnit, Vector3 targetPos)
    {
        float lerpDuration = 1f;
        float timeElapsed = 0f;
        
        Vector3 startPos = shipUnit.transform.position;
        targetPos += startPos;
        
        while (timeElapsed < lerpDuration)
        {
            float t = timeElapsed / lerpDuration;
            t = Mathf.Sin(.5f * t * Mathf.PI);
            
            if (shipUnit != null) shipUnit.transform.position = Vector3.Lerp(startPos, targetPos, t);
            
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
    
    private IEnumerator FadeToColor(Color startColor, Color targetColor)
    {
        float timeElapsed = 0;
        float lerpDuration = .75f;
        while (timeElapsed < lerpDuration)
        {
            sr.color = Color.Lerp(startColor, targetColor, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        sr.color = targetColor;
    }

    public void StopShipSpawning(float t = 0) => IdleShipInRange = false;
}
