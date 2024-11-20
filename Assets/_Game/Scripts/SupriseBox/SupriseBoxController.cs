using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SupriseBoxController : MonoBehaviour
{
    private SupriseBoxManager supriseBoxManager;
    private PlacedTowerManager placedTowerManager;

    private void Awake()
    {
        supriseBoxManager = FindAnyObjectByType<SupriseBoxManager>();
        placedTowerManager = FindAnyObjectByType<PlacedTowerManager>();
    }

    public void OnClickedToSupriseBox()
    {
        Time.timeScale = 0.5f;

        placedTowerManager.ActivateIndicatorAllTowers();

        Destroy(gameObject);
    }
}