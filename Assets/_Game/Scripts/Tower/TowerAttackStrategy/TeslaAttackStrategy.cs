using UnityEngine;
public class TeslaAttackStrategy : IAttackStrategy
{
    private ILightningEffectStrategy effectStrategy;
    private Vector3 towerPosition;

    public TeslaAttackStrategy(ILightningEffectStrategy effectStrategy, Vector3 towerPosition)
    {
        this.effectStrategy = effectStrategy;
        this.towerPosition = towerPosition;
    }

    public void Attack(IAttackable target, float damage)
    {
        Vector3 targetPosition = ((IPositionProvider)target).GetPosition();

        if (towerPosition == Vector3.zero)
        {
            Debug.LogError("Tower position has not been initialized correctly.");
            return;
        }

        target.TakeDamage(damage);

        //float distance = Vector3.Distance(towerPosition, targetPosition);

        effectStrategy.CreateLightningEffect(towerPosition, targetPosition, null);
    }

    public IBulletMovementStrategy GetBulletMovementStrategy()
    {
        return new TeslaBulletMoveStrategy();
    }

    public BulletObjectPool.BulletType GetBulletType()
    {
        return BulletObjectPool.BulletType.TeslaBullet;
    }
}