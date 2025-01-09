using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaypointPath : MonoBehaviour
{
    public Transform[] enemyWaypoints;

    private void OnDrawGizmos()
    {
        if (enemyWaypoints != null && enemyWaypoints.Length > 1)
        {
            for (int i = 0; i < enemyWaypoints.Length - 1 ; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(enemyWaypoints[i].position, enemyWaypoints[i + 1].position);
            }
        }
    }
}