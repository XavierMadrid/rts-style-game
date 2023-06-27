using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerHex : EnergyHex
{
    private SpriteRenderer sr;
    private readonly Color poweredColor = new(0.2972f, 0.7023f, 1, 1f);
    
    protected override void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = HexBuilder.POWER_HEX.HexColor;
        
        base.Start();
    }

    protected override void HexActivation(bool value)
    {
        StartCoroutine(value
            ? FadeToColor(HexBuilder.POWER_HEX.HexColor, poweredColor)
            : FadeToColor(poweredColor, HexBuilder.POWER_HEX.HexColor));

        if (value)
        {
            foreach (var hexObject in linkedHexes)
            {
                if (hexObject.TryGetComponent<EnergyHex>(out var energyHex))
                {
                    energyHex.HexActivated = true;
                }
                else
                {
                    // Debug.LogError($"Failed to get the EnergyHex script from a linked object {hexObject}");
                }
            }
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
}
