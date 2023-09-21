using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTurretHex : TurretHex
{
    // Start is called before the first frame update
    void Awake()
    {
        MaxHealth = 5;
        StartingHealth = 0;
        HealthPerSec = 1;
        DefaultColor = HexBuilder.MISSILE_HEX.HexColor;
        ActiveColor = new Color(1f, 0.7348f, 0.4009f, 1f);
    }
}
