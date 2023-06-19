using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

public class MetalPlanet : Planet
{
    void Awake()
    {
        base.maxYield = 90;
        base.harvestAmount = 3;
        base.harvestDelayTime = 1;
        base.resourceToHarvest = ResourceManager.METAL;
    }
}
