using UnityEngine;

public class EnemyComposite : MonoBehaviour, IAttackable, IPositionProvider, IHealthProvider, ISpeedProvider
{
    private EnemyHealthController enemyHealth;
    private EnemyMovement enemyMovement;

    void Awake()
    {
        enemyHealth = GetComponent<EnemyHealthController>();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    public void TakeDamage(float amount)
    {
        enemyHealth?.TakeDamage(amount);
    }

    public Vector3 GetPosition()
    {
        return enemyMovement?.GetPosition() ?? Vector3.zero;
    }

    public float GetHealth()
    {
        return enemyHealth?.GetHealth() ?? 0f;
    }

    public float GetSpeedValue()
    {
        return enemyMovement.GetSpeedValue();
    }
}