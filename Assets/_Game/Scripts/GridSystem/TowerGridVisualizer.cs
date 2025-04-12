using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGridVisualizer : MonoBehaviour
{
    [SerializeField] GameObject gridObject;

    public GameObject GridObject { get => gridObject; set => gridObject = value; }

    public void SetGridVisual(bool isVisible)
    {
        GridObject.SetActive(isVisible);
    }
}
