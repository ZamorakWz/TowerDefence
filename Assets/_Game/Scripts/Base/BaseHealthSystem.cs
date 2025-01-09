using UnityEngine;
using System;

public class BaseHealthSystem
{
    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; }
    public bool IsDead => CurrentHealth <= 0;

    public BaseHealthSystem(int maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
    }
}