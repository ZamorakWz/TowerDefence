using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SupriseBoxManager : MonoBehaviour
{
    [SerializeField] private GameObject supriseBoxPrefab;
    [SerializeField] private Canvas indicatorPanelUI;

    private float effectAmount;
    private float effectDuration;

    private void Awake()
    {
        effectAmount = Random.Range(-8f, 12f);
        effectDuration = Random.Range(4f, 12f);
    }

    public void SpawnSupriseBox(Vector3 position)
    {
        Instantiate(supriseBoxPrefab, position, Quaternion.identity);
    }

    private void ChooseSupriseRandomly(AbstractBaseTower tower)
    {
        int randomAttribute = Random.Range(0, 3);

        switch (randomAttribute)
        {
            case 0:
                tower.ModifyDamage(effectAmount, effectDuration);
                break;
            case 1:
                tower.ModifyRange(effectAmount, effectDuration);
                break;
            case 2:
                tower.ModifyFireRate(effectAmount, effectDuration);
                break;
        }
    }

    public void OnClickedToTower(AbstractBaseTower tower)
    {
        Debug.Log("OnClickedToTower is called from SupriseBoxManager.");
        if (tower == null)
        {
            Debug.LogError("Tower passed to OnClickedToTower is null!");
            return;
        }

        ChooseSupriseRandomly(tower);

        indicatorPanelUI.gameObject.SetActive(false);

        Time.timeScale = 1.0f;

        Destroy(supriseBoxPrefab);
    }

    public void OnClickedToSupriseBox()
    {
        Time.timeScale = 0.25f;

        if (indicatorPanelUI != null)
        {
            indicatorPanelUI.gameObject.SetActive(true);
        }

        Debug.Log($"Clicked to SupriseBox!");
    }
}