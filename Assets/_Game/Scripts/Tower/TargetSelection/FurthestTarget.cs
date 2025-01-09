using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurthestTarget : ITargetSelectionStrategy
{
    public IEnumerable<IAttackable> SelectTargets(List<IAttackable> targets, Vector3 towerPosition)
    {
        IAttackable furthestTarget = null;
        float furthestDistance = float.MinValue;

        foreach (var target in targets)
        {
            float distance = Vector3.Distance(towerPosition, ((IPositionProvider)target).GetPosition());

            if (distance > furthestDistance)
            {
                furthestDistance = distance;
                furthestTarget = target;
            }
        }

        if (furthestTarget != null)
        {
            yield return furthestTarget;
        }
    }
}