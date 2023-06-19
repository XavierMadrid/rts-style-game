using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    protected int Damage = 1;

    public int DamageStrength
    {
        get => Damage;
        set => Damage = Mathf.Clamp(DamageStrength, 0, 999);
    }
    
    protected int MaxHealth = 5;
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
        Health = 5;
    }

    private void Update()
    {
        if (ShipController.ExtendStats)
        {
            Debug.Log($"Health: {Health}");
        }
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

    public virtual void DestroyShip() => Destroy(gameObject);
}
