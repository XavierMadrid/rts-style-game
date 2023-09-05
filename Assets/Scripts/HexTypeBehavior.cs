using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTypeBehavior : Targetable
{
    public bool Disabled;

    protected bool AtMaxHealth;
    
    private readonly int healthPerSec = 1;
    protected readonly int MaxHealth = 10;
    private int health;

    private WaitForSeconds oneSec = new(1);
    
    public int Health
    {
        get => health;
        private set
        {
            if (value <= 0)
            {
                health = 0;
                AtMaxHealth = false;
                ObjectTargetable(false); // "dead" as in disabled    
            }
            else
            {
                if (value == MaxHealth)
                {
                    health = MaxHealth;

                    AtMaxHealth = true;
                    ObjectTargetable(true);                
                }
                else if (value > MaxHealth)
                {
                    health = MaxHealth;
                }
                else
                {
                    health = value;
                }
            }
            
            HealthChanged(health);
            Debug.Log($"{gameObject} Health: {health}");
        }
    }

    public override void DamageObject(int damage)
    {
        Health -= damage;
    }
    
    protected IEnumerator PassiveHealthRegen()
    {
        Health = 0;
        while (gameObject != null)
        {
            Health += healthPerSec;

            yield return oneSec;
        }
    } 
}
