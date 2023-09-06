using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyShootBehavior : ShootBehavior
{
    ObservableCollection<GameObject> targetableObjects = new();

    protected override void Shoot(Transform target, int damage, float shotAngle)
    {
        damage = GetComponent<Ship>().DamageStrength;
        shotAngle = (shotAngle + 90) * Mathf.Deg2Rad;

        base.Shoot(target, damage, shotAngle);
    }

    // private bool once
    
    protected override ObservableCollection<GameObject> GetTargetableObjects()
    {
        targetableObjects.Clear();

        // Add all ships to targetable objects
        foreach (var obj in ManagerReferences.Instance.ShipController.ShipUnits)
        {
            Debug.Log($"Added Ship {obj} to targetable objects");
            targetableObjects.Add(obj);
        }
        
        // Add all targetable hexes to targetable objects
        foreach (var obj in ManagerReferences.Instance.TargetableHexIdentifier.TargetableHexes)
        {
            Debug.Log($"Added Hex {obj} to targetable objects");
            targetableObjects.Add(obj);
        }
        
        // For debugging
        string output = $"{gameObject}:: ";
        foreach (var targetable in targetableObjects)
        {
            output += targetable + " : ";
        }

        Debug.Log(output);
        // --- //
        
        return targetableObjects;
    }
}
