using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : AbstractBaseTower
{
    protected override void InitializeAttackStrategy()
    {
        attackStrategy = new LaserAttackStrategy();
    }

    protected override void InitializeTargetDetectionStrategy()
    {
        targetDetector = GetComponent<BoxTargetDetector>();
        targetDetector.InitializeTargetDetector(towerRange);
    }

    protected override void InitializeTargetSelectionStrategy()
    {
        targetSelectionStrategy = new FurthestTarget();
    }
    public override List<ITargetSelectionStrategy> GetAvailableStrategies()
    {
        List<ITargetSelectionStrategy> strategies = availableStrategies;
        availableStrategies.Clear();
        strategies.Add(new FurthestTarget());
        return strategies;
    }
}