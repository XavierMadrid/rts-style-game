using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyShootBehavior : ShootBehavior
{
    protected override void Shoot(Transform target, int damage, float shotAngle)
    {
        damage = GetComponent<Ship>().DamageStrength;
        shotAngle = (shotAngle + 90) * Mathf.Deg2Rad;

        base.Shoot(target, damage, shotAngle);
    }

    // private bool once;
    
    protected override ObservableCollection<GameObject> GetTargetableObjects()
    {
        ObservableCollection<GameObject> targetableObjects = new();

        targetableObjects = ManagerReferences.Instance.ShipController.ShipUnits;

        targetableObjects.AddRange(ManagerReferences.Instance.TargetableHexIdentifier.TargetableHexes);

        string output = "";
        foreach (var targetable in targetableObjects)
        {
            output += targetable + " : ";
        }

        Debug.Log(output);
        
        
        return targetableObjects;
    }
}
