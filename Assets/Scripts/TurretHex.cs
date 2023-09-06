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

    private bool energySupplied;
    
    protected override void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        turretHexsb = GetComponent<TurretHexShootBehavior>();

        
        StartCoroutine(PassiveHealthRegen());
        
        base.Start();
    }

    protected override void HexActivation(bool value)
    {
        energySupplied = value;
        ObjectTargetable(value);
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
    
    protected override void ObjectTargetable(bool isTargetable)
    {
        if (isTargetable)
        {
            if (energySupplied && HealthPositive)
            {
                if (Disabled)
                {
                    turretHexsb.enabled = true;
                    base.ObjectTargetable(true);
                    
                    Debug.Log($"ObjectTargetable({isTargetable}): energySupplied: {energySupplied}, AtMaxHealth: {HealthPositive}, Disabled: {Disabled}; true");

                    StartCoroutine(FadeToColor(HexBuilder.TURRET_HEX.HexColor, poweredColor));
                }
            }
        }
        else if (!Disabled)
        {
            turretHexsb.enabled = false;
            base.ObjectTargetable(false);
            
            Debug.Log($"ObjectTargetable({isTargetable}): energySupplied: {energySupplied}, AtMaxHealth: {HealthPositive}, Disabled: {Disabled}; false");
            
            StartCoroutine(FadeToColor(poweredColor, HexBuilder.TURRET_HEX.HexColor));
        }
        
        
        
        
        // if (energySupplied && HealthPositive && Disabled)
        // {
        //     turretHexsb.enabled = true;
        //     base.ObjectTargetable(true);
        //     
        //     StartCoroutine(FadeToColor(HexBuilder.TURRET_HEX.HexColor, poweredColor));
        // }
        // else if ((!energySupplied || !HealthPositive) && !Disabled)
        // {
        //     turretHexsb.enabled = false;
        //     base.ObjectTargetable(false);
        //     
        //     StartCoroutine(FadeToColor(poweredColor, HexBuilder.TURRET_HEX.HexColor));
        // }
    }
}
