using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;

    public static event Action<int> OnGoldChanged;

    [SerializeField] private LevelDataSO currentLevelData;

    // public -> get, private set
    public int currentGold { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        currentGold = currentLevelData.levelBeginnigGold;
    }

    public void AddGold(int amount)
    {
        currentGold += amount;

        OnGoldChanged?.Invoke(currentGold);
    }

    public void RemoveGold(int amount)
    {
        currentGold -= amount;

        OnGoldChanged?.Invoke(currentGold);
    }

    public int GetCurrentGold()
    {
        return currentGold;
    }
}