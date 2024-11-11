using UnityEngine;

public class GroundValidator : MonoBehaviour
{
    public static GroundValidator Instance { get; private set; }
    [SerializeField] private float _rayDistance = 5f;

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
    }

    public bool CheckGroundValidity(Vector3 position)
    {
        RaycastHit hit;
        LayerMask validGroundLayer = LayerMask.GetMask("TowerPlaceableGround");

        Vector3 rayStart = position + (Vector3.up * 0.5f);

        bool hitGround = Physics.Raycast(rayStart, Vector3.down, out hit, _rayDistance, validGroundLayer);

        return hitGround;
    }
}