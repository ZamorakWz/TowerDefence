using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class SlotRewards : MonoBehaviour
{
    [SerializeField]
    private TowerButtonsUIManager towerButtonsUIManager;
    private RewardConditioning rewardConditioning;
    private AbstractBaseTower tower;
    private SlotManager slotManager;
    public int goldReward;

    void Start()
    {
        rewardConditioning = gameObject.GetComponent<RewardConditioning>();
        slotManager = gameObject.GetComponent<SlotManager>();
        tower = GetComponent<AbstractBaseTower>();
    }

    public void rewardAssign() 
    {
        for (int i = 0; i<slotManager._paycondition.Count; i++) 
        {
            foreach (var condition in rewardConditioning.payConditionList)
            {
                if (condition.payCondition == slotManager._paycondition[i])
                {
                    foreach (var reward in condition.Rewards)
                    {
                        MethodInfo method = GetType().GetMethod(reward);
                        if (method != null)
                        {
                            method.Invoke(this, null);
                        }
                        else
                        {
                            Debug.LogError($"Method '{reward}' not found!");
                        }
                    }
                }
            }
        }
    }

    public void halfofbet() 
    {
        GoldManager.Instance.AddGold(goldReward/2);
        Debug.Log("half of the bet returned");
    }
    public void fullofbet()
    {
        GoldManager.Instance.AddGold(goldReward);
        Debug.Log("full of the bet returned");
    }
    public void twotimesofbet()
    {
        GoldManager.Instance.AddGold(goldReward*2);
        Debug.Log("two times of the bet returned");
    }
    public void fivetimesofbet()
    {
        GoldManager.Instance.AddGold(goldReward*5);
        Debug.Log("five times of the bet returned");
    }
    public void tentimesofbet()
    {
        GoldManager.Instance.AddGold(goldReward*10);
        Debug.Log("ten times of the bet returned");
    }
    public void rewardbasetower()
    {
        towerButtonsUIManager.CreateTowerButtons();
        Debug.Log("level one tower rewarded");
    }
    public void rewardleveltwotower()
    {
        towerButtonsUIManager.CreateTowerButtons();
        Debug.Log("level two tower rewarded");
    }
}
