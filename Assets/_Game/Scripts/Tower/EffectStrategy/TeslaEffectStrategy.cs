using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaEffectStrategy : ILightningEffectStrategy
{
    private ParticleSystem teslaParticlePrefab;

    public TeslaEffectStrategy(ParticleSystem teslaParticlePrefab)
    {
        this.teslaParticlePrefab = teslaParticlePrefab;
    }

    public void CreateLightningEffect(Vector3 startPosition, Vector3 endPosition, Transform obj)
    {
        ParticleSystem effect = Object.Instantiate(teslaParticlePrefab, startPosition, Quaternion.identity);
        float distance = Vector3.Distance(endPosition, startPosition);

        if (endPosition != startPosition)
        {
            Vector3 direction = (endPosition - startPosition).normalized;
            effect.transform.rotation = Quaternion.LookRotation(direction);

            distance = Vector3.Distance(endPosition, startPosition);
            float length = distance;

            var shape = effect.shape;
            shape.length = distance;
        }

        var main = effect.main;

        float speedMultiplier = 5f;
        float maxSpeed = 10f;
        float calculatedSpeed = Mathf.Clamp(distance * speedMultiplier, 10f, maxSpeed);

        main.startSpeed = calculatedSpeed;
        main.startLifetime = distance / calculatedSpeed * 1f;

        effect.Play();

        Object.Destroy(effect.gameObject, main.duration);
    }
}