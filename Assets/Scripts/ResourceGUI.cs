using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceGUI : MonoBehaviour
{

    [SerializeField] private GameObject metalAmount;
    [SerializeField] private GameObject greeneryAmount;
    [SerializeField] private GameObject energyAmount;
    [SerializeField] private GameObject goldAmount;
    [SerializeField] private GameObject antimatterAmount;
    [SerializeField] private GameObject shipCount;
    
    private TextMeshProUGUI metalAmountTMP;
    private TextMeshProUGUI greeneryAmountTMP;
    private TextMeshProUGUI energyAmountTMP;
    private TextMeshProUGUI goldAmountTMP;
    private TextMeshProUGUI antimatterAmountTMP;
    private TextMeshProUGUI shipCountTMP;
    
    // Start is called before the first frame update
    void Start()
    {
        ManagerReferences.Instance.ShipController.ShipUnits.CollectionChanged += ShipUnitsCountChanged;

        metalAmountTMP = metalAmount.GetComponent<TextMeshProUGUI>();
        greeneryAmountTMP = greeneryAmount.GetComponent<TextMeshProUGUI>();
        energyAmountTMP = energyAmount.GetComponent<TextMeshProUGUI>();
        goldAmountTMP = goldAmount.GetComponent<TextMeshProUGUI>();
        antimatterAmountTMP = antimatterAmount.GetComponent<TextMeshProUGUI>();
        shipCountTMP = shipCount.GetComponent<TextMeshProUGUI>();
    }

    private void AmountChangedMetal(object sender, EventArgs e)
    {
        metalAmountTMP.text = ResourceManager.METAL.CurrentAmount.ToString();
    }
    
    private void AmountChangedGreenery(object sender, EventArgs e)
    {
        greeneryAmountTMP.text = ResourceManager.GREENERY.CurrentAmount.ToString();
    }
    
    private void AmountChangedEnergy(object sender, EventArgs e)
    {
        energyAmountTMP.text = ResourceManager.ENERGY.CurrentAmount.ToString();
    }
    
    private void AmountChangedGold(object sender, EventArgs e)
    {
        goldAmountTMP.text = ResourceManager.GOLD.CurrentAmount.ToString();
    }
    
    private void AmountChangedAntimatter(object sender, EventArgs e)
    {
        antimatterAmountTMP.text = ResourceManager.ANTIMATTER.CurrentAmount.ToString();
    }
    
    private void ShipUnitsCountChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        shipCountTMP.text = ManagerReferences.Instance.ShipController.ShipUnits.Count.ToString();
    }

    void OnEnable()
    {
        ResourceManager.METAL.OnAmountChanged += AmountChangedMetal;
        ResourceManager.GREENERY.OnAmountChanged += AmountChangedGreenery;
        ResourceManager.ENERGY.OnAmountChanged += AmountChangedEnergy;
        ResourceManager.GOLD.OnAmountChanged += AmountChangedGold;
        ResourceManager.ANTIMATTER.OnAmountChanged += AmountChangedAntimatter;
    }

    private void OnDisable()
    {
        ResourceManager.METAL.OnAmountChanged -= AmountChangedMetal;
        ResourceManager.GREENERY.OnAmountChanged -= AmountChangedGreenery;
        ResourceManager.ENERGY.OnAmountChanged -= AmountChangedEnergy;
        ResourceManager.GOLD.OnAmountChanged -= AmountChangedGold;
        ResourceManager.ANTIMATTER.OnAmountChanged -= AmountChangedAntimatter;

        ManagerReferences.Instance.ShipController.ShipUnits.CollectionChanged -= ShipUnitsCountChanged;
    }
}
