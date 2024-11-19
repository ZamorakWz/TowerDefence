using UnityEngine;

public class TowerRangeVisualizer : MonoBehaviour, ITowerRangeUpdater
{
    [SerializeField] private GameObject rangeSpherePrefab;
    private float towerRange;
    private GameObject rangeSphere;

    private void Awake()
    {
        rangeSphere = Instantiate(rangeSpherePrefab, gameObject.transform);
        rangeSphere.transform.localPosition = Vector3.zero;
        rangeSphere.SetActive(false);
    }

    private void OnEnable()
    {
        TowerPlacementManager.OnTowerSelected += HandleTowerRange;
    }

    private void OnDisable()
    {
        TowerPlacementManager.OnTowerSelected -= HandleTowerRange;
    }

    public void HandleTowerRange(GameObject tower)
    {
        if (tower != gameObject) return;

        AbstractBaseTower baseTower = gameObject.GetComponent<AbstractBaseTower>();

        float towerRange;
        bool isTowerInitialize = baseTower.isTowerInitialize;

        if (!isTowerInitialize)
        {
            TowerTypeSO towerData = gameObject.GetComponent<AbstractBaseTower>().GetTowerData();
            towerRange = towerData.towerRange;
        }
        else
        {
            towerRange = baseTower.towerDamage;
        }

        UpdateTowerRangeVisualization(towerRange);
        ToggleRangeVisualization(true);
    }

    public void UpdateTowerRangeVisualization(float newRange)
    {
        if (rangeSphere == null) return;

        Debug.Log($"newRange is: {newRange}");

        rangeSphere.transform.localScale = new Vector3(newRange * 2f, newRange * 2f, newRange * 2f);
    }

    public void ToggleRangeVisualization(bool show)
    {
        if (rangeSphere == null) return;
        rangeSphere.SetActive(show);
    }
}