using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricAttackStrategy : IAttackStrategy
{
    private ILightningEffectStrategy effectStrategy;
    private Vector3 towerPosition;

    private int chainCount = 4;
    private float chainRadius = 2f;

    public ElectricAttackStrategy(ILightningEffectStrategy effectStrategy, Vector3 towerPosition)
    {
        this.effectStrategy = effectStrategy;
        this.towerPosition = towerPosition;
    }

    public void Attack(IAttackable target, float damage)
    {
        if (target is EnemyComposite enemyComposite)
        {
            enemyComposite.StartCoroutine(ChainAttack(enemyComposite, damage));
        }
    }

    private IEnumerator ChainAttack(EnemyComposite initialTarget, float damage)
    {
        EnemyComposite currentTarget = initialTarget;

        for (int i = 0; i < chainCount; i++)
        {
            if (currentTarget == null)
            {
                yield break;
            }

            currentTarget.TakeDamage(damage);

            Vector3 currentTargetPosition = currentTarget.GetPosition();
            effectStrategy.CreateLightningEffect(towerPosition, currentTargetPosition, Vector3.Distance(towerPosition, currentTargetPosition));

            Collider[] colliders = Physics.OverlapSphere(currentTargetPosition, chainRadius);
            EnemyComposite nextTarget = null;

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out EnemyComposite potentialTarget) && potentialTarget != currentTarget)
                {
                    nextTarget = potentialTarget;
                    break;
                }
            }

            currentTarget = nextTarget;
            towerPosition = currentTargetPosition;

            yield return new WaitForSeconds(0.2f);
        }
    }


    public IBulletMovementStrategy GetBulletMovementStrategy()
    {
        return new TeslaBulletMoveStrategy();
    }

    public BulletObjectPool.BulletType GetBulletType()
    {
        return BulletObjectPool.BulletType.ElectricBullet;
    }
}