using System;
using UnityEngine;

public class Ship : MonoBehaviour
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
            OnHealthChanged?.Invoke(health);
            if (health <= 0) DestroyShip();
        }
    }

    public event Func<int, bool> OnHealthChanged;
    
    // Start is called before the first frame update
    void Start()
    {
        Health = MaxHealth;
    }

    /// <summary>
    /// Damage the target by an integer.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>Remaining health of the target. If 0, the target is dead.</returns>
    public int DamageShip(int value)
    {
        Health -= value;
        return Health;
    }

    public virtual void DestroyShip()
    {
        if (OnHealthChanged != null)
        {
            foreach (var subscriber in OnHealthChanged.GetInvocationList())
            {
                OnHealthChanged -= subscriber as Func<int, bool>;
            }
        }
        
        Destroy(gameObject);
    }

}
