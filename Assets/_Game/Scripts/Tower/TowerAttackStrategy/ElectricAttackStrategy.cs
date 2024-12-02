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

        //Vector3 targetPosition = ((IPositionProvider)target).GetPosition();

        //if (towerPosition == Vector3.zero)
        //{
        //    Debug.LogError("Tower position has not been initialized correctly.");
        //    return;
        //}

        //target.TakeDamage(damage);

        //float distance = Vector3.Distance(towerPosition, targetPosition);

        //effectStrategy.CreateTeslaEffect(towerPosition, targetPosition, distance);
    }

    //private IEnumerator ChainAttack(IAttackable initialTarget, float damage)
    //{
    //    IAttackable currentTarget = initialTarget;

    //    for (int i = 0; i < chainCount; i++)
    //    {
    //        if (currentTarget == null)
    //        {
    //            yield break;
    //        }

    //        currentTarget.TakeDamage(damage);

    //        Vector3 currentTargetPosition = ((IPositionProvider)currentTarget).GetPosition();
    //        effectStrategy.CreateTeslaEffect(towerPosition, currentTargetPosition, Vector3.Distance(towerPosition, currentTargetPosition));

    //        Collider[] colliders = Physics.OverlapSphere(currentTargetPosition, chainRadius);
    //        IAttackable nextTarget = null;

    //        foreach (Collider collider in colliders)
    //        {
    //            if (collider.TryGetComponent(out IAttackable potentialTarget) && potentialTarget != currentTarget)
    //            {
    //                nextTarget = potentialTarget;
    //                break;
    //            }
    //        }

    //        currentTarget = nextTarget;
    //        towerPosition = currentTargetPosition;

    //        yield return new WaitForSeconds(0.2f);
    //    }
    //}

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