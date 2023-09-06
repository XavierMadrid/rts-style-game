using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// inherited by the all hex types (i.e. energy, connector, workshop, etc)
public class HexTypeBehavior : Targetable
{
    protected bool HealthPositive;
    
    private readonly int healthPerSec = 1;
    protected readonly int MaxHealth = 10;

    private WaitForSeconds oneSec = new(1);
    
    private int health;
    public int Health
    {
        get => health;
        
        /* Targetability of a hex due to health is as follows:
            
            Hexes regenerate healthPerSec (default is 1) health per second.
            
            A hex starts as targetable. A hex stays targetable as long as its health stays > 0. 
            
            When health drops to 0, the hex becomes untargetable. It stays untargetable unless health reaches MaxHealth again.
        
        */
        private set
        {
            
            if (value <= 0)
            {
                health = 0;
                HealthPositive = false;
                ObjectTargetable(false);  
            }
            else
            {
                if (value == MaxHealth)
                {
                    health = MaxHealth;

                    HealthPositive = true;
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
