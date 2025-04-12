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

    [Inject] private PlacedTowerManager placedTowerManager;

    //Grid Test
    [SerializeField] GridManager gridManager;
    Vector3 gridPosition;
    [SerializeField] GameObject gridPlacementObjectPrefab;
    GameObject gridPlacementObject;


    // Tower Rotation
    private Vector2 selectedTowerPrefabSize;

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

            if (Input.GetKeyDown(KeyCode.R))
            {
                RotateTower();
            }

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
        selectedTowerPrefabSize = GetTowerSize(tower);
        Debug.Log("Selected tower prefab: " + selectedTowerPrefab.gameObject.name);

        // Grid Placement
        // @TODO: grid placement object will be in prefab and every tower prefab is gonna have tower grid visualizer
        gridPlacementObject = Instantiate(gridPlacementObjectPrefab);
        Vector3 gridObjectScale = gridPlacementObject.transform.localScale;
        gridObjectScale = new Vector3(gridObjectScale.x * selectedTowerPrefabSize.x, 0,gridObjectScale.z * selectedTowerPrefabSize.y);
        gridPlacementObject.transform.localScale = gridObjectScale;
        gridPlacementObject.transform.parent = selectedTowerPrefab.transform;

        selectedTowerPrefab.GetComponent<TowerGridVisualizer>().GridObject = gridPlacementObject;

        placedTowerManager.GridVisualizationForAllTowers(true);

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
            //Vector2 selectedTowerSize = selectedTowerPrefab.GetComponent<AbstractBaseTower>().GetTowerData().towerSize;
            selectedTowerPrefab.transform.position += new Vector3(selectedTowerPrefabSize.x, 0, selectedTowerPrefabSize.y);

            //selectedTowerPrefab.transform.position = hitPoint.Value;
            isCanPlaceHere = CanPlaceHere(gridPosition, selectedTowerPrefabSize);
        }
        else
        {
            selectedTowerPrefab.SetActive(false);
        }
    }

    private void PlaceTower()
    {
        // Check Tower size
        //Vector2 selectedTowerSize = selectedTowerPrefab.GetComponent<AbstractBaseTower>().GetTowerData().towerSize;
        if (!gridManager.CheckTowerPlacements(gridPosition, selectedTowerPrefabSize))
        {
            return;
        }

        if (isCanPlaceHere && TryGetPlacementPosition(out Vector3? hitPoint, out RaycastHit hitInfo))
        {
            if (selectedTowerPrefab != null)
            {
                //selectedTowerPrefab.transform.position = hitPoint.Value;
                //selectedTowerPrefab.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);

                OnTowerPlaced?.Invoke(selectedTowerPrefab);

                //GridMechanicTesting
                //selectedTowerSize = selectedTowerPrefab.GetComponent<AbstractBaseTower>().GetTowerData().towerSize;
                gridManager.gridData.AddTowerAt(gridPosition, selectedTowerPrefabSize);
                
                placedTowerManager.AddTowerToList(selectedTowerPrefab.GetComponent<AbstractBaseTower>());
                placedTowerManager.DeactivateRangeVisualForAllTowers();
                
                //Grid Visualize
                placedTowerManager.GridVisualizationForAllTowers(false);

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

    public Vector2 GetTowerSize(GameObject selectedTower)
    {
        selectedTowerPrefabSize = selectedTower.GetComponent<AbstractBaseTower>().GetTowerData().towerSize;

        return selectedTowerPrefabSize;
    }
    
    public Vector2 RotateTower()
    {
        bool canRotate = selectedTowerPrefab.GetComponent<AbstractBaseTower>().GetTowerData().isLookAtTower;

        if(selectedTowerPrefabSize.x != selectedTowerPrefabSize.y)
        {
            selectedTowerPrefabSize = new Vector2(selectedTowerPrefabSize.y, selectedTowerPrefabSize.x);
        }
        
        if(!canRotate)
        {
            selectedTowerPrefab.transform.eulerAngles += new Vector3(0, 90, 0);
        }
        
        return selectedTowerPrefabSize;
    }

}

