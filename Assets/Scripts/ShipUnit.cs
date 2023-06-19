using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipUnit : Ship
{
    public WorkshopHex WorkshopHexHome { get; set; }

    public override void DestroyShip()
    {
        if (WorkshopHexHome != null) WorkshopHexHome.ShipAlive = false; // default 3 ships do not have a hex home. results in error
        ManagerReferences.Instance.ShipController.ShipUnits.Remove(gameObject);
        
        Destroy(gameObject);
    }
}
