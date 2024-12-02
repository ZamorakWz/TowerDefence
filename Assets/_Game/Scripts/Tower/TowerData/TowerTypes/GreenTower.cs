using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenTower : AbstractBaseTower
{
    //Direct Attacker Tower

    protected override void InitializeAttackStrategy()
    {
        attackStrategy = new DirectAttackStrategy();
    }

    protected override void InitializeTargetSelectionStrategy()
    {
        if (targetSelectionStrategy != null)
        {
            Debug.Log($"Existing strategy will be used: {targetSelectionStrategy.GetType().Name}");
            return;
        }

        targetSelectionStrategy = new NearestTarget();
        Debug.Log("Setting default strategy to NearestTarget");
    }

    protected override void InitializeTargetDetectionStrategy()
    {
        targetDetector = GetComponent<SphereTargetDetector>();
        targetDetector.InitializeTargetDetector(towerRange);
    }
}