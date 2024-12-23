using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricEffectStrategy : ILightningEffectStrategy
{
    private ParticleSystem electricParticlePrefab;
    public ParticleSystem effect;

    public ElectricEffectStrategy(ParticleSystem electricParticlePrefab)
    {
        this.electricParticlePrefab = electricParticlePrefab;
    }

    public void CreateLightningEffect(Vector3 startPosition, Vector3 endPosition, Transform obj)
    {
        effect = Object.Instantiate(electricParticlePrefab, startPosition, Quaternion.identity);
        float distance = Vector3.Distance(endPosition, startPosition);

        if (endPosition != startPosition)
        {
            //Vector3 direction = (endPosition - startPosition).normalized;
            //effect.transform.rotation = Quaternion.LookRotation(direction);

            //Vector3 rotation = effect.transform.rotation.eulerAngles;
            //rotation.x = 90f;
            //effect.transform.rotation = Quaternion.Euler(rotation);

            effect.transform.rotation = Quaternion.identity;
        }
        //else
        //{
        //    effect.transform.rotation = Quaternion.identity;
        //}

        var main = effect.main;

        main.startLifetime = Mathf.Clamp(distance / 10f, 0.5f, 5f);

        effect.transform.SetParent(obj);

        effect.Play();

        //Object.Destroy(effect.gameObject, distance);
    }
}