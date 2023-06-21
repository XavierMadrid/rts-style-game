using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerReferences : MonoBehaviour
{
     public static ManagerReferences Instance { get; private set; }

     public Camera MainCamera { get; private set; }
     public ShipController ShipController { get; private set; }
     public HexBuilder HexBuilder { get; private set; }
     public PlanetSpawner PlanetSpawner { get; private set; }
     public EnemyHandler EnemyHandler { get; private set; }
     public UIManager UIManager { get; private set; }

     void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        MainCamera = GetComponentInChildren<Camera>();
        ShipController = GetComponentInChildren<ShipController>();
        HexBuilder = GetComponentInChildren<HexBuilder>();
        PlanetSpawner = GetComponentInChildren<PlanetSpawner>();
        EnemyHandler = GetComponentInChildren<EnemyHandler>();
        UIManager = GetComponentInChildren<UIManager>();
    }
}
