using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarTower : AbstractBaseTower
{
    //Big AOE Tower

    [SerializeField] private ParticleSystem explosionEffectPrefab;

    protected override void InitializeAttackStrategy()
    {
        IAOEEffectStrategy effectStrategy = new AOEEffectStrategy(explosionEffectPrefab);
        attackStrategy = new BigAOEAttackStrategy(towerData.towerAOERadius, effectStrategy);
    }

    protected override void InitializeTargetSelectionStrategy()
    {
        targetSelectionStrategy = new NearestTarget();
    }
}