using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealthController : MonoBehaviour, IAttackable, IHealthProvider
{
    public Action<float> OnHealthChanged;
    public static Action<EnemyHealthController> OnEnemyDied;

    private EnemySpawnController enemySpawnController;

    [SerializeField] private EnemyTypeSO enemyTypeSO;

    [SerializeField] private float currentHealth;
    private bool isDead;

    public float CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    private void Awake()
    {
        enemySpawnController = FindObjectOfType<EnemySpawnController>();
    }

    private void OnEnable()
    {
        isDead = false;
    }

    public void SetEnemyHealthBeginningValue()
    {
        currentHealth = enemyTypeSO.typeMaxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }

        Debug.Log($"{amount} damage is taken to {gameObject.name}");

        OnHealthChanged?.Invoke(currentHealth);
    }

    public void Die()
    {
        if (isDead) { return; }

        isDead = true;

        OnEnemyDied?.Invoke(this);

        var supriseBoxDrop = GetComponent<EnemySupriseBoxDrop>();
        if (supriseBoxDrop != null)
        {
            supriseBoxDrop.TryDropSupriseBox();
        }

        gameObject.SetActive(false);

        enemySpawnController.AliveEnemyCount--;

        GoldManager.Instance.AddGold(enemyTypeSO.typeGold);
    }

    public float GetHealth()
    {
        return CurrentHealth;
    }
}