using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealthController : MonoBehaviour, IAttackable, IHealthProvider
{
    public Action<float> OnHealthChanged;
    public static Action<EnemyHealthController> OnEnemyDied;

    private EnemySpawnController _enemySpawnController;

    [SerializeField] private EnemyTypeSO _enemyTypeSO;

    [SerializeField] private float _currentHealth;
    private bool _isDead;

    public float CurrentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = value; }
    }

    private void Awake()
    {
        _enemySpawnController = FindObjectOfType<EnemySpawnController>();
    }

    private void OnEnable()
    {
        _isDead = false;
    }

    public void SetEnemyHealthBeginningValue()
    {
        _currentHealth = _enemyTypeSO.typeMaxHealth;
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;

        if (_currentHealth <= 0)
        {
            Die();
        }

        Debug.Log($"{amount} damage is taken!");

        OnHealthChanged?.Invoke(_currentHealth);
    }

    public void Die()
    {
        if (_isDead) { return; }

        _isDead = true;

        OnEnemyDied?.Invoke(this);

        var supriseBoxDrop = GetComponent<EnemySupriseBoxDrop>();
        if (supriseBoxDrop != null)
        {
            supriseBoxDrop.TryDropSupriseBox();
        }

        gameObject.SetActive(false);

        _enemySpawnController.AliveEnemyCount--;

        GoldManager.Instance.AddGold(_enemyTypeSO.typeGold);
    }

    public float GetHealth()
    {
        return CurrentHealth;
    }
}