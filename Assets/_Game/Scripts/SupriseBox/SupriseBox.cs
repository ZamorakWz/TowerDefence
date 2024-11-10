using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupriseBox
{
    private float effectAmount;
    private float effectDuration;

    public SupriseBox()
    {
        effectAmount = Random.Range(4f, 12f);
        effectDuration = Random.Range(-8, 12f);
    }

    //private void ChooseSupriseRandomly(AbstractBaseTower tower)
    //{
    //    int randomAttribute = Random.Range(0, 3);

    //    switch (randomAttribute)
    //    {
    //        case 0:
    //            tower.ModifyDamage(effectAmount, effectDuration);
    //            break;
    //        case 1:
    //            tower.ModifyRange(effectAmount, effectDuration);
    //            break;
    //        case 2:
    //            tower.ModifyFireRate(effectAmount, effectDuration);
    //            break;
    //    }
    //}

    //public void OnLootBoxClicked(AbstractBaseTower tower)
    //{
    //    ChooseSupriseRandomly(tower);
    //}
}