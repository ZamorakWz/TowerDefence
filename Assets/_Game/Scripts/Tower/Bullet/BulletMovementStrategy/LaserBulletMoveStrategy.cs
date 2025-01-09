using System;
using UnityEngine;

public class LaserBulletMoveStrategy : IBulletMovementStrategy
{
    public void MoveBullet(Transform bulletTransform, Vector3 targetPosition, float bulletSpeed, Action onComplete)
    {
        //This movement is written to BulletMovement.cs
    }
}