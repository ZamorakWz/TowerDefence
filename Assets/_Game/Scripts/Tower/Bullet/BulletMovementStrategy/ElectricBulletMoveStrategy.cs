using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBulletMoveStrategy : IBulletMovementStrategy
{
    public void MoveBullet(Transform bulletTransform, Vector3 targetPosition, float bulletSpeed, Action onComplete)
    {
        float distance = Vector3.Distance(bulletTransform.position, targetPosition);
        float duration = distance / bulletSpeed;

        bulletTransform.DOMove(targetPosition, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => onComplete?.Invoke());
    }
}