using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKeyDown(KeyCode.M)) Debug.Log($"Metal: {METAL.CurrentAmount}");
            if (Input.GetKeyDown(KeyCode.G)) Debug.Log($"Gold: {GOLD.CurrentAmount}");
            if (Input.GetKeyDown(KeyCode.F)) Debug.Log($"Greenery: {GREENERY.CurrentAmount}");
            if (Input.GetKeyDown(KeyCode.N)) Debug.Log($"Antimatter: {ANTIMATTER.CurrentAmount}");
            if (Input.GetKeyDown(KeyCode.E)) Debug.Log($"Energy: {ENERGY.CurrentAmount}");
            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log($"Metal: {METAL.CurrentAmount}");
                Debug.Log($"Gold: {GOLD.CurrentAmount}");
                Debug.Log($"Greenery: {GREENERY.CurrentAmount}");
                Debug.Log($"Antimatter: {ANTIMATTER.CurrentAmount}");
                Debug.Log($"Energy: {ENERGY.CurrentAmount}");
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                METAL.AddAmountToResource(100);
                GOLD.AddAmountToResource(100);
                GREENERY.AddAmountToResource(100);
                ENERGY.AddAmountToResource(100);
                ANTIMATTER.AddAmountToResource(100);
            }
        }
    }

    public static GameResource NULL = new GameResource("Null", 0);
    public static GameResource METAL = new GameResource("Metal", 0);
    public static GameResource GOLD = new GameResource("Gold", 0);
    public static GameResource GREENERY = new GameResource("Greenery", 0);
    public static GameResource ANTIMATTER = new GameResource("Antimatter", 0);
    public static GameResource ENERGY = new GameResource("Energy", 0);
}

public class GameResource
{
    public string Name { get; private set; }

    private int currentAmount;
    public int CurrentAmount
    {
        get => currentAmount;
        private set
        {
            currentAmount = value;
            OnAmountChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler<EventArgs> OnAmountChanged;
    
    public GameResource(string resourceName, int initialAmount)
    {
        Name = resourceName;
        CurrentAmount = Mathf.Clamp(initialAmount, 0, 10000);
    }

    public void AddAmountToResource(int amountToAdd)
    {
        if (Name == "Null")
        {
            throw new NoNullAllowedException(
                "Attempted to add resource amount to the NULL resource; this is an error.");
        }
        CurrentAmount += amountToAdd;
        if (CurrentAmount < 0) CurrentAmount = 0;
    }
    
    public bool AttemptPurchase(int cost)
    {
        if (CurrentAmount >= cost)
        {
            return true;
        }
        Debug.Log($"Purchase Failed: Insufficient amount of {Name}. Deficit: {cost - CurrentAmount}");
        return false;
    }
}

public struct ResourcePrice
{
    public GameResource Resource { get; private set; }
    public int Cost { get; private set; }
    
    public ResourcePrice(GameResource resourceType, int cost)
    {
        Resource = resourceType;
        Cost = cost;
    }
    
    public bool TryPurchaseWithResourcePrice()
    {
        return Resource.AttemptPurchase(Cost);
    }
}


