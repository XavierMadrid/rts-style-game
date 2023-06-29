using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class EnemyShootBehavior : ShootBehavior
{
    protected override void Shoot(Transform target, int damage, float shotAngle)
    {
        damage = GetComponent<Ship>().DamageStrength;
        shotAngle = (shotAngle + 90) * Mathf.Deg2Rad;

        base.Shoot(target, damage, shotAngle);
    }

    protected override ObservableCollection<GameObject> GetTargetableObjects() => ManagerReferences.Instance.ShipController.ShipUnits;
}
