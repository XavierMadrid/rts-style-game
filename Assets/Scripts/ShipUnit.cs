using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipUnit : Ship
{
    public WorkshopHex WorkshopHexHome { get; set; }

    public override void DestroyShip()
    {
        if (WorkshopHexHome != null) WorkshopHexHome.ShipAlive = false;
        ManagerReferences.Instance.ShipController.ShipUnits.Remove(gameObject);
        
        base.DestroyShip();
    }
}
