using UnityEngine;

public class GroundValidator : MonoBehaviour
{
    public static GroundValidator Instance { get; private set; }
    [SerializeField] private float rayDistance = 5f;
    private LayerMask validGroundLayer;

    bool hitGround;

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

    public bool CheckGroundValidity(Vector3 position, Vector2 towerSize)
    {
        RaycastHit hit;

        Vector3 rayStart = position + (Vector3.up * 0.1f);

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                rayStart = position + (Vector3.up * 0.1f) + new Vector3(i * towerSize.x * 2, 0, j * towerSize.y * 2);

                Debug.Log(rayStart);

                hitGround = Physics.Raycast(rayStart, Vector3.down, out hit, rayDistance, validGroundLayer);

                if (!hitGround) return false;
            }
        }
        Debug.Log(hitGround);
        return hitGround;

    }
}