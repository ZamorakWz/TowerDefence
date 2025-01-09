using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastestTarget : ITargetSelectionStrategy
{
    public IEnumerable<IAttackable> SelectTargets(List<IAttackable> targets, Vector3 towerPosition)
    {
        IAttackable fastestTarget = null;
        float maxSpeed = float.MinValue;

        foreach (var target in targets)
        {
            if (target is EnemyComposite enemy)
            {
                float speed = enemy.GetSpeedValue();

                if (speed > maxSpeed)
                {
                    maxSpeed = speed;
                    fastestTarget = target;
                }
            }

            //float speed = ((ISpeedProvider)target).GetSpeedValue();

            //if (speed > maxSpeed)
            //{
            //    maxSpeed = speed;
            //    fastestTarget = target;
            //}
        }

        if (fastestTarget != null)
        {
            yield return fastestTarget;
        }
    }
}