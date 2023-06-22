using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CenterHex : WorkshopHex
{
    private int initialUnitCount = 3;
    private int shipsAliveCount;
    
    public int ShipsAliveCount
    {
        get => shipsAliveCount;
        set => shipsAliveCount = Mathf.Clamp(value, 0, initialUnitCount);
    }

    private WaitForSeconds oneSecDelay = new(1);
    
    protected override bool ShipAliveSet(bool value)
    {
        if (value && ShipsAliveCount >= 3) return true;
        if (value && ShipsAliveCount < 3)
        {
            ShipsAliveCount++;
            return false;
        }

        ShipsAliveCount--;
        StartCoroutine(SpawnDelayAnimation());
        return false;
    }

    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();
        
        float[] randRots = new float[initialUnitCount];

        randRots[0] = Random.Range(0f, 360f);

        for (int i = 1; i < initialUnitCount; i++) randRots[i] = randRots[0] + i * 120f;
        for (int i = 0; i < initialUnitCount; i++) SpawnShipUnit(randRots[i]);
    }

    // ship units do not need to be next to the center hex to respawn (nor does the player need to pay)
    public override bool TrySpawnShip() => false;

    private IEnumerator SpawnDelayAnimation()
    {
        StartCoroutine(FadeToColor(HexBuilder.CENTER_HEX.HexColor, Color.white));
        
        yield return oneSecDelay;
        yield return oneSecDelay;
        yield return oneSecDelay;
        
        StartCoroutine(FadeToColor(Color.white, HexBuilder.CENTER_HEX.HexColor));
        
        SpawnShipUnit(Random.Range(0f, 360f));
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
}
