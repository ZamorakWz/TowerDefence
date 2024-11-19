using UnityEngine;

public class TowerRangeVisualizer : MonoBehaviour, ITowerRangeUpdater
{
    //[SerializeField] private GameObject rangeSpherePrefab;
    private float towerRange;
    [SerializeField] private GameObject rangeSphere;

    private void Awake()
    {
        //rangeSphere = Instantiate(rangeSpherePrefab, gameObject.transform);
        //rangeSphere.transform.localPosition = Vector3.zero;
        //rangeSphere.SetActive(false);
    }

    private void OnEnable()
    {
        towerRange = GetCurrentTowerRange();

        TowerPlacementManager.OnTowerSelected += HandleTowerRange;
    }

    private void OnDisable()
    {
        TowerPlacementManager.OnTowerSelected -= HandleTowerRange;
    }

    public void HandleTowerRange(GameObject tower)
    {
        ToggleRangeVisualization(true);
    }

    public void ToggleRangeVisualization(bool show)
    {
        if (rangeSphere == null) return;

        UpdateTowerRangeVisualization(towerRange);
        rangeSphere.SetActive(show);
    }

    private float GetCurrentTowerRange()
    {
        AbstractBaseTower baseTower = gameObject.GetComponent<AbstractBaseTower>();

        if (!baseTower.isTowerInitialize)
        {
            TowerTypeSO towerData = baseTower.GetTowerData();
            return towerData.towerRange;
        }
        else
        {
            return baseTower.towerRange;
        }
    }

    public void UpdateTowerRangeVisualization(float newRange)
    {
        float currentRange = GetCurrentTowerRange();
        rangeSphere.transform.localScale = new Vector3(currentRange * 2f, currentRange * 2f, currentRange * 2f);
    }
}