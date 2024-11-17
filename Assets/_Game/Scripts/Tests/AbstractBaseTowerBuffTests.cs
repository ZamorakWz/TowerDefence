using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AbstractBaseTowerBuffTests
{
    private AbstractBaseTower tower;
    private SupriseBoxManager supriseBoxManager;

    [SetUp]
    public void Setup()
    {
        //Firstly make comment to AbstractBaseTower > LateUpdate > if Statement
        var towerGameObject = new GameObject();
        tower = towerGameObject.AddComponent<GreenTower>();

        var towerTypeSO = ScriptableObject.CreateInstance<TowerTypeSO>();
        towerTypeSO.towerDamage = 10;
        towerTypeSO.towerRange = 5;
        towerTypeSO.towerFireRate = 3;

        towerGameObject.AddComponent<LineRenderer>();

        // Attach necessary mock components
        towerGameObject.AddComponent<SphereTargetDetector>(); // Mock target detector
        var mockCollider = towerGameObject.AddComponent<SphereCollider>();
        mockCollider.isTrigger = true;

        towerGameObject.AddComponent<TowerRangeVisualizer>(); // Mock tower range visualizer

        // Set up firePoint
        var firePoint = new GameObject("FirePoint").transform;
        firePoint.parent = towerGameObject.transform;
        tower.GetType().GetField("firePoint", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
             ?.SetValue(tower, firePoint);

        tower.Initialize(towerTypeSO);
    }

    [UnityTest]
    public IEnumerator TowerBuff_ModifDamageTemporarily()
    {
        float initialDamage = tower.towerDamage;
        float amount = 5f;
        float duration = 2f;

        var modifyDamageCoroutine = tower.StartCoroutine(tower.ModifyDamageTemporarily(amount, duration));
        yield return new WaitForSeconds(duration / 2);

        // Verify that the damage was increased
        Assert.AreEqual(initialDamage + amount, tower.towerDamage, 0.1f);
        Debug.Log($"Expected Damage: {initialDamage + amount}, Actual Damage: {tower.towerDamage}");

        // Wait for the duration plus a small buffer
        yield return modifyDamageCoroutine;

        yield return new WaitForSeconds(0.1f);
        // Verify that the damage has been reverted
        Assert.AreEqual(initialDamage, tower.towerDamage, 0.1f);
        Debug.Log($"Expected Damage: {initialDamage}, Actual Damage: {tower.towerDamage}");
    }

    [UnityTest]
    public IEnumerator TowerBuff_ModifRangeTemporarily()
    {
        float initialRange = tower.towerRange;
        float amount = 10f;
        float duration = 2f;

        var modifyRangeCoroutine = tower.StartCoroutine(tower.ModifyRangeTemporarily(amount, duration));
        yield return new WaitForSeconds(duration / 2);

        // Verify that the range was increased
        Assert.AreEqual(initialRange + amount, tower.towerRange, 0.1f);
        Debug.Log($"Expected Range: {initialRange + amount}, Actual Range: {tower.towerRange}");

        // Wait for the duration plus a small buffer
        yield return modifyRangeCoroutine;

        yield return new WaitForSeconds(0.1f);
        // Verify that the range has been reverted
        Assert.AreEqual(initialRange, tower.towerRange, 0.1f);
        Debug.Log($"Expected Range: {initialRange}, Actual Range: {tower.towerRange}");
    }

    [UnityTest]
    public IEnumerator TowerBuff_ModifyFireRateTemporarily()
    {
        float initialFireRate = tower.towerFireRate;
        float amount = 2f;
        float duration = 2f;

        var modifyFireRateCoroutine = tower.StartCoroutine(tower.ModifyFireRateTemporarily(amount, duration));
        yield return new WaitForSeconds(duration / 2);

        // Verify that the firerate was increased
        Assert.AreEqual(initialFireRate + amount, tower.towerFireRate, 0.1f);
        Debug.Log($"Expected FireRate: {initialFireRate + amount}, Actual FireRate: {tower.towerFireRate}");

        // Wait for the duration plus a small buffer
        yield return modifyFireRateCoroutine;

        yield return new WaitForSeconds(0.1f);
        // Verify that the firerate has been reverted
        Assert.AreEqual(initialFireRate, tower.towerFireRate, 0.1f);
        Debug.Log($"Expected FireRate: {initialFireRate}, Actual FireRate: {tower.towerFireRate}");
    }
}