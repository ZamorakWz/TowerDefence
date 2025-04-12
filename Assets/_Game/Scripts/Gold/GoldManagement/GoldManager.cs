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
    public int CurrentGold { get; private set; }

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

        CurrentGold = currentLevelData.levelBeginnigGold;
    }

    public void AddGold(int amount)
    {
        CurrentGold += amount;

        OnGoldChanged?.Invoke(CurrentGold);
    }

    public void RemoveGold(int amount)
    {
        CurrentGold -= amount;

        OnGoldChanged?.Invoke(CurrentGold);
    }

    public int GetCurrentGold()
    {
        return CurrentGold;
    }
}