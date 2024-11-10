using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySupriseBoxDrop : MonoBehaviour
{
    [SerializeField] private float supriseBoxDropChance = 0.15f;
    private SupriseBoxManager supriseBoxManager;

    private void Start()
    {
        supriseBoxManager = FindAnyObjectByType<SupriseBoxManager>();
    }

    public void TryDropSupriseBox()
    {
        if (Random.value < supriseBoxDropChance)
        {
            supriseBoxManager.SpawnSupriseBox(gameObject.transform.position);
        }
    }
}