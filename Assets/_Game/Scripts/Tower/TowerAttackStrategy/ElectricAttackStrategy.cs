using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricAttackStrategy : IAttackStrategy
{
    private ILightningEffectStrategy effectStrategy;
    private Vector3 towerPosition;

    private int chainCount = 4;
    private float chainRadius = 6f;

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
        if (initialTarget == null)
        {
            yield break;
        }

        Vector3 previousTargetPosition = towerPosition;

        for (int i = 0; i < chainCount; i++)
        {
            if (initialTarget == null)
            {
                yield break;
            }

            //initialTarget.TakeDamage(damage);

            Vector3 currentTargetGetPosition = initialTarget.GetPosition();
            Vector3 currentTargetHigherYPosition = new Vector3(currentTargetGetPosition.x, currentTargetGetPosition.y + 8, currentTargetGetPosition.z);

            effectStrategy.CreateLightningEffect(currentTargetHigherYPosition, currentTargetGetPosition, initialTarget.transform);

            initialTarget.TakeDamage(damage);

            Collider[] colliders = Physics.OverlapSphere(currentTargetGetPosition, chainRadius);
            Debug.Log($"Found {colliders.Length} colliders within chain radius.");
            EnemyComposite nextTarget = null;

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out EnemyComposite potentialTarget) && potentialTarget != initialTarget)
                {
                    Debug.Log($"Potential target found: {potentialTarget.name}");
                    if (currentTargetGetPosition.magnitude > potentialTarget.GetPosition().magnitude)
                    {
                        nextTarget = potentialTarget;
                        break;
                    }
                }
            }

            initialTarget = nextTarget;
            previousTargetPosition = currentTargetHigherYPosition;

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