using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricEffectStrategy : ILightningEffectStrategy
{
    private ParticleSystem electricParticlePrefab;

    public ElectricEffectStrategy(ParticleSystem teslaParticlePrefab)
    {
        this.electricParticlePrefab = teslaParticlePrefab;
    }

    public void CreateLightningEffect(Vector3 startPosition, Vector3 endPosition, float distance)
    {
        ParticleSystem effect = Object.Instantiate(electricParticlePrefab, startPosition, Quaternion.identity);

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
