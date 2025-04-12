using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageController : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private EnemyTypeSO _enemyTypeSO;
    private EnemyHealthController _healthController;

    private EnemySpawnController enemySpawnController;

    private void Start()
    {
        enemySpawnController = FindObjectOfType<EnemySpawnController>();
        _healthController = GetComponent<EnemyHealthController>();
        _damage = _enemyTypeSO.typeDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        var damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null )
        {
            Debug.Log($"Base damaged! {gameObject.name}, {_damage}");
            damageable.TakeDamage(_damage);

            if (other.CompareTag("Base"))
            {
                gameObject.SetActive(false);

                enemySpawnController.AliveEnemyCount--;
            }
            else
            {
                _healthController.Die();
            }
        }
    }
}