using UnityEngine;

public class TowerRangeVisualizer : MonoBehaviour, ITowerRangeUpdater
{
    //[SerializeField] private GameObject rangeSpherePrefab;
    private float towerRange;
    [SerializeField] private GameObject rangeSphere;
    [SerializeField] private GameObject rangeBox;

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
        UpdateTowerRangeVisualization(towerRange);

        if (rangeSphere != null)
        {
            rangeSphere.SetActive(show);
        }

        if (rangeBox != null)
        {
            rangeBox.SetActive(show);
        }
    }

    private float GetCurrentTowerRange()
    {
        AbstractBaseTower baseTower = gameObject.GetComponent<AbstractBaseTower>();

        if (!baseTower.isTowerInitialized)
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

        if (rangeSphere != null)
        {
            rangeSphere.transform.localScale = new Vector3(currentRange * 2f, currentRange * 2f, currentRange * 2f);
        }

        if (rangeBox != null)
        {
            rangeBox.transform.rotation = gameObject.transform.rotation;

            Vector3 localPosition = rangeBox.transform.localPosition;
            localPosition.z = currentRange / 2f;
            rangeBox.transform.localPosition = localPosition;

            Vector3 scale = rangeBox.transform.localScale;
            scale.z = currentRange;
            rangeBox.transform.localScale = scale;
        }
    }
}