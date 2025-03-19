using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TowerButtonsUIManager : MonoBehaviour
{
    [SerializeField] private TowerPlacementManager towerPlacementManager;
    [SerializeField] protected List<AbstractBaseTower> availableTowers;
    public GameObject _rewardBox;
    public GameObject[] towerList;
    private bool isUsed;

    private void OnEnable()
    {
        TowerCreationManager.OnGetTowerList += HandleGetTowerList;
    }

    private void OnDisable()
    {
        TowerCreationManager.OnGetTowerList -= HandleGetTowerList;
    }

    public void CreateTowerButtons()
    {
        if (_rewardBox.transform.childCount > 0)
        {
            GameObject destroyPiece = _rewardBox.transform.GetChild(0).gameObject;
            Destroy(destroyPiece);
        }
        GameObject towerType = Instantiate(towerList[Random.Range(0, towerList.Length)]);
        towerType.transform.SetParent(_rewardBox.transform, false);
        towerType.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        UnityEngine.UI.Button button = towerType.GetComponent<UnityEngine.UI.Button>();

        foreach (AbstractBaseTower tower in availableTowers)
        {
            if (button.name.Contains(tower.name)) 
            {
                button.onClick.AddListener(() => OnTowerButtonClicked(tower.GetType().Name));
            }
        }
    }

    public void OnTowerButtonClicked(string towerType)
    {
        GameObject createdTower = TowerCreationManager.Instance.CreateTower(towerType, Vector3.zero);

        if (createdTower != null)
        {
            towerPlacementManager.SelectTower(createdTower);
        }
        GameObject destroyPiece = _rewardBox.transform.GetChild(0).gameObject;
        Destroy(destroyPiece);
    }

    private void HandleGetTowerList(List<AbstractBaseTower> towers)
    {
        foreach (var tower in towers)
        {
            availableTowers.Add(tower);
        }

        CreateTowerButtons();
    }
}