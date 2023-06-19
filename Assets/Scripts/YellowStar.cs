using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowStar : Star
{
    void Awake()
    {
        base.MaxYield = 240;
        base.harvestAmount = 2;
        base.harvestDelayTime = 1f;
    }
}
