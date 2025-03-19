using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class payConditions
{
    public string payCondition;
    public List<string> Rewards;
}

public class RewardConditioning : MonoBehaviour
{
    public List<payConditions> payConditionList;
}
