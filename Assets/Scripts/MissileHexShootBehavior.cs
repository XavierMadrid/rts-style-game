using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class MissileHexShootBehavior : ShootBehavior
{
    protected override ObservableCollection<GameObject> GetTargetableObjects() => ManagerReferences.Instance.EnemyHandler.EnemyShips;

    private void Awake()
    {
        ShootDelay = 3.5f;
        Damage = 5;
        Range = 150f;
        enabled = false;
    }

    protected override void RotateTowardsTarget(Transform target)
    {
        
    }
}
