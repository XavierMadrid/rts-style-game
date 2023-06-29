using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class TurretHex : EnergyHex
{
    private readonly Color poweredColor = new(1, 0.8366f, 0, 1f);
    
    private SpriteRenderer sr;
    private TurretHexShootBehavior turretHexsb;

    protected override void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        turretHexsb = GetComponent<TurretHexShootBehavior>();
        
        base.Start();
    }

    protected override void HexActivation(bool value)
    {
        turretHexsb.enabled = value;
        
        StartCoroutine(value ? FadeToColor(HexBuilder.TURRET_HEX.HexColor, poweredColor) 
            : FadeToColor(poweredColor, HexBuilder.TURRET_HEX.HexColor));
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
