using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILightningEffectStrategy
{
    void CreateLightningEffect(Vector3 startPosition, Vector3 endPosition, float distance);
}