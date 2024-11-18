using UnityEngine;

public class EnemyComposite : MonoBehaviour, IAttackable, IPositionProvider, IHealthProvider
{
    private EnemyHealthController healthController;
    private EnemyMovement movement;

    void Awake()
    {
        healthController = GetComponent<EnemyHealthController>();
        movement = GetComponent<EnemyMovement>();
    }

    public void TakeDamage(float amount)
    {
        healthController?.TakeDamage(amount);
    }

    public Vector3 GetPosition()
    {
        return movement?.GetPosition() ?? Vector3.zero;
    }

    public float GetHealth()
    {
        return healthController?.GetHealth() ?? 0f;
    }
}