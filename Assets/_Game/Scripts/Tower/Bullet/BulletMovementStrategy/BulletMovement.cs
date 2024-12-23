using System.Collections;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 10f;
    private float damage;
    private IAttackable target;
    private IAttackStrategy attackStrategy;

    [Header("If Laser Bullet:")]
    [SerializeField] private bool isLaserBullet;
    [SerializeField] float maxDistance = 20f;

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

            if (isLaserBullet)
            {
                StartCoroutine(MoveLaserBullet(transform, targetPosition, bulletSpeed));
            }
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

    public IEnumerator MoveLaserBullet(Transform bulletTransform, Vector3 targetPosition, float bulletSpeed)
    {
        Vector3 startPosition = bulletTransform.position;
        float startYPosition = startPosition.y;

        Vector3 direction = (targetPosition - startPosition).normalized;
        direction.y = 0f;

        float travelDistance = 0f;

        transform.LookAt(targetPosition);

        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.x = 90f;
        transform.rotation = Quaternion.Euler(rotation);


        while (travelDistance < maxDistance)
        {
            Vector3 movement = direction * bulletSpeed * Time.deltaTime;
            Vector3 newPosition = bulletTransform.position += movement;

            newPosition.y = startYPosition;

            travelDistance += movement.magnitude;

            yield return null;
        }

        bulletTransform.gameObject.SetActive(false);
    }

    //Laser tower some of attack logic
    private void OnTriggerEnter(Collider other)
    {
        if (isLaserBullet && other.TryGetComponent(out IAttackable attackable))
        {
            attackStrategy.Attack(attackable, damage);
        }
    }
}