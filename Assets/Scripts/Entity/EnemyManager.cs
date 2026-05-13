using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] private GameObject _enemyPrefab;
    public List<EnemyCharacter> Enemies = new();
    private Vector3[] _positions = new Vector3[3];
    public void Initialize(EntityStatsSO[] enemies, int[] intentStart)
    {
        for (int i = 0; i < 3; i++)
        {
            _positions[i] = -i * Vector3.right * 4f;
        }
        for (int i = 0; i < enemies.Length; i++)
        {
            int index = intentStart.Length >= i + 1 ? intentStart[i] : 0;
            SpawnEnemy(enemies[i], index);
        }
    }

    public void SpawnEnemy(EntityStatsSO enemyStats, int intentIndex = 0)
    {
        if (Enemies.Count >= 3) return;
        int index = Enemies.Count;
        EnemyCharacter newEnemy = Instantiate(_enemyPrefab, transform.position + _positions[index], Quaternion.identity, transform).transform.GetChild(0).GetComponent<EnemyCharacter>();
        newEnemy.Initialize(enemyStats, intentIndex);
        Enemies.Add(newEnemy);
    }

    public EnemyCharacter FrontEnemy()
    {
        return Enemies[Enemies.Count -1 ];
    }

    public void OnDeath(EnemyCharacter enemy)
    {
        Enemies.Remove(enemy);
        if (!enemy.IsMinon && NonMinionsLeft() == 0) EndCombat();
    }

    private IEnumerator ILevelOver()
    {
        yield return new WaitForSeconds(2f);
        CombatManager.Instance.EndCombat(true);
    }

    public void SwitchEnemies()
    {
        Enemies.Reverse();
        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].transform.DOMove(transform.position + _positions[i], 1.2f);
        }
    }

    public int NonMinionsLeft()
    {
        int count = 0;
        foreach (EnemyCharacter enemy in Enemies)
        {
            if (!enemy.IsMinon) count++;
        }
        return count;
    }

    private void EndCombat()
    {
        List<EnemyCharacter> minions = new List<EnemyCharacter>(Enemies);
        foreach (EnemyCharacter enemy in minions)
        {
            if (enemy.IsMinon) enemy.Health.TakeDamage(999999);
        }
        ActionQueue.Instance.IsLevelOver = true;
        StartCoroutine(ILevelOver());
    }
}
