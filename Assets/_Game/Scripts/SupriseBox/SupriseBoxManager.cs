using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SupriseBoxManager : MonoBehaviour
{
    [SerializeField] private GameObject supriseBoxPrefab;
    private GameObject currentSupriseBox;

    public void SpawnSupriseBox(Vector3 position)
    {
        currentSupriseBox = Instantiate(supriseBoxPrefab, position, Quaternion.identity);
    }

    private void ChooseSupriseRandomly(AbstractBaseTower tower)
    {
        if (tower.isBuffActive)
        {
            return;
        }

        int effectAmount = Random.Range(-8, 12);
        int effectDuration = Random.Range(4, 12);

        int randomAttribute = Random.Range(0, 3);

        switch (randomAttribute)
        {
            case 0:
                StartCoroutine(tower.ModifyDamageTemporarily(effectAmount, effectDuration));
                StartCoroutine(tower.ShowEffectText("Damage", effectAmount, effectDuration));
                break;
            case 1:
                StartCoroutine(tower.ModifyRangeTemporarily(effectAmount, effectDuration));
                StartCoroutine(tower.ShowEffectText("Range", effectAmount, effectDuration));
                break;
            case 2:
                StartCoroutine(tower.ModifyFireRateTemporarily(effectAmount, effectDuration));
                StartCoroutine(tower.ShowEffectText("Fire Rate", effectAmount, effectDuration));
                break;
        }
    }

    public void OnClickedToTower(AbstractBaseTower tower)
    {
        ChooseSupriseRandomly(tower);

        Time.timeScale = 1.0f;
    }
}