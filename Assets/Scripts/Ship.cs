using System;
using UnityEngine;

public class Ship : Targetable
{
    protected int Damage = 1;

    public int DamageStrength
    {
        get => Damage;
        set => Damage = Mathf.Clamp(DamageStrength, 0, 999);
    }
    
    protected int maxHealth = 4;
    public int MaxHealth => maxHealth;

    private int health;
    public int Health
    {
        get => health;
        set
        {
            health = Mathf.Clamp(value, 0, MaxHealth);
            
            HealthChanged(health);
            // OnHealthChanged?.Invoke(health); it is handled in the HealthChanged() statement above.
            
            if (health <= 0) DestroyShip();
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Health = MaxHealth;
    }
    
    public override void DamageObject(int damage)
    {
        Health -= damage;
    }

    public virtual void DestroyShip()
    {
        // if (OnHealthChanged != null)
        // {
        //     foreach (var subscriber in OnHealthChanged.GetInvocationList())
        //     {
        //         OnHealthChanged -= subscriber as Func<int, bool>;
        //     }
        // }
        ObjectTargetable(false);
        
        Destroy(gameObject);
    }

}
