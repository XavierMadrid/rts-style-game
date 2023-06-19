using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    protected int maxYield = 100;
    protected int harvestAmount = 1;
    protected float harvestDelayTime = 1;

    protected GameResource resourceToHarvest = ResourceManager.NULL;
    
    private int currentYield;
    private bool planetDepleted;

    public int CurrentYield
    {
        get => currentYield;
        private set { currentYield = value; }
    }

    private bool stopHarvestTrigger;

    private WaitForSeconds harvestDelay;

    public event EventHandler<EventArgs> OnPlanetDepleted;

    void Start()
    {
        harvestDelay = new WaitForSeconds(harvestDelayTime);
    }

    public bool TryHarvest()
    {
        if (!planetDepleted)
        {
            StartCoroutine(HarvestOverTime());
            return true;
        }

        return false;
    }

    private IEnumerator HarvestOverTime()
    {
        while (!stopHarvestTrigger)
        {
            resourceToHarvest.AddAmountToResource(harvestAmount);
            CurrentYield += harvestAmount;

            if (CurrentYield > maxYield - harvestAmount)
            {
                planetDepleted = true;
                
                OnPlanetDepleted?.Invoke(this, EventArgs.Empty);
                
                stopHarvestTrigger = true;
            }
            
            yield return harvestDelay;
        }
        
        stopHarvestTrigger = false;
    }

    public void StopHarvest(float t = 0) => stopHarvestTrigger = true;
}
