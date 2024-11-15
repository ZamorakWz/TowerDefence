using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SupriseBoxManagerTests
{
    private SupriseBoxManager supriseBoxManager;
    private AbstractBaseTower mockTower;

    [SetUp]
    public void Setup()
    {
        var supriseBoxGameObject = new GameObject();
        supriseBoxManager = supriseBoxGameObject.AddComponent<SupriseBoxManager>();

        var towerGameObject = new GameObject();
        mockTower = towerGameObject.AddComponent<GreenTower>();
    }

    [Test]
    public void ChooseSupriseRandomly_ModifiesOneAttribute()
    {

    }
}