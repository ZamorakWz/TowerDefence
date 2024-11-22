using UnityEngine;

public class GroundValidator : MonoBehaviour
{
    public static GroundValidator Instance { get; private set; }
    [SerializeField] private float rayDistance = 5f;
    private LayerMask validGroundLayer;

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
        validGroundLayer = LayerMask.GetMask("TowerPlaceableGround");
    }

    //public bool CheckGroundValidity(Vector3 position)
    //{
    //    RaycastHit hit;
    //    bool hitGround = Physics.Raycast(position + Vector3.up * 0.5f, Vector3.down, out hit, rayDistance, validGroundLayer);

    //    if (!hitGround) return false;

    //    float tolerance = 0.1f;
    //    return Mathf.Abs(hit.point.y - position.y) < tolerance;
    //}

    public bool CheckGroundValidity(Vector3 position)
    {
        RaycastHit hit;
        LayerMask validGroundLayer = LayerMask.GetMask("TowerPlaceableGround");

        Vector3 rayStart = position + (Vector3.up * 0.5f);

        bool hitGround = Physics.Raycast(rayStart, Vector3.down, out hit, rayDistance, validGroundLayer);

        return hitGround;
    }
}