using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigAOEAttackStrategy : IAttackStrategy
{
    private float aoeRadius;
    private IAOEEffectStrategy effectStrategy;

    public BigAOEAttackStrategy(float radius, IAOEEffectStrategy effectStrategy)
    {
        aoeRadius = radius;
        this.effectStrategy = effectStrategy;
    }

    public void Attack(IAttackable target, float damage)
    {
        Vector3 targetPosition = ((IPositionProvider)target).GetPosition();

        Collider[] hitColliders = Physics.OverlapSphere(targetPosition, aoeRadius);

        foreach (var hitCollider in hitColliders)
        {
            IAttackable attackable = hitCollider.GetComponent<IAttackable>();
            if (attackable != null)
            {
                attackable.TakeDamage(damage);
            }
        }

        effectStrategy.CreateAOEEffect(targetPosition, aoeRadius);
    }

    public IBulletMovementStrategy GetBulletMovementStrategy()
    {
        return new ArcBulletMoveStrategy();
    }

    public BulletObjectPool.BulletType GetBulletType()
    {
        return BulletObjectPool.BulletType.MortarBullet;
    }
}