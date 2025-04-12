using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Zenject;
using System.Collections.Generic;

public class TowerDataUI : MonoBehaviour
{
    [SerializeField] private Canvas towerCanvas;
    [SerializeField] private TextMeshProUGUI topicText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI rangeText;
    [SerializeField] private TextMeshProUGUI fireRateText;
    [SerializeField] private TextMeshProUGUI towerUpgradeCostText;

    [SerializeField] private Button upgradeButton;

    [SerializeField] private TextMeshProUGUI strategyText;
    [SerializeField] private TMP_Dropdown strategyDropdown;
    [SerializeField] private TextMeshProUGUI youCantMakeTargetSelectionText;

    private AbstractBaseTower tower;
    private float upgradeCost;
    private float towerUpgradeCost;
    private List<ITargetSelectionStrategy> targetSelectionStrategyList;

    [Inject] private TowerDataPanelManager towerDataPanelManager;

    private void Awake()
    {
        tower = GetComponent<AbstractBaseTower>();
        HidePanel();
    }

    private void OnEnable()
    {
        TowerPlacementManager.OnTowerPlaced += HandleUpdateUI;
    }

    private void OnDisable()
    {
        TowerPlacementManager.OnTowerPlaced -= HandleUpdateUI;
    }

    public void ShowPanel()
    {
        UpdateTowerUI();
        if (towerCanvas != null)
        {
            towerCanvas.gameObject.SetActive(true);
        }
    }

    public void HidePanel()
    {
        if (towerCanvas != null)
        {
            towerCanvas.gameObject.SetActive(false);
        }
    }

    private void UpdateTowerUI()
    {
        if (tower.isTowerPlaced)
        {
            topicText.text = $"{tower.GetTowerData().towerName}";

            towerUpgradeCost = tower.GetUpgradeCost(tower.upgradeLevel);

            damageText.text = $"Damage:{tower.towerDamage.ToString("F1")}";
            rangeText.text = $"Range:{tower.towerRange.ToString("F1")}";
            fireRateText.text = $"Fire Rate:{tower.towerFireRate.ToString("F1")}";
            towerUpgradeCostText.text = $"Upgrade Cost:{towerUpgradeCost.ToString("F1")}";

            UpdateButton(upgradeButton, tower.upgradeLevel);
        }
        else
        {
            HidePanel();
        }
    }

    private void UpdateButton(Button button, int upgradeLevel)
    {
        upgradeCost = tower.GetUpgradeCost(upgradeLevel);
    }

    //Bind to button
    public void OnTowerUpgrade()
    {
        tower.UpgradeTower();
        UpdateTowerUI();
    }

    private void HandleUpdateUI(GameObject gameObject)
    {
        if (gameObject == this.gameObject)
        {
            UpdateTowerUI();
        }
    }

    public void UpdateStrategyDropdown()
    {
        targetSelectionStrategyList = tower.availableStrategies;

        strategyDropdown.ClearOptions();

        if (targetSelectionStrategyList.Count <= 1)
        {
            strategyDropdown.gameObject.SetActive(false);
            strategyText.gameObject.SetActive(false);

            if (youCantMakeTargetSelectionText != null)
            {
                youCantMakeTargetSelectionText.gameObject.SetActive(true);
            }

            return;
        }

        strategyDropdown.gameObject.SetActive(true);

        List<string> strategyNames = new List<string>();
        foreach (var strategy in targetSelectionStrategyList)
        {
            strategyNames.Add(strategy.GetType().Name);
        }

        strategyDropdown.AddOptions(strategyNames);

        int currentStrategyIndex = 0;
        for (int i = 0; i < targetSelectionStrategyList.Count; i++)
        {
            if (targetSelectionStrategyList[i].GetType().Name == tower.targetSelectionStrategy.GetType().Name)
            {
                currentStrategyIndex = i;
                break;
            }
        }

        strategyDropdown.value = currentStrategyIndex;
        strategyDropdown.RefreshShownValue();

        strategyDropdown.onValueChanged.AddListener(OnStrategyChanged);
    }

    //Bind to dropdown
    public void OnStrategyChanged(int index)
    {
        ITargetSelectionStrategy selectedStrategy = null;

        switch (index)
        {
            case 0:
                selectedStrategy = targetSelectionStrategyList[0];
                break;
            case 1:
                selectedStrategy = targetSelectionStrategyList[1];
                break;
            case 2:
                selectedStrategy = targetSelectionStrategyList[2];
                break;
            case 3:
                selectedStrategy = targetSelectionStrategyList[3];
                break;
            default:
                break;
        }

        tower.ChangeTargetSelectionStrategy(selectedStrategy);
    }

    //Bind to button
    public void OnRotateLeft()
    {
        RotateTower(-45f);
    }

    //Bind to button
    public void OnRotateRight()
    {
        RotateTower(45f);
    }

    private void RotateTower(float angle)
    {
        Quaternion currentRotation = tower.gameObject.transform.rotation;

        Vector3 localUp = tower.gameObject.transform.up;

        Quaternion rotationChange = Quaternion.AngleAxis(angle, localUp);

        tower.gameObject.transform.rotation = rotationChange * currentRotation;
    }
}