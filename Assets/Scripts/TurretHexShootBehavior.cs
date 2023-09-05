using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting;
using UnityEngine;

public class TurretHexShootBehavior : ShootBehavior
{
    private int energyDrain = 1;

    public float ShootCd // property in case of animation (range) changes upon range change
    {
        get => ShootDelay;
        set => ShootDelay = value;
    }
    
    protected override ObservableCollection<GameObject> GetTargetableObjects() => ManagerReferences.Instance.EnemyHandler.EnemyShips;

    private void Awake()
    {
        ShootCd = 1;
        enabled = false;
    }

    protected override bool DoSearch()
    {
        
        return base.DoSearch();
    }

    protected override void Shoot(Transform target, int damage, float shotAngle)
    {
        damage = 1;
        Vector3 dir = target.position - transform.position;
        shotAngle = Mathf.Atan2(dir.y, dir.x);

        if (ResourceManager.ENERGY.AttemptPurchase(energyDrain))
        {
            ResourceManager.ENERGY.AddAmountToResource(-energyDrain);
            base.Shoot(target, damage, shotAngle);
        }
        else
        {
            Debug.Log("Shot failed. Not enough energy.");
        }
    }

    protected override void RotateTowardsTarget(Transform targetTransform)
    {
        // hexes themselves do not rotate. However, perhaps an animation should in the future.
    }
}
