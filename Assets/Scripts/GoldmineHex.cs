using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldmineHex : HexTypeBehavior
{
    private WaitForSeconds goldGenDelay = new(1f);
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GenerateGoldResource());
    }

    private IEnumerator GenerateGoldResource()
    {
        while (!Equals(gameObject, null))
        {
            yield return goldGenDelay;
            ResourceManager.GOLD.AddAmountToResource(1);
        }
    }
}
