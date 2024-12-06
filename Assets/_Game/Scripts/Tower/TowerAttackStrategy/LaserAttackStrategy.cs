using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttackStrategy : IAttackStrategy
{
    public void Attack(IAttackable target, float damage)
    {
        target.TakeDamage(damage);
    }

    public IBulletMovementStrategy GetBulletMovementStrategy()
    {
        return new LaserBulletMoveStrategy();
    }

    public BulletObjectPool.BulletType GetBulletType()
    {
        return BulletObjectPool.BulletType.LaserBullet;
    }
}