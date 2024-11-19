using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedTowerManager : MonoBehaviour
{
    //Decide the list reference is it gonna be abstractbasetower
    //or gameobject or smthng else
    [SerializeField] private List <AbstractBaseTower> PlacedTowerList = new List <AbstractBaseTower>();

    public void AddTowerToList(AbstractBaseTower tower)
    {
        //if (!PlacedTowerList.Contains(tower))
        //{
        //    PlacedTowerList.Add(tower);
        //}

        PlacedTowerList.Add(tower);
    }

    public void RemoveTowerToList(AbstractBaseTower tower)
    {
        if (PlacedTowerList.Contains(tower))
        {
            PlacedTowerList.Remove(tower);
        }
    }

    public void DisableTower(AbstractBaseTower tower)
    {
        if (PlacedTowerList.Contains(tower))
        {
            tower.gameObject.SetActive(false);
        }
    }

    public void ActivateRangeVisualForAllTowers()
    {
        if (PlacedTowerList.Count == 0) return;

        for (int i = 0; i < PlacedTowerList.Count; i++)
        {
            if (PlacedTowerList[i] != null && PlacedTowerList[i].gameObject.activeSelf)
            {
                TowerRangeVisualizer rangeVisualizer = PlacedTowerList[i].GetComponent<TowerRangeVisualizer>();
                if (rangeVisualizer != null)
                {
                    rangeVisualizer.ToggleRangeVisualization(true);
                }
            }
        }
    }

    public void DeactivateRangeVisualForAllTowers()
    {
        if (PlacedTowerList.Count == 0) return;

        for (int i = 0; i < PlacedTowerList.Count; i++)
        {
            if (PlacedTowerList[i] != null && PlacedTowerList[i].gameObject.activeSelf)
            {
                TowerRangeVisualizer rangeVisualizer = PlacedTowerList[i].GetComponent<TowerRangeVisualizer>();
                if (rangeVisualizer != null)
                {
                    rangeVisualizer.ToggleRangeVisualization(false);
                }
            }
        }
    }
}