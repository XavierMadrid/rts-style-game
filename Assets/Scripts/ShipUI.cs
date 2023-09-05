using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class ShipUI : MonoBehaviour
{
    [SerializeField] private GameObject healthBarLeft = null;
    [SerializeField] private GameObject healthBarMiddle = null;
    [SerializeField] private GameObject healthBarRight = null;
    private GameObject[] barSegments;
    
    private Color enemyBarColor = Color.white;
    private Color shipBarColor = new Color(1, .6691f, 0, 1);
    private Color barColor;
    
    private Image healthBarLeftImage;
    private Image healthBarMiddleImage;
    private Image healthBarRightImage;

    private Targetable targetablecs;
    private Ship shipcs;
    
    private bool isEnemy;
    private bool healthBarSetup = false;
    private bool displayHealthBar = false;

    private void Awake()
    {
        targetablecs = GetComponent<Targetable>();
        shipcs = GetComponent<Ship>();
        targetablecs.OnHealthChanged += DetermineExtraHealthInfo;
    }

    private void Start()
    {
        isEnemy = TryGetComponent<EnemyShip>(out var enemyShipcs);
        barColor = isEnemy ? enemyBarColor : shipBarColor;

        displayHealthBar = ManagerReferences.Instance.UIManager.ExtendInfoToggle;
        CreateHealthBar();
    }

    private void CreateHealthBar()
    {
        barSegments = new GameObject[shipcs.MaxHealth + 3];
        InstantiateBarSegment(healthBarLeft, 0);
        
        for (int i = 1; i < shipcs.MaxHealth - 1; i++) InstantiateBarSegment(healthBarMiddle, i);
        
        InstantiateBarSegment(healthBarRight, shipcs.MaxHealth - 1);
        healthBarSetup = true;
        DetermineExtraHealthInfo(gameObject, shipcs.Health);
    }

    private void InstantiateBarSegment(GameObject barSegment, int barNumber)
    {
        GameObject barSegmentClone = Instantiate(barSegment, transform.position, Quaternion.identity);
        barSegmentClone.transform.SetParent(ManagerReferences.Instance.UIManager.Canvas.transform, false);

        barSegmentClone.GetComponent<Image>().color = barColor;
        
        HealthBarBehavior hbb = barSegmentClone.GetComponent<HealthBarBehavior>();
        hbb.BarNumber = barNumber;
        hbb.MaxSegmentNumber = shipcs.MaxHealth;
        hbb.ShipToFollow = transform;
        hbb.ShipAssigned = true;
        barSegments[barNumber] = barSegmentClone;
    }
    
    private void DetermineExtraHealthInfo(GameObject damagedShip, int health)
    {
        if (health == 0 || !healthBarSetup) return;

        if (!displayHealthBar)
        {
            for (int i = 0; i < shipcs.MaxHealth; i++)
            {
                barSegments[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < shipcs.MaxHealth; i++)
            {
                barSegments[i].SetActive(i < health);
            }
        }
    }

    private void ExtendInfoRequested(bool extendInfo)
    {
        displayHealthBar = extendInfo;
        DetermineExtraHealthInfo(gameObject, shipcs.Health);
    }

    private void OnEnable()
    {
        ManagerReferences.Instance.UIManager.OnExtendInfoRequested += ExtendInfoRequested;
    }

    private void OnDisable()
    {
        targetablecs.OnHealthChanged -= DetermineExtraHealthInfo;
        ManagerReferences.Instance.UIManager.OnExtendInfoRequested -= ExtendInfoRequested;
    }
}
