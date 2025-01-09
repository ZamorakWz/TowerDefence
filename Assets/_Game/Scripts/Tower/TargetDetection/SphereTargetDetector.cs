using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class SphereTargetDetector : MonoBehaviour, ITargetDetector
{
    private SphereCollider detectionCollider;
    [SerializeField] private HashSet<IAttackable> targetsInRange = new HashSet<IAttackable>();

    private void OnEnable()
    {
        EnemyHealthController.OnEnemyDied += HandleEnemyDied;
    }

    private void OnDisable()
    {
        EnemyHealthController.OnEnemyDied -= HandleEnemyDied;
    }

    public void InitializeTargetDetector(float detectionRadius)
    {
        detectionCollider = gameObject.GetComponentInChildren<SphereCollider>();
        detectionCollider.radius = detectionRadius;
        detectionCollider.isTrigger = true;
    }

    public void UpdateRange(float newRange)
    {
        detectionCollider.radius = newRange;
    }

    public List<IAttackable> GetTargetsInRange()
    {
        return new List<IAttackable>(targetsInRange);
    }

    public void AddTarget(IAttackable target)
    {
        targetsInRange.Add(target);
    }

    public void RemoveTarget(IAttackable target)
    {
        targetsInRange.Remove(target);
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyComposite target = other.GetComponent<EnemyComposite>();
        if (target != null)
        {
            AddTarget(target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnemyComposite target = other.GetComponent<EnemyComposite>();
        if (target != null)
        {
            RemoveTarget(target);
        }
    }

    private void HandleEnemyDied(EnemyHealthController enemy)
    {
        EnemyComposite target = enemy.GetComponent<EnemyComposite>();
        if (target != null)
        {
            RemoveTarget(target);
        }
    }
}