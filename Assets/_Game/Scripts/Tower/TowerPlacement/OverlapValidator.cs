using UnityEngine;

public class OverlapValidator : MonoBehaviour
{
    public static OverlapValidator Instance { get; private set; }

    [SerializeField] private float _checkRadius = 1f;

    private static int targetDetectionLayer;
    private static int treeLayer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        targetDetectionLayer = LayerMask.NameToLayer("TargetDetection");
        treeLayer = LayerMask.NameToLayer("Tree");
    }

    public bool CheckOverlapping(Vector3 position, GameObject selectedTower)
    {
        Collider[] colliders = Physics.OverlapSphere(position, _checkRadius);

        TowerOutline outline = selectedTower.GetComponent<TowerOutline>();

        foreach (var collider in colliders)
        {
            if (collider is SphereCollider && collider.gameObject.layer == targetDetectionLayer)
            {
                var baseTower = collider.GetComponentInParent<AbstractBaseTower>();

                if (baseTower != null && baseTower.gameObject != selectedTower)
                {
                    outline.OutlineColor = Color.red;
                    outline.OutlineWidth = 10f;
                    return false;
                }
            }
            else if (collider is CapsuleCollider && collider.gameObject.layer == treeLayer)
            {
                outline.OutlineColor = Color.red;
                outline.OutlineWidth = 10f;
                return false;
            }
        }
        outline.OutlineWidth = 0f;
        return true;
    }
}