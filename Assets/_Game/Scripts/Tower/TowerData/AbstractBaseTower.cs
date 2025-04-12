using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

public abstract class AbstractBaseTower : MonoBehaviour
{
    [SerializeField] protected TowerTypeSO towerData;
    [SerializeField] protected Transform firePoint;
    [SerializeField] private TextMeshProUGUI buffText;
    private BulletObjectPool bulletObjectPool;

    public List<ITargetSelectionStrategy> availableStrategies { get; private set; } = new List<ITargetSelectionStrategy>();

    //Attributes for each tower
    public float towerDamage { get; private set; }
    public float towerFireRate { get; private set; }
    public float towerRange { get; private set; }

    //TowerCost
    public float towerCost { get; private set; }

    //Temporary attributes
    private float temporaryTowerDamage;
    private float temporaryTowerFireRate;
    private float temporaryTowerRange;

    //Upgrade levels for each property
    public int upgradeLevel { get; private set; } = 0;

    private int baseUpgradeCost = 4;

    private float lastAttackTime;

    protected IAttackStrategy attackStrategy;
    protected ITargetDetector targetDetector;
    public ITargetSelectionStrategy targetSelectionStrategy;
    protected IAttackable currentTarget;
    protected IAttackManager attackManager;
    protected ITowerRangeUpdater towerRangeVisualizer;

    public bool isTowerPlaced { get; private set; }

    protected Vector3 towerPosition;

    private TowerDataUI towerDataUI;

    public bool isBuffActive { get; private set; }

    public bool isTowerInitialized { get; private set; }

    protected Coroutine attackRoutine;

    #region ------------------------------MONOBEHAVIOURS------------------------------
    private void Awake()
    {
        towerDataUI = GetComponent<TowerDataUI>();
        bulletObjectPool = FindAnyObjectByType<BulletObjectPool>();
    }

    private void LateUpdate()
    {
        if (buffText.gameObject.activeSelf)
        {
            buffText.gameObject.transform.rotation = Camera.main.transform.rotation;
        }
    }

    protected virtual void OnEnable()
    {
        SubscribeEvent();
    }

    //Event was unsubscribed on disable,
    //it caused missing reference exception
    //because OnTowerPlaced event tried to reach old subscriber object.
    //This is why i added OnDisable method
    protected virtual void OnDisable()
    {
        UnSubscribeEvent();

        StopAttackRoutine();
    }
    protected virtual void OnDestroy()
    {
        UnSubscribeEvent();

        StopAttackRoutine();
    }
    #endregion

    #region ------------------------------TOWER UPGRADE------------------------------
    public float GetUpgradeCost(int level)
    {
        return baseUpgradeCost * Mathf.Pow(2, level);
    }

    public void UpgradeTower()
    {
        int cost = (int)GetUpgradeCost(upgradeLevel);
        if (GoldManager.Instance.GetCurrentGold() >= cost)
        {
            GoldManager.Instance.RemoveGold(cost);
            upgradeLevel++;
            towerDamage *= 1.1f;
            towerRange *= 1.1f;
            towerFireRate *= 1.1f;

            targetDetector.UpdateRange(towerRange);
            towerRangeVisualizer.UpdateTowerRangeVisualization(towerRange);

            //update tower damage
            if (attackManager != null)
            {
                attackManager.UpdateDamage(towerDamage);
                attackManager.UpdateFireRate(towerFireRate);
            }
        }
    }

    #endregion

    #region------------------------------TOWER ATTACK------------------------------
    //protected async UniTaskVoid AttackRoutine()
    //{
    //    while (isActiveAndEnabled)
    //    {
    //        if (isTowerPlaced)
    //        {
    //            PerformAttackTasks();
    //        }

    //        await UniTask.Delay(100);
    //    }
    //}

    //protected virtual void PerformAttackTasks()
    //{
    //    if (currentTarget == null || !IsTargetValid(currentTarget))
    //    {
    //        SelectNewTarget();
    //    }

