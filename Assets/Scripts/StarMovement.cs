using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = .05f;
    private CircleCollider2D col2D;
    private SpriteRenderer sr;
    private bool stopStarMovementTrigger;
    private bool currentlyConnected;
    private float radius;

    private Collider2D[] shipCols = new Collider2D[5];
    private ContactFilter2D shipsFilter;
    private Color originalColor;
    private Color linkedColor;
    float colorTint = .2f;

    public static event EventHandler<EventArgs> OnStarMoved;
    
    private void Awake()
    {
        col2D = GetComponent<CircleCollider2D>();
        radius = col2D.radius;
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        linkedColor = new(originalColor.r + colorTint, originalColor.g + colorTint, originalColor.b + colorTint, 1);
        
        shipsFilter = new ContactFilter2D() {useLayerMask = true, layerMask = LayerMask.GetMask("ShipUnits") };
    }

    public (bool success, float speed) TryConnectToStar(Collider2D shipCol)
    {
        if (currentlyConnected) return (false, 0.05f);

        Debug.Log(col2D.IsTouching(shipCol));
        
        Array.Clear(shipCols, 0, shipCols.Length);
        Physics2D.OverlapCircle(transform.position, radius, shipsFilter, shipCols);

        foreach (var col in shipCols)
        {
            if (ReferenceEquals(col, shipCol))
            {
                currentlyConnected = true;
                stopStarMovementTrigger = false;

                StartCoroutine(FadeToColor(linkedColor));

                return (true, movementSpeed);
            }
        }

        return (false, 0.05f);
    }

    public void StartMoveStar(Transform shipTransform)
    {
        StartCoroutine(FollowShip(shipTransform));
    }

    private IEnumerator FollowShip(Transform shipTransform)
    {
        float timeElapsed = 0;
        float lerpDuration = 30; 

        while (timeElapsed < lerpDuration && !stopStarMovementTrigger)
        {
            Vector3 currentPos = transform.position;
            Vector3 targetPos = new Vector3(shipTransform.position.x, shipTransform.position.y, currentPos.z);
            transform.position = Vector3.Lerp(currentPos, targetPos, timeElapsed / 1000); // XD

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        OnStarMoved?.Invoke(this, EventArgs.Empty);
        
        StartCoroutine(FadeToColor(originalColor));
        currentlyConnected = false;
    }
    
    private IEnumerator FadeToColor(Color targetColor)
    {
        float timeElapsed = 0;
        float lerpDuration = .75f;
        Color startColor = sr.color;
        while (timeElapsed < lerpDuration)
        {
            sr.color = Color.Lerp(startColor, targetColor, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        sr.color = targetColor;
    }

    public void StopMovingStar() => stopStarMovementTrigger = true;
}
