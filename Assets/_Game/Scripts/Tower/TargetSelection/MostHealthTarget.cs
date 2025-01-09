using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MostHealthTarget : ITargetSelectionStrategy
{
    public IEnumerable<IAttackable> SelectTargets(List<IAttackable> targets, Vector3 towerPosition)
    {
        IAttackable mostHealthTarget = null;
        float maxHealth = float.MinValue;

        foreach (var target in targets)
        {
            if (target is EnemyComposite enemy)
            {
                float health = enemy.GetHealth();

                if (health > maxHealth)
                {
                    maxHealth = health;
                    mostHealthTarget = target;
                }
            }

            //float health = ((IHealthProvider)target).GetHealth();
            //if (health > maxHealth)
            //{
            //    maxHealth = health;
            //    mostHealthTarget = target;
            //}
        }

        if (mostHealthTarget != null)
        {
            yield return mostHealthTarget;
        }
    }
}