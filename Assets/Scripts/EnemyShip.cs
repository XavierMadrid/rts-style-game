using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : Ship
{
    public override void DestroyShip()
    {
        ManagerReferences.Instance.EnemyHandler.EnemyShips.Remove(gameObject);
        
        base.DestroyShip();
    }
}
