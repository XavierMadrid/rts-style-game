using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class TargetableHexIdentifier : MonoBehaviour
{
    private bool hexFound = false;
    
    public ObservableCollection<GameObject> TargetableHexes = new();
    
    private void HexDisabled(GameObject hexObject, bool isTargetable)
    {
        hexFound = false;
        if (!isTargetable)
        {
            Hex hexPos = hexObject.transform.position.ToHex(); // get position of disabled hex
            
            foreach (var targetable in TargetableHexes)
            {
                if (hexPos.Equals(targetable.transform.position.ToHex()))
                {
                    TargetableHexes.Remove(hexObject);
                    hexFound = true;
                    
                    break;
                }
            }

            Debug.Log("HexFound: " + hexFound);
            
        }
        else
        {
            TargetableHexes.Add(hexObject);
        }
    }
    
    private void DisableableHexPlaced(Hex hexPos, GameObject hexObject)
    {
        Debug.Log(hexObject + ": Hex event subscribed to HexDisabled.");
        hexObject.GetComponent<Targetable>().OnObjectTargetable += HexDisabled;
    }

    private void Start()
    {
        ManagerReferences.Instance.HexBuilder.OnDisableableHexPlaced += DisableableHexPlaced;
    }

    private void OnDisable()
    {
        ManagerReferences.Instance.HexBuilder.OnDisableableHexPlaced -= DisableableHexPlaced;
    }
}
