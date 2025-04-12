using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacedTowerManager : MonoBehaviour
{
    [SerializeField] private List <AbstractBaseTower> placedTowerList = new List <AbstractBaseTower>();
    private List<Coroutine> activeCoroutines = new List<Coroutine>();

    public void AddTowerToList(AbstractBaseTower tower)
    {
        placedTowerList.Add(tower);
    }

    public void RemoveTowerToList(AbstractBaseTower tower)
    {
        if (placedTowerList.Contains(tower))
        {
            placedTowerList.Remove(tower);
        }
    }

    public void DisableTower(AbstractBaseTower tower)
    {
        if (placedTowerList.Contains(tower))
        {
            tower.gameObject.SetActive(false);
        }
    }

    public void GridVisualizationForAllTowers(bool isVisible)
    {
        if (placedTowerList.Count == 0) return;

        for (int i = 0; i < placedTowerList.Count; i++)
        {
            if (placedTowerList[i] != null && placedTowerList[i].gameObject.activeSelf)
            {
                TowerGridVisualizer gridVisualizer = placedTowerList[i].GetComponent<TowerGridVisualizer>();

                if (gridVisualizer != null)
                {
                    gridVisualizer.SetGridVisual(isVisible);
                }
            }
        }
    }

    public void ActivateRangeVisualForAllTowers()
    {
        if (placedTowerList.Count == 0) return;

        for (int i = 0; i < placedTowerList.Count; i++)
        {
            if (placedTowerList[i] != null && placedTowerList[i].gameObject.activeSelf)
            {
                TowerRangeVisualizer rangeVisualizer = placedTowerList[i].GetComponent<TowerRangeVisualizer>();
                if (rangeVisualizer != null)
                {
                    rangeVisualizer.ToggleRangeVisualization(true);
                }
            }
        }
    }

    public void DeactivateRangeVisualForAllTowers()
    {
        if (placedTowerList.Count == 0) return;

        for (int i = 0; i < placedTowerList.Count; i++)
        {
            if (placedTowerList[i] != null && placedTowerList[i].gameObject.activeSelf)
            {
                TowerRangeVisualizer rangeVisualizer = placedTowerList[i].GetComponent<TowerRangeVisualizer>();
                if (rangeVisualizer != null)
                {
                    rangeVisualizer.ToggleRangeVisualization(false);
                }
            }
        }
    }

    public void ActivateIndicatorAllTowers()
    {
        if (placedTowerList.Count == 0) return;

        for (int i = 0; i < placedTowerList.Count; i++)
        {
            TowerOutline towerOutline = placedTowerList[i].gameObject.GetComponent<TowerOutline>();

            towerOutline.OutlineColor = Color.yellow;

            Coroutine newCoroutine = StartCoroutine(ChangeIndicatorOutlineWidth(towerOutline));
            activeCoroutines.Add(newCoroutine);
        }
    }

    public void DeactivateIndicatorAllTowers()
    {
        if (placedTowerList.Count == 0) return;

        foreach (var coroutine in activeCoroutines)
        {
            StopCoroutine(coroutine);
        }

        for (int i = 0; i < placedTowerList.Count; i++)
        {
            TowerOutline towerOutline = placedTowerList[i].gameObject.GetComponent<TowerOutline>();
            towerOutline.OutlineWidth = 0;
        }

        activeCoroutines.Clear();
    }

    IEnumerator ChangeIndicatorOutlineWidth(TowerOutline towerOutline)
    {
        int amount = 0;
        bool isIncreasing = true;

        int maxOutlineWidth = 10;
        int minOutlineWidth = 5;

        while (true)
        {
            towerOutline.OutlineWidth = amount;

            yield return new WaitForSeconds(0.1f);

            if (isIncreasing)
            {
                amount++;
                if (amount >= maxOutlineWidth)
                {
                    isIncreasing = false;
                }
            }
            else
            {
                amount--;
                if (amount <= minOutlineWidth)
                {
                    isIncreasing = true;
                }
            }
        }
    }
}