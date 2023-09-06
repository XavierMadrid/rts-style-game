using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Targetable : MonoBehaviour
{
    public bool Disabled;
    
    public abstract void DamageObject(int damage);

    public event Action<GameObject, int> OnHealthChanged;
    public event Action<GameObject, bool> OnObjectTargetable;

    protected virtual void HealthChanged(int newHealth)
    {
        OnHealthChanged?.Invoke(gameObject, newHealth);
    }

    protected virtual void ObjectTargetable(bool isTargetable)
    {
        Disabled = !isTargetable;
        OnObjectTargetable?.Invoke(gameObject, isTargetable);
        if (Disabled == isTargetable)
        {
            
        }
    }
}
