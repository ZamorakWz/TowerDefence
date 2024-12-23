using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSphereVisualizer : MonoBehaviour
{
    public Vector3 currentTargetPosition;
    public float sphereRadius = 12f;
    public Color gizmoColor = Color.red;

    public void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(currentTargetPosition, sphereRadius);
    }
}
