using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTower : AbstractBaseTower
{
    [SerializeField] private ParticleSystem electricParticlePrefab;

    protected override void InitializeAttackStrategy()
    {
        ILightningEffectStrategy effectStrategy = new ElectricEffectStrategy(electricParticlePrefab);
        attackStrategy = new ElectricAttackStrategy(effectStrategy, GetTowerPosition());
    }

    protected override void InitializeTargetSelectionStrategy()
    {
        targetSelectionStrategy = new NearestTarget();
    }

    protected override void InitializeTargetDetectionStrategy()
    {
        targetDetector = GetComponent<BoxTargetDetector>();
        targetDetector.InitializeTargetDetector(towerRange);
    }
}