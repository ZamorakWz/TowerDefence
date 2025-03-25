using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Zenject;

public class InputHandler : MonoBehaviour
{
    //GetMousePosition Variables
    [SerializeField] Camera cam;
    [SerializeField] LayerMask placementLayerMask;
    Vector3 returnPosition;

    private bool isClickedToSupriseBox = false;
    [SerializeField] private SupriseBoxManager supriseBoxManager;

    [Inject] private TowerDataPanelManager towerDataPanelManager;
    [Inject] private PlacedTowerManager placedTowerManager;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleInput(Input.mousePosition);
        }
        else if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                HandleInput(touch.position);
            }
        }
    }

    //GetMousePosition in InputHandler
    public Vector3 GetMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = cam.nearClipPlane;
        Ray rayToScreen = cam.ScreenPointToRay(mousePosition);
        RaycastHit rayCastHit;
        if (Physics.Raycast(rayToScreen, out rayCastHit, Mathf.Infinity, placementLayerMask))
        {
            returnPosition = rayCastHit.point;
        }
        Debug.Log(returnPosition);
        return returnPosition;
    }


    private bool IsPointerOverUIElement()
    {
        if (Input.touchCount > 0)
        {
            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        }

        return EventSystem.current.IsPointerOverGameObject();
    }

    private void HandleInput(Vector3 screenPosition)
    {
        if (!IsPointerOverUIElement())
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            int layerMask = LayerMask.GetMask("TowerInteraction", "SupriseBoxInteraction");

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, layerMask))
            {
                GameObject hitObject = hitInfo.collider.gameObject;
                ProcessHitObject(hitObject);
            }
            else
            {
                towerDataPanelManager.CloseAllPanels();
                placedTowerManager.DeactivateRangeVisualForAllTowers();
            }
        }
    }

    private void ProcessHitObject(GameObject hitObject)
    {
        switch (hitObject.tag)
        {
            case "Tower":
                TowerRangeVisualizer towerRangeVisualizer = hitObject.gameObject.GetComponent<TowerRangeVisualizer>();
                if (towerRangeVisualizer != null)
                {
                    towerRangeVisualizer.ToggleRangeVisualization(true);
                }

                TowerDataUI towerDataUI = hitObject.GetComponent<TowerDataUI>();
                if (towerDataUI != null)
                {
                    towerDataPanelManager.ShowTowerPanel(towerDataUI);
                }

                if (isClickedToSupriseBox)
                {
                    AbstractBaseTower tower = hitObject.GetComponent<AbstractBaseTower>();
                    if (tower != null)
                    {
                        supriseBoxManager.OnClickedToTower(tower);
                        isClickedToSupriseBox = false;
                    }
                }
                break;
            case "SupriseBox":
                SupriseBoxController supriseBoxController = hitObject.GetComponent<SupriseBoxController>();
                supriseBoxController.OnClickedToSupriseBox();
                isClickedToSupriseBox = true;
                break;
            default:
                break;
        }
    }
}