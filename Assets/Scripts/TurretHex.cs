using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class TurretHex : EnergyHex
{
    protected Color ActiveColor = new(1, 0.8366f, 0, 1f);
    protected Color DefaultColor = HexBuilder.TURRET_HEX.HexColor;
    
    protected int StartingHealth = 7;
    protected int HealthPerSec = 1;
    
    private SpriteRenderer sr;
    private ShootBehavior shootBehaviorcs;

    private bool energySupplied;
    
    protected override void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        shootBehaviorcs = GetComponent<ShootBehavior>();
        
        StartCoroutine(PassiveHealthRegen(StartingHealth, HealthPerSec));
        
        base.Start();
    }

    protected override void HexActivation(bool value)
    {
        energySupplied = value;
        ObjectTargetable(value);
    }

    protected override void ObjectTargetable(bool isTargetable)
    {
        if (isTargetable != Disabled) return;

        if (isTargetable)
        {
            if (energySupplied && HealthPositive)
            {
                shootBehaviorcs.enabled = true;
                Debug.Log("Enabled");
                // Debug.Log($"ObjectTargetable({isTargetable}): energySupplied: {energySupplied}, AtMaxHealth: {HealthPositive}, Disabled: {Disabled}; true");
                base.ObjectTargetable(true);
                
                StartCoroutine(FadeToColor(DefaultColor, ActiveColor));
            }
            // fall through
        }
        else
        {
            shootBehaviorcs.enabled = false;
            Debug.Log("disabled");
            base.ObjectTargetable(false);
            
            // Debug.Log($"ObjectTargetable({isTargetable}): energySupplied: {energySupplied}, AtMaxHealth: {HealthPositive}, Disabled: {Disabled}; false");

            StartCoroutine(FadeToColor(ActiveColor, DefaultColor));
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
