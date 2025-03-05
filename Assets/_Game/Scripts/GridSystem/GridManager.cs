using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GridData gridData;
    public float gridSize = 1f;

    private void Start()
    {
        gridData = new GridData();
    }

    public Vector3 GetGridPosition(Vector3 worldPos)
    {
        float x = worldPos.x;

        if (x < 0)
        {
            x -= gridSize;
        }

        x -= x % gridSize;

        float y = worldPos.y;

        if (y < 0)
        {
            y -= gridSize;
        }

        y -= y % gridSize;


        float z = worldPos.z;

        if (z < 0)
        {
            z -= gridSize;
        }

        z -= z % gridSize;


        return new Vector3(x, y, z);
    }

    public bool CheckTowerPlacements(Vector3 towerPosition, Vector2 towerSize)
    {
        List<Vector3> towerPos = gridData.CalculatePositions(towerPosition, towerSize);

        foreach (Vector3 pos in towerPos)
        {
            Debug.Log(pos);
            if (gridData.placedTowers.ContainsKey(pos))
            {
                Debug.Log("This grid is occupied!");
                return false;
            }
        }

        if (gridData.placedTowers.ContainsKey(towerPosition))
        {
            Debug.Log("This grid is occupied!");
            return false;
        }
        else
        {
            Debug.Log("This grid is empty");
            return true;
        }

    }
}


public class GridData
{
    public Dictionary<Vector3, PlacementData> placedTowers = new();

    public void AddTowerAt(Vector3 gridPosition, Vector2 towerSize)
    {
        List<Vector3> placedTowerPositions = CalculatePositions(gridPosition, towerSize);

        PlacementData data = new PlacementData(placedTowerPositions);
        foreach (var position in placedTowerPositions)
        {
            if (placedTowers.ContainsKey(position))
            {
                return;
            }
            placedTowers[position] = data;
        }
    }

    public List<Vector3> CalculatePositions(Vector3 gridPosition, Vector2 towerSize)
    {
        List<Vector3> placedTowerPositions = new();

        for (int i = 0; i < (int)towerSize.x; i++)
        {
            for (int j = 0; j < (int)towerSize.y; j++)
            {
                // GridSize is hardcoded at the moment and the value is 2
                placedTowerPositions.Add(gridPosition + new Vector3(i * 2, 0, j * 2));
            }
        }
        return placedTowerPositions;
    }
}

public class PlacementData
{
    public List<Vector3> placements;

    public PlacementData(List<Vector3> placements)
    {
        this.placements = placements;
    }
}