    //    if (attackManager.CanAttack())
    //    {
    //        var targets = targetSelectionStrategy.SelectTargets(targetDetector.GetTargetsInRange(), transform.position);
    //        attackManager.Attack(targets);
    //    }
    //}

    protected IEnumerator AttackRoutine()
    {
        while (isActiveAndEnabled)
        {
            if (isTowerPlaced)
            {
                PerformAttackTasks();
            }

            yield return new WaitForSeconds(0.1f); // 100ms
        }
    }

    protected virtual void PerformAttackTasks()
    {
        if (currentTarget == null || !IsTargetValid(currentTarget))
        {
            SelectNewTarget();
        }

        if (attackManager.CanAttack())
        {
            var targets = targetSelectionStrategy.SelectTargets(targetDetector.GetTargetsInRange(), transform.position);
            attackManager.Attack(targets);
        }
    }

    public void StartAttackRoutine()
    {
        if (attackRoutine == null)
        {
            attackRoutine = StartCoroutine(AttackRoutine());
            Debug.Log("AttackRoutine is Started!");
        }
    }

    public void StopAttackRoutine()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            Debug.Log("AttackRoutine is Stopped!");
            attackRoutine = null;
        }
    }
    #endregion

    #region------------------------------TOWER INITIALIZE------------------------------
    public virtual void Initialize(TowerTypeSO towerData)
    {
        if (isTowerInitialized == false)
        {
            this.towerData = towerData;

            //Make independence the informations that come from scriptable objects
            towerDamage = towerData.towerDamage;
            towerFireRate = towerData.towerFireRate;
            towerRange = towerData.towerRange;

            //Cost
            towerCost = towerData.towerCost;


            //Setup targetdetection
            InitializeTargetDetectionStrategy();

            //TowerRangeVisualizer
            towerRangeVisualizer = gameObject.GetComponent<ITowerRangeUpdater>();

            //Setup strategies
            InitializeAttackStrategy();
            InitializeTargetSelectionStrategy();

            //Setup attackmanager
            attackManager = new AttackManager(attackStrategy,
                 towerFireRate,
                 towerDamage, firePoint, gameObject, bulletObjectPool);

            InitializeStrategiesList();

            towerDataUI.UpdateStrategyDropdown();

            isTowerInitialized = true;
        }
    }

    protected abstract void InitializeAttackStrategy();
    protected abstract void InitializeTargetSelectionStrategy();
    protected abstract void InitializeTargetDetectionStrategy();

    public TowerTypeSO GetTowerData()
    {
        return towerData;
    }
    #endregion

    #region------------------------------TOWER PLACEMENT------------------------------
    protected Vector3 GetTowerPosition()
    {
        return towerPosition != Vector3.zero ? towerPosition : transform.position;
    }

    protected virtual void HandleTowerPlaced(GameObject tower)
    {
        if (tower != null && tower.activeInHierarchy && tower == this.gameObject && isTowerPlaced == false)
        {
            towerPosition = transform.position;

            Initialize(towerData);

            StartAttackRoutine();

            Debug.Log($"{tower.gameObject.name} has been fully initialized and attack routine has started.");

            isTowerPlaced = true;
        }
    }

    #endregion

    #region ------------------------------TOWER SUPRISEBOX ATTRIBUTE CHANGE------------------------------
    public IEnumerator ModifyDamageTemporarily(float amount, float duration)
    {
        if (isBuffActive)
        {
            yield break;
        }

        isBuffActive = true;

        if (temporaryTowerDamage == 0)
        {
            temporaryTowerDamage = towerDamage;
            towerDamage += amount;

            if (towerDamage < 0)
            {
                towerDamage = 0;
            }

            attackManager.UpdateDamage(towerDamage);
        }

        yield return new WaitForSeconds(duration);

        towerDamage = temporaryTowerDamage;
        temporaryTowerDamage = 0;

        attackManager.UpdateDamage(towerDamage);

        isBuffActive = false;
    }

    public IEnumerator ModifyRangeTemporarily(float amount, float duration)
    {
        if (isBuffActive)
        {
            yield break;
        }

        isBuffActive = true;

        if (temporaryTowerRange == 0)
        {
            temporaryTowerRange = towerRange;
            towerRange += amount;

            if (towerRange < 0)
            {
                towerRange = 0;
            }

            targetDetector.UpdateRange(towerRange);
            towerRangeVisualizer.UpdateTowerRangeVisualization(towerRange);
        }

        yield return new WaitForSeconds(duration);

        towerRange = temporaryTowerRange;
        temporaryTowerRange = 0;

        targetDetector.UpdateRange(towerRange);
        towerRangeVisualizer.UpdateTowerRangeVisualization(towerRange);

        isBuffActive = false;
    }

    public IEnumerator ModifyFireRateTemporarily(float amount, float duration)
    {
        if (isBuffActive)
        {
            yield break;
        }

        isBuffActive = true;

        if (temporaryTowerFireRate == 0)
        {
            temporaryTowerFireRate = towerFireRate;
            towerFireRate += amount;

            if (towerFireRate < 0)
            {
                towerFireRate = 0;
            }

            attackManager.UpdateFireRate(towerFireRate);
        }

        yield return new WaitForSeconds(duration);

        towerFireRate = temporaryTowerFireRate;
        temporaryTowerFireRate = 0;

        attackManager.UpdateFireRate(towerFireRate);

        isBuffActive = false;
    }

    public IEnumerator ShowEffectText(string effectType, int amount, int duration)
    {
        int remainingDuration = duration;

        buffText.gameObject.SetActive(true);

        while (remainingDuration > 0)
        {
            buffText.text = $"{effectType} {(amount > 0 ? "+" : "")}{amount} for {remainingDuration} seconds";

            yield return new WaitForSeconds(1f);

            remainingDuration -= 1;
        }

        buffText.gameObject.SetActive(false);
    }
    #endregion

    #region------------------------------TARGET SELECTION------------------------------
    public List<IAttackable> GetTargetsInRange()
    {
        return targetDetector.GetTargetsInRange().OfType<IAttackable>().ToList();
    }

    protected virtual void SelectNewTarget()
    {
        var targets = targetDetector.GetTargetsInRange();
        var selectedTargets = targetSelectionStrategy.SelectTargets(targets, transform.position);
        currentTarget = selectedTargets.FirstOrDefault();
    }

    protected bool IsTargetValid(IAttackable target)
    {
        bool isValid = GetTargetsInRange().Contains(target);
        return isValid;
    }
    #endregion

    #region------------------------------TARGET SELECTION STRATEGY------------------------------

    protected virtual void InitializeStrategiesList()
    {
        availableStrategies.Clear();

        availableStrategies.Add(new NearestTarget());
        availableStrategies.Add(new FastestTarget());
        availableStrategies.Add(new MostHealthTarget());
        availableStrategies.Add(new MostProgressTarget());
    }

    public virtual List<ITargetSelectionStrategy> GetAvailableStrategies()
    {
        return availableStrategies;
    }

    public virtual void ChangeTargetSelectionStrategy(ITargetSelectionStrategy newStrategy)
    {
        StopAttackRoutine();

        targetSelectionStrategy = newStrategy;
        Debug.Log($"{targetSelectionStrategy} is changing to: {newStrategy}");

        SelectNewTarget();

        StartAttackRoutine();
    }
    #endregion

    #region------------------------------Events------------------------------
    protected void SubscribeEvent()
    {
        Debug.Log("Subscribe Event");
        TowerPlacementManager.OnTowerPlaced += HandleTowerPlaced;
    }

    protected void UnSubscribeEvent()
    {
        Debug.Log("UNSUBSCRIBE");
        TowerPlacementManager.OnTowerPlaced -= HandleTowerPlaced;
    }
    #endregion
}