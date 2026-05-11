using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] private GameObject _enemyPrefab;
    public List<EnemyCharacter> Enemies = new();
    public void Initialize(EntityStatsSO[] enemies, int[] intentStart)
    {
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
        EnemyCharacter newEnemy = Instantiate(_enemyPrefab, transform.position - index * Vector3.right * 4f, Quaternion.identity, transform).transform.GetChild(0).GetComponent<EnemyCharacter>();
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
        if (Enemies.Count <= 0)
        {
            ActionQueue.Instance.IsLevelOver = true;
            StartCoroutine(ILevelOver());
        }
    }

    private IEnumerator ILevelOver()
    {
        yield return new WaitForSeconds(2f);
        CombatManager.Instance.EndCombat(true);
    }
}
