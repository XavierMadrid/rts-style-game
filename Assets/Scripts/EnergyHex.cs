using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyHex : HexTypeBehavior
{
    protected List<GameObject> linkedHexes = new ();
    
    private const int linkRange = 3; // is multiplied by 6 to account for hex position intervals of 6
    private int activatedHexesCount;
    private bool hexActivated;

    public virtual bool HexActivated
    {
        get => hexActivated;
        set
        {
            if (value != hexActivated)
            {
                Debug.Log($"{transform.position}: HexActivated bool: {value}");
                HexActivation(value);
                hexActivated = value;
            }
        }
    }
    
    private Hex transformHexPos;
    
    protected virtual void Start()
    {
        transformHexPos = transform.position.ToHex();
        
        InitialSearchForLinks();
    }


    // Start is called before the first frame update
    private void OnEnable()
    {
        ManagerReferences.Instance.HexBuilder.OnEnergyHexPlaced += EnergyHexPlaced;
    }

    protected virtual void EnergyHexPlaced(GameObject hexObject, Hex newHexPos)
    {
        activatedHexesCount = 0;
        if (transformHexPos.DistanceTo(newHexPos) > linkRange * 6) return;
        
        for (int i = 0; i < linkedHexes.Count; i++)
        {
            if (linkedHexes[i] != null) continue;
            
            linkedHexes[i] = hexObject;

            if (linkedHexes[i] == ManagerReferences.Instance.HexBuilder.CenterHex ||
                linkedHexes[i].GetComponent<EnergyHex>().hexActivated) activatedHexesCount++;

            break;
        }

        if (activatedHexesCount > 0)
        {
            HexActivated = true;
        }
    }

    private void InitialSearchForLinks()
    {
        activatedHexesCount = 0;
        foreach (var keyValuePair in ManagerReferences.Instance.HexBuilder.EnergyHexPosDict)
        {
            if (transformHexPos.DistanceTo(keyValuePair.Key) > linkRange * 6 || transformHexPos.Equals(keyValuePair.Key)) continue; // is the hex in range

            linkedHexes.Add(keyValuePair.Value);
            
            if (keyValuePair.Value.transform.position.ToHex().Equals(ManagerReferences.Instance.HexBuilder.CenterHex.transform.position.ToHex()) || 
                keyValuePair.Value.GetComponent<EnergyHex>().hexActivated)
            {
                activatedHexesCount++;
            }
        }
        Debug.Log("Activated Hex Count: " + activatedHexesCount);
        HexActivated = activatedHexesCount > 0;
    }

    protected virtual void HexActivation(bool value)
    {
        
    }

    private void OnDisable()
    {
        ManagerReferences.Instance.HexBuilder.OnEnergyHexPlaced -= EnergyHexPlaced;
    }
}
