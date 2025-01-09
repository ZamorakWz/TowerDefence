using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTargetDetector : MonoBehaviour, ITargetDetector
{
    private BoxCollider detectionCollider;
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
        BoxCollider[] colliders = gameObject.GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("TargetDetection"))
            {
                detectionCollider = collider;
                break;
            }
        }

        detectionCollider.isTrigger = true;

        Vector3 size = detectionCollider.size;
        size.z = detectionRadius;
        detectionCollider.size = size;

        Vector3 center = detectionCollider.center;
        center.z = detectionRadius / 2;
        detectionCollider.center = center;
    }

    public void UpdateRange(float newRange)
    {
        Vector3 size = detectionCollider.size;
        size.z = newRange;
        detectionCollider.size = size;

        Vector3 center = detectionCollider.center;
        center.z = newRange / 2;
        detectionCollider.center = center;
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
