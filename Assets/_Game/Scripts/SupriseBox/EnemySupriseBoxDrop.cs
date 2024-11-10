using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySupriseBoxDrop : MonoBehaviour
{
    [SerializeField] private float supriseBoxDropChance = 0.15f;
    [SerializeField] private SupriseBoxManager supriseBoxManager;

    public void TryDropSupriseBox()
    {
        if (Random.value < supriseBoxDropChance)
        {
            supriseBoxManager.SpawnSupriseBox(gameObject.transform.position);
        }
    }
}