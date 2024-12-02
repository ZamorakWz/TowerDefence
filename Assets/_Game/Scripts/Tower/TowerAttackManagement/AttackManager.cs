using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AttackManager : IAttackManager
{
    private IAttackStrategy attackStrategy;
    private float fireRate;
    private float damage;
    private float lastAttackTime;
    private Transform firePoint;
    private GameObject tower;
    private AbstractBaseTower towerComponent;
    private BulletObjectPool bulletObjectPool;

    public AttackManager(IAttackStrategy attackStrategy, float fireRate, float damage, Transform firePoint, GameObject tower, BulletObjectPool bulletObjectPool)
    {
        this.attackStrategy = attackStrategy;
        this.fireRate = fireRate;
        this.damage = damage;
        this.firePoint = firePoint;
        this.tower = tower;
        towerComponent = this.tower.GetComponent<AbstractBaseTower>();
        this.bulletObjectPool = bulletObjectPool;
    }

    public void UpdateDamage(float newDamage)
    {
        damage = newDamage;
    }
    
    public void UpdateFireRate(float newFireRate)
    {
        fireRate = newFireRate;
    }

    public void Attack(IEnumerable<IAttackable> targets)
    {
        if (CanAttack())
        {
            foreach (var target in targets)
            {
                FireBullet(target);
                if (CheckIsLookAtTower() && target is IPositionProvider positionProvider)
                {
                    tower.transform.LookAt(positionProvider.GetPosition(), Vector3.up);
                }
            }
            lastAttackTime = Time.time;
        }
    }

    public bool CheckIsLookAtTower()
    {
        return towerComponent.GetTowerData().isLookAtTower ? true : false;
    }

    private void FireBullet(IAttackable target)
    {
        GameObject bullet = bulletObjectPool.GetPooledBullet(attackStrategy.GetBulletType());

        if (bullet == null)
        {
            Debug.LogError($"Bullet could not be retrieved for type: {attackStrategy.GetBulletType()}");
            return;
        }

        bullet.transform.position = firePoint.position;

        BulletMovement bulletMovement = bullet.GetComponent<BulletMovement>();
        bulletMovement.SetBulletTarget(target, damage, attackStrategy);
    }

    public bool CanAttack()
    {
        return Time.time - lastAttackTime >= 1f / fireRate;
    }
}