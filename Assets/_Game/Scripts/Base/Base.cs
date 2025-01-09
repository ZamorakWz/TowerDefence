using UnityEngine;
using UnityEngine.Events;

public class Base : MonoBehaviour, IDamageable
{
    public static event UnityAction<int, int> OnBaseHealthChanged;

    [SerializeField] private LevelDataSO currentLevelData;
    private BaseHealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = new BaseHealthSystem(currentLevelData.levelBaseHealth);
        OnBaseHealthChanged?.Invoke(healthSystem.MaxHealth, healthSystem.CurrentHealth);
    }

    public void TakeDamage(int damage)
    {
        healthSystem.TakeDamage(damage);
        OnBaseHealthChanged?.Invoke(healthSystem.MaxHealth, healthSystem.CurrentHealth);

        if (healthSystem.IsDead)
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