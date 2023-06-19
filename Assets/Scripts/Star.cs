using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Star : MonoBehaviour
{
    protected int MaxYield = 240;
    protected int harvestAmount = 1;
    protected float harvestDelayTime = 2f;
    private bool stopHarvestTrigger = false;
    private bool currentlyHarvesting = false;
    private bool starDepleted;
    
    private GameObject harvestingObject;
    private GameResource resourceToHarvest = ResourceManager.ENERGY;
    private WaitForSeconds harvestDelay;

    public event EventHandler<EventArgs> OnStarDepleted;
    
    private int currentYield = 0;

    public int CurrentYield
    {
        get => currentYield;
        private set { currentYield = value; }
    }
    
    void Start()
    {
        harvestDelay = new WaitForSeconds(harvestDelayTime);
    }
    public bool HarvestStar(GameObject harvestingObject)
    {
        if (currentlyHarvesting || starDepleted) return false; // prevent multiple star collectors from harvesting the same star at the same time, or if star empty

        this.harvestingObject = harvestingObject;
        StartCoroutine(HarvestStarOverTime());
        return true;
    }

    private IEnumerator HarvestStarOverTime()
    {
        currentlyHarvesting = true;
        
        while (!stopHarvestTrigger)
        {
            resourceToHarvest.AddAmountToResource(harvestAmount);
            CurrentYield += harvestAmount;

            if (CurrentYield > MaxYield - harvestAmount)
            {
                starDepleted = true;
                stopHarvestTrigger = true;
                
                OnStarDepleted?.Invoke(this, EventArgs.Empty);
            }
            
            yield return harvestDelay;
        }

        currentlyHarvesting = false;
        stopHarvestTrigger = false;
    }

    public void StopHarvestStar() => stopHarvestTrigger = true;
}
