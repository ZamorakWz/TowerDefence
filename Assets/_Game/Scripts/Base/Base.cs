using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour, IDamageable
{
    public static Action<int, int> OnBaseHealthChanged;

    [SerializeField] private int maxHealth = 100;
    private IBaseHealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = new HealthSystem(maxHealth);

        OnBaseHealthChanged?.Invoke(maxHealth, healthSystem.currentHealth);
    }

    public void TakeDamage(int damage)
    {
        healthSystem.TakeDamage(damage);

        OnBaseHealthChanged?.Invoke(maxHealth, healthSystem.currentHealth);

        if (healthSystem.isDead)
        {
            OnBaseDeath();
        }
    }

    private void OnBaseDeath()
    {
        Debug.Log("Base is destroyed! Game Over!");

        Time.timeScale = 0;
    }
}