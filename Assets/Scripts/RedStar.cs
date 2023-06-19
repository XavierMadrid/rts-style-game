using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedStar : Star
{
    void Awake()
    {
        base.MaxYield = 78;
        base.harvestAmount = 3;
        base.harvestDelayTime = 1f;
    }
}
