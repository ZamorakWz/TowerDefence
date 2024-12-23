using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerButtonsUIManager : MonoBehaviour
{
    [SerializeField] private Transform towerButtonContainer;
    [SerializeField] private Button towerButtonPrefab;
    [SerializeField] private TowerPlacementManager towerPlacementManager;
    [SerializeField] private List<AbstractBaseTower> availableTowers;

    private void OnEnable()
    {
        TowerCreationManager.OnGetTowerList += HandleGetTowerList;
    }

    private void OnDisable()
    {
        TowerCreationManager.OnGetTowerList -= HandleGetTowerList;
    }

    private void CreateTowerButtons()
    {
        foreach (AbstractBaseTower tower in availableTowers)
        {
            Button button = Instantiate(towerButtonPrefab, towerButtonContainer);
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

            if (buttonText != null)
            {
                buttonText.text = tower.gameObject.name;
            }

            button.onClick.AddListener(() => OnTowerButtonClicked(tower.GetType().Name));
        }
    }

    private void OnTowerButtonClicked(string towerType)
    {
        GameObject createdTower = TowerCreationManager.Instance.CreateTower(towerType, Vector3.zero);
        if (createdTower != null)
        {
            towerPlacementManager.SelectTower(createdTower);
        }
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