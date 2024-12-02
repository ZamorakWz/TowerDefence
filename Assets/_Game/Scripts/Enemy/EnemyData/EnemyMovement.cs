using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour, IPositionProvider, ISpeedProvider
{
    private EnemyWaypointPath waypointPath;

    private Transform targetWaypoint;
    [SerializeField] private int currentWaypointIndex = 0;
    public int CurrentWaypointIndex
    {  
        get { return currentWaypointIndex; }
        set { currentWaypointIndex = Mathf.Max(0, value); }
    }

    private Vector3 currentDirection;

    private float speed;
    [SerializeField] private EnemyTypeSO enemyType;

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    void Start()
    {
        waypointPath = FindObjectOfType<EnemyWaypointPath>();

        Speed = enemyType.typeSpeed;
    }

    void Update()
    {
        MoveTowardsWaypoints();
    }

    private void MoveTowardsWaypoints()
    {
        if (currentWaypointIndex < waypointPath.enemyWaypoints.Length)
        {
            targetWaypoint = waypointPath.enemyWaypoints[currentWaypointIndex];
            gameObject.transform.LookAt(targetWaypoint);
            currentDirection = targetWaypoint.position - transform.position;
            transform.Translate(currentDirection.normalized * speed * Time.deltaTime, Space.World);

            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                currentWaypointIndex++;
            }
        }
    }

    public void SetEnemyWaypointIndexBeginingValue()
    {
        CurrentWaypointIndex = 0;
    }

    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }

    public float GetSpeedValue()
    {
        return Speed;
    }
}