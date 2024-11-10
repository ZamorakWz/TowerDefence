using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Zenject;

public class InputHandler : MonoBehaviour
{
    [Inject] private TowerDataPanelManager towerDataPanelManager;

    [SerializeField] private bool isClickedToSupriseBox = false;
    [SerializeField] private SupriseBoxManager supriseBoxManager;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse button down detected");

            HandleInput(Input.mousePosition);
        }
        else if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("Touch began detected");
                HandleInput(touch.position);
            }
        }
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
                Debug.Log($"Raycast hit: {hitObject.name} with tag {hitObject.tag}");
                ProcessHitObject(hitObject);
            }
            else
            {
                towerDataPanelManager.CloseAllPanels();
            }
        }
    }

    private void ProcessHitObject(GameObject hitObject)
    {
        switch (hitObject.tag)
        {
            case "Tower":
                TowerDataUI towerDataUI = hitObject.GetComponent<TowerDataUI>();
                if (towerDataUI != null)
                {
                    towerDataPanelManager.ShowTowerPanel(towerDataUI);
                }

                if (isClickedToSupriseBox)
                {
                    AbstractBaseTower tower = hitObject.GetComponent<AbstractBaseTower>();
                    Debug.Log("SupriseBoxManager.Instance exists, calling OnClickedToTower.");
                    supriseBoxManager.OnClickedToTower(tower);
                    isClickedToSupriseBox = false;
                }
                break;
            case "SupriseBox":
                supriseBoxManager.OnClickedToSupriseBox();
                isClickedToSupriseBox = true;
                break;
            default:
                towerDataPanelManager.CloseAllPanels();
                break;
        }
    }
}

//using UnityEngine;
//using UnityEngine.EventSystems;
//using System.Collections.Generic;
//using Zenject;

//public class InputHandler : MonoBehaviour
//{
//    [Inject] private TowerDataPanelManager towerDataPanelManager;
//    [SerializeField] private bool isClickedToSupriseBox = false;

//    void Update()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            HandleInput(Input.mousePosition);
//        }
//        else if (Input.touchCount > 0)
//        {
//            Touch touch = Input.GetTouch(0);
//            if (touch.phase == TouchPhase.Began)
//            {
//                Debug.Log("Touch began detected");
//                HandleInput(touch.position);
//            }
//        }
//    }

//    private bool IsPointerOverUIElement()
//    {
//        if (Input.touchCount > 0)
//        {
//            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
//        }
//        return EventSystem.current.IsPointerOverGameObject();
//    }

//    private void HandleInput(Vector3 screenPosition)
//    {
//        if (!IsPointerOverUIElement())
//        {
//            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
//            int layerMask = LayerMask.GetMask("TowerInteraction", "SupriseBoxInteraction");
//            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, layerMask))
//            {
//                GameObject hitObject = hitInfo.collider.gameObject;
//                Debug.Log($"Raycast hit: {hitObject.name} with tag {hitObject.tag}");
//                ProcessHitObject(hitObject);
//            }
//            else
//            {
//                ResetSupriseBoxState();
//            }
//        }
//        else
//        {
//            ResetSupriseBoxState();
//        }
//    }

//    private void ProcessHitObject(GameObject hitObject)
//    {
//        switch (hitObject.tag)
//        {
//            case "Tower":
//                Debug.Log("Tower hit");
//                HandleTowerClick(hitObject);
//                break;
//            case "SupriseBox":
//                Debug.Log("SupriseBox hit");
//                HandleSupriseBoxClick();
//                break;
//            default:
//                Debug.Log($"Hit object with unhandled tag: {hitObject.tag}");
//                ResetSupriseBoxState();
//                //towerDataPanelManager.CloseAllPanels();
//                break;
//        }
//    }

//    private void HandleTowerClick(GameObject towerObject)
//    {
//        TowerDataUI towerDataUI = towerObject.GetComponent<TowerDataUI>();
//        if (towerDataUI != null)
//        {
//            towerDataPanelManager.ShowTowerPanel(towerDataUI);
//        }

//        if (isClickedToSupriseBox)
//        {
//            AbstractBaseTower tower = towerObject.GetComponent<AbstractBaseTower>();

//            if (tower != null)
//            {
//                if (SupriseBoxManager.Instance != null)
//                {
//                    SupriseBoxManager.Instance.OnClickedToTower(tower);
//                }
//            }

//            ResetSupriseBoxState();
//        }
//    }

//    private void HandleSupriseBoxClick()
//    {
//        if (SupriseBoxManager.Instance != null)
//        {
//            SupriseBoxManager.Instance.OnClickedToSupriseBox();

//        }

//        isClickedToSupriseBox = true;
//    }

//    private void ResetSupriseBoxState()
//    {
//        towerDataPanelManager.CloseAllPanels();

//        if (isClickedToSupriseBox)
//        {
//            isClickedToSupriseBox = false;
//        }
//    }
//}