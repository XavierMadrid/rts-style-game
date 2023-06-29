using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class TurretHexShootBehavior : ShootBehavior
{
    private int energyDrain = -1;
    
    protected override ObservableCollection<GameObject> GetTargetableObjects() => ManagerReferences.Instance.EnemyHandler.EnemyShips;

    private void Awake()
    {
        enabled = false;
    }

    protected override void Shoot(Transform target, int damage, float shotAngle)
    {
        damage = 1;
        Vector3 dir = target.position - transform.position;
        shotAngle = Mathf.Atan2(dir.y, dir.x);
        
        ResourceManager.ENERGY.AddAmountToResource(energyDrain);
        
        base.Shoot(target, damage, shotAngle);
    }

    protected override void RotateTowardsTarget(Transform targetTransform)
    {
        // hexes themselves do not rotate. However, perhaps an animation should in the future.
    }
}
