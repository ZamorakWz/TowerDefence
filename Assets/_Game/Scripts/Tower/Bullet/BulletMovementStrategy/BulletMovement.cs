using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 10f;
    private float damage;
    private IAttackable target;
    private IAttackStrategy attackStrategy;

    public void SetBulletTarget(IAttackable target, float damage, IAttackStrategy attackStrategy)
    {
        this.target = target;
        this.damage = damage;
        this.attackStrategy = attackStrategy;

        if (target is IPositionProvider positionProvider)
        {
            Vector3 targetPosition = positionProvider.GetPosition();
            var movementStrategy = attackStrategy.GetBulletMovementStrategy();
            movementStrategy.MoveBullet(transform, targetPosition, bulletSpeed, OnHitTarget);
        }
    }

    private void OnHitTarget()
    {
        if (target is IAttackable attackable)
        {
            attackStrategy.Attack(attackable, damage);
        }

        gameObject.SetActive(false);
    }
}