using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

public class ForestPlanet : Planet
{
    void Awake()
    {
        base.maxYield = 30;
        base.harvestAmount = 1;
        base.harvestDelayTime = 1;
        base.resourceToHarvest = ResourceManager.GREENERY;
    }
}
