using System.Collections.Generic;
using UnityEngine;
public class TeslaTower : AbstractBaseTower
{
    [SerializeField] private ParticleSystem teslaEffectPrefab;

    protected override void InitializeAttackStrategy()
    {
        ITeslaEffectStrategy effectStrategy = new TeslaEffectStrategy(teslaEffectPrefab);
        attackStrategy = new TeslaAttackStrategy(effectStrategy, GetTowerPosition());
    }
    protected override void InitializeTargetSelectionStrategy()
    {
        targetSelectionStrategy = new AllTargets();
    }
    protected override void InitializeTargetDetectionStrategy()
    {
        targetDetector = GetComponent<SphereTargetDetector>();
        targetDetector.InitializeTargetDetector(towerRange);
    }
    public override List<ITargetSelectionStrategy> GetAvailableStrategies()
    {
        List<ITargetSelectionStrategy> strategies = new List<ITargetSelectionStrategy>();
        strategies.Add(new AllTargets());
        return strategies;
    }
}