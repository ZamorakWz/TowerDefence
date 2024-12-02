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

    [SerializeField] private Button damageUpgradeButton;
    [SerializeField] private Button rangeUpgradeButton;
    [SerializeField] private Button fireRateUpgradeButton;

    [SerializeField] private TextMeshProUGUI strategyText;
    [SerializeField] private TMP_Dropdown strategyDropdown;
    [SerializeField] private TextMeshProUGUI youCantMakeTargetSelectionText;

    private AbstractBaseTower tower;
    private float upgradeCost;
    private float damageUpgradeCost;
    private float rangeUpgradeCost;
    private float fireRateUpgradeCost;
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

            damageUpgradeCost = tower.GetUpgradeCost(tower.damageUpgradeLevel);
            rangeUpgradeCost = tower.GetUpgradeCost(tower.rangeUpgradeLevel);
            fireRateUpgradeCost = tower.GetUpgradeCost(tower.fireRateUpgradeLevel);

            damageText.text = $"Damage:{tower.towerDamage.ToString("F1")}({damageUpgradeCost} G)";
            rangeText.text = $"Range:{tower.towerRange.ToString("F1")}({rangeUpgradeCost} G)";
            fireRateText.text = $"Fire Rate:{tower.towerFireRate.ToString("F1")}({fireRateUpgradeCost} G)";

            UpdateButton(damageUpgradeButton, tower.damageUpgradeLevel);
            UpdateButton(rangeUpgradeButton, tower.rangeUpgradeLevel);
            UpdateButton(fireRateUpgradeButton, tower.fireRateUpgradeLevel);
        }
        else
        {
            HidePanel();
        }

        //UpdateStrategyDropdown();
    }

    private void UpdateButton(Button button, int upgradeLevel)
    {
        upgradeCost = tower.GetUpgradeCost(upgradeLevel);
        button.interactable = GoldManager.Instance.GetCurrentGold() >= upgradeCost;
        button.GetComponentInChildren<TextMeshProUGUI>().text = "x1.1";
    }

    //Bind to button
    public void OnDamageUpgrade()
    {
        tower.UpgradeDamage();
        UpdateTowerUI();
    }

    //Bind to button
    public void OnRangeUpgrade()
    {
        tower.UpgradeRange();
        UpdateTowerUI();
    }

    //Bind to button
    public void OnFireRateUpgrade()
    {
        tower.UpgradeFireRate();
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
        Debug.Log("UpdateStrategyDropdown called.");

        targetSelectionStrategyList = tower.availableStrategies;
        Debug.LogWarning($"{targetSelectionStrategyList.Count} is the count of strategies that come from Abstract class to towerdataui");

        strategyDropdown.ClearOptions();
        Debug.Log("Cleared dropdown options.");

        if (targetSelectionStrategyList.Count <= 1)
        {
            Debug.Log("Only one or no strategies available, hiding dropdown.");

            strategyDropdown.gameObject.SetActive(false);
            strategyText.gameObject.SetActive(false);

            if (youCantMakeTargetSelectionText != null)
            {
                youCantMakeTargetSelectionText.gameObject.SetActive(true);
            }

            return;
        }

        strategyDropdown.gameObject.SetActive(true);
        Debug.Log("Showing dropdown and adding strategies.");

        List<string> strategyNames = new List<string>();
        foreach (var strategy in targetSelectionStrategyList)
        {
            strategyNames.Add(strategy.GetType().Name);
            Debug.Log($"Added strategy to dropdown: {strategy.GetType().Name}");
        }

        strategyDropdown.AddOptions(strategyNames);

        //tower.ChangeTargetSelectionStrategy(targetSelectionStrategies[1]);

        //if (tower.targetSelectionStrategy == null)
        //{
        //    Debug.Log("Tower's targetSelectionStrategy is null. Setting default strategy.");
        //    tower.targetSelectionStrategy = strategies[0];
        //}
        //else
        //{
        //    Debug.Log($"Tower's current strategy: {tower.targetSelectionStrategy.GetType().Name}");
        //}

        int currentStrategyIndex = 0;
        for (int i = 0; i < targetSelectionStrategyList.Count; i++)
        {
            if (targetSelectionStrategyList[i].GetType().Name == tower.targetSelectionStrategy.GetType().Name)
            {
                currentStrategyIndex = i;
                Debug.Log($"Found current strategy at index {i}: {targetSelectionStrategyList[i].GetType().Name}");
                break;
            }
        }

        strategyDropdown.value = currentStrategyIndex;
        Debug.Log($"Dropdown value set to index {currentStrategyIndex}");
        strategyDropdown.RefreshShownValue();

        strategyDropdown.onValueChanged.AddListener(OnStrategyChanged);

        //tower.ChangeTargetSelectionStrategy(targetSelectionStrategyList[currentStrategyIndex]);
        //Debug.Log($"Final strategy in tower: {tower.targetSelectionStrategy?.GetType().Name}");
    }

    //Bind to dropdown
    public void OnStrategyChanged(int index)
    {
        ////List<ITargetSelectionStrategy> strategies = tower.GetAvailableStrategies();

        ////List<ITargetSelectionStrategy> strategies = tower.availableStrategies;

        //if (index >= 0 && index < targetSelectionStrategyList.Count)
        //{
        //    ITargetSelectionStrategy selectedStrategy = targetSelectionStrategyList[index];
        //    tower.targetSelectionStrategy = selectedStrategy;
        //    Debug.Log($"{tower.targetSelectionStrategy} changed to {selectedStrategy}");
        //}

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