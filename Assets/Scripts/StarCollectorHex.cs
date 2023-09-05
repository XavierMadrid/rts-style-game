using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCollectorHex : HexTypeBehavior
{
    private bool currentlyHarvesting;
    
    [SerializeField] private Color collectingColor = new(.9811f, .3993f, 0, 1);
    private SpriteRenderer sr;
    private Star starcs;
    
    private Collider2D[] colliderResults = new Collider2D[10];
    private ContactFilter2D stars;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        stars = new ContactFilter2D() {layerMask = LayerMask.GetMask("CelBodies")};

        CheckForNearbyStars();
    }

    private void StarMoved(object sender, EventArgs e)
    {
        CheckForNearbyStars();
    }
    
    private void CheckForNearbyStars()
    {
        if (currentlyHarvesting) return;
        
        Physics2D.OverlapCircle(transform.position, 8f, stars, colliderResults);
        foreach (var col in colliderResults)
        {
            if (col is null) return;
            if (col.gameObject.TryGetComponent<Star>(out var star))
            {
                if (star.HarvestStar(gameObject))
                {
                    starcs = star;
                    currentlyHarvesting = true;
                    star.OnStarDepleted += StarDepleted;

                    StartCoroutine(FadeToColor(collectingColor));
                    
                    return;
                }
            }
        }
    }

    private void StarDepleted(object sender, EventArgs e)
    {
        starcs.OnStarDepleted -= StarDepleted;
        
        currentlyHarvesting = false;
        CheckForNearbyStars();
        StartCoroutine(FadeToColor(HexBuilder.STAR_COLLECTOR_HEX.HexColor));
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

    private void OnEnable()
    {
        StarMovement.OnStarMoved += StarMoved;
    }

    private void OnDisable()
    {
        StarMovement.OnStarMoved -= StarMoved;
    }
}
