using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TowerPlacementManager : MonoBehaviour
{
    public static Action<GameObject> OnTowerSelected;
    public static Action<GameObject> OnTowerPlaced;

    private Camera mainCamera;
    private LayerMask placementLayer;
    private GameObject selectedTowerPrefab;
    private bool isCanPlaceHere;

    //Grid Test
    [SerializeField] GridManager gridManager;
    Vector3 gridPosition;

    [Inject] private PlacedTowerManager placedTowerManager;

    private void Awake()
    {
        placementLayer = LayerMask.GetMask("TowerPlaceableGround");
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (selectedTowerPrefab != null)
        {
            UpdatePrefabPlacement();

            if (Input.GetMouseButtonDown(0))
            {
                PlaceTower();
            }
        }
    }

    public void SelectTower(GameObject tower)
    {
        if (selectedTowerPrefab != null)
        {
            Destroy(selectedTowerPrefab);
        }
        selectedTowerPrefab = tower;
        Debug.Log("Selected tower prefab: " + selectedTowerPrefab.gameObject.name);

        OnTowerSelected?.Invoke(tower);
        placedTowerManager.ActivateRangeVisualForAllTowers();
    }

    private void UpdatePrefabPlacement()
    {
        if (TryGetPlacementPosition(out Vector3? hitPoint, out RaycastHit hitInfo))
        {
            selectedTowerPrefab.SetActive(true);

            //Grid Test
            gridPosition = gridManager.GetGridPosition(hitPoint.Value);
            selectedTowerPrefab.transform.position = gridPosition;
            //Temp Solution to align towers to grids
            Vector2 selectedTowerSize = selectedTowerPrefab.GetComponent<AbstractBaseTower>().GetTowerData().towerSize;
            selectedTowerPrefab.transform.position += new Vector3(selectedTowerSize.x, 0, selectedTowerSize.y);

            //selectedTowerPrefab.transform.position = hitPoint.Value;
            isCanPlaceHere = CanPlaceHere(gridPosition, selectedTowerSize);
        }
        else
        {
            selectedTowerPrefab.SetActive(false);
        }
    }

    private void PlaceTower()
    {
        // Check Tower size
        Vector2 selectedTowerSize = selectedTowerPrefab.GetComponent<AbstractBaseTower>().GetTowerData().towerSize;
        if (!gridManager.CheckTowerPlacements(gridPosition, selectedTowerSize))
        {
            return;
        }



        //Tower cost is needed to be checked before the player place the tower.
        //Tower is destroyed when player has not enough gold
        float selectedTowerCost = selectedTowerPrefab.GetComponent<AbstractBaseTower>().GetTowerData().towerCost;
        if (selectedTowerCost > GoldManager.Instance.currentGold)
        {
            Debug.Log("insufficient amount of gold");
            Destroy(selectedTowerPrefab);
            selectedTowerPrefab = null;
            return;
        }

        GoldManager.Instance.RemoveGold((int)selectedTowerCost);
        //

        if (isCanPlaceHere && TryGetPlacementPosition(out Vector3? hitPoint, out RaycastHit hitInfo))
        {
            if (selectedTowerPrefab != null)
            {
                //selectedTowerPrefab.transform.position = hitPoint.Value;
                selectedTowerPrefab.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);

                OnTowerPlaced?.Invoke(selectedTowerPrefab);

                //GridMechanicTesting
                //selectedTowerSize = selectedTowerPrefab.GetComponent<AbstractBaseTower>().GetTowerData().towerSize;
                gridManager.gridData.AddTowerAt(gridPosition, selectedTowerSize);
                


                placedTowerManager.AddTowerToList(selectedTowerPrefab.GetComponent<AbstractBaseTower>());
                placedTowerManager.DeactivateRangeVisualForAllTowers();


                selectedTowerPrefab = null;
            }
        }
    }

    private bool TryGetPlacementPosition(out Vector3? hitPoint, out RaycastHit hitInfo)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, placementLayer))
        {
            hitPoint = hitInfo.point;
            return true;
        }
        hitPoint = null;
        hitInfo = default;
        return false;
    }

    public bool CanPlaceHere(Vector3 position, Vector2 towerSize)
    {
        return GroundValidator.Instance.CheckGroundValidity(position, towerSize);
        //&& OverlapValidator.Instance.CheckOverlapping(position, selectedTowerPrefab);
    }
}