using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerHex : MonoBehaviour
{
    private List<GameObject> linkedHexes = new ();
    private SpriteRenderer sr;
    private readonly Color poweredColor = new(0.2972f, 0.7023f, 1, 1f);
    
    private const int linkRange = 18;
    private int activatedHexesCount;
    private bool hexActivated;

    private bool HexActivated
    {
        get => hexActivated;
        set
        {
            if (value != hexActivated)
            {
                HexActivation(value);
                hexActivated = value;
            }
        }
    }
    
    private Hex transformHexPos;
    
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = HexBuilder.POWER_HEX.HexColor;
        
        transformHexPos = transform.position.ToHex();
        
        InitialSearchForLinks();
    }


    // Start is called before the first frame update
    private void OnEnable()
    {
        ManagerReferences.Instance.HexBuilder.OnEnergyHexPlaced += EnergyHexPlaced;
    }

    private void EnergyHexPlaced(GameObject hexObject, Hex newHexPos)
    {
        activatedHexesCount = 0;
        if (transformHexPos.DistanceTo(newHexPos) > linkRange) return;
        
        for (int i = 0; i < linkedHexes.Count; i++)
        {
            if (linkedHexes[i] != null) continue;
            
            linkedHexes[i] = hexObject;

            if (linkedHexes[i] == ManagerReferences.Instance.HexBuilder.CenterHex ||
                linkedHexes[i].GetComponent<PowerHex>().hexActivated) activatedHexesCount++;

            break;
        }

        if (activatedHexesCount > 0) HexActivated = true;
    }

    private void InitialSearchForLinks()
    {
        activatedHexesCount = 0;
        foreach (var keyValuePair in ManagerReferences.Instance.HexBuilder.EnergyHexPosDict)
        {
            Debug.Log(keyValuePair.Key, keyValuePair.Value);
            Debug.Log("transform: " + transformHexPos);
            Debug.Log(keyValuePair.Value);
            Debug.Log(keyValuePair.Value.transform.position.ToHex());
            Debug.Log(transformHexPos.DistanceTo(keyValuePair.Key));
            if (transformHexPos.DistanceTo(keyValuePair.Key) > linkRange || transformHexPos.Equals(keyValuePair.Key))
            {
                Debug.Log("failed");
                continue; // is the hex in range
            }

            linkedHexes.Add(keyValuePair.Value);
            
            if (keyValuePair.Value.transform.position.ToHex().Equals(ManagerReferences.Instance.HexBuilder.CenterHex.transform.position.ToHex()) || 
                keyValuePair.Value.GetComponent<PowerHex>().HexActivated)
            {
                Debug.Log("true");
                activatedHexesCount++;
            }
        }

        HexActivated = activatedHexesCount > 0;
    }

    private void HexActivation(bool value)
    {
        StartCoroutine(value
            ? FadeToColor(HexBuilder.POWER_HEX.HexColor, poweredColor)
            : FadeToColor(poweredColor, HexBuilder.POWER_HEX.HexColor));

        foreach (var hexObject in linkedHexes)
        {
            if (hexObject.TryGetComponent<PowerHex>(out var powerHex))
            {
                powerHex.HexActivated = true;
            }
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
    
    private void OnDisable()
    {
        ManagerReferences.Instance.HexBuilder.OnEnergyHexPlaced -= EnergyHexPlaced;
    }
}
