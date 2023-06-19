using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeStar : Star
{
    void Awake()
    {
        base.MaxYield = 600;
        base.harvestAmount = 2;
        base.harvestDelayTime = 1f;
    }
}
