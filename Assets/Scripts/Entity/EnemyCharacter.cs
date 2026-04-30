using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCharacter : Singleton<EnemyCharacter>
{
    [HideInInspector] public Health Health;
    [SerializeField] private EntityStatsSO _entityStats;
    private int _intentIndex;
    [HideInInspector] public CharacterAnimator CharacterAnimator;

    [SerializeField] private IntentManager _intentManager;

    private int _strenght;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private SpriteRenderer _shadowSprite;

    public void Initialize()
    {
        CharacterAnimator = GetComponent<CharacterAnimator>();
        Health = GetComponent<Health>();
        _entityStats = PersistentInfo.Instance.CurrentLevelSO.EnemyStats;
        CharacterAnimator.Animator.runtimeAnimatorController = PersistentInfo.Instance.CurrentLevelSO.EnemyAnimator;
        Health.Initialize(_entityStats);
        Health.OnDeath.AddListener(OnDeath);
        DisplayIntent();
    }

    public void OnTurnStart()
    {

        Health.RemoveBlock();

        PerformIntent();
        DisplayIntent();
    }

    /*public void OnTurnEnd()
    {
        DisplayIntent();
    }  */ 

    private void PerformIntent()
    {
        EnemyIntent intent = _entityStats.IntentSequence[_intentIndex];
        if (intent.DMGAmount > 0)
        {
            SFXManager.Instance.PlaySFX(SFXManager.Instance.EnemyAttack, true);
        }
        else
        {
            SFXManager.Instance.PlaySFX(SFXManager.Instance.EnemyAttack, true, 0.5f);
        }

        CharacterAnimator.PlayAttackAnimation();
        if (intent.DMGAmount > 0 ) PlayerCharacter.Instance.Health.TakeDamage(intent.DMGAmount + _strenght);
        Health.AddBlock(intent.BlockAmount);
        _strenght += intent.STRGain;
        _intentIndex = (_intentIndex + 1) % _entityStats.IntentSequence.Length;
    }

    private void DisplayIntent()
    {
        EnemyIntent intent = _entityStats.IntentSequence[_intentIndex];
        _intentManager.DisplayIntent(intent);
    }

    private void OnDeath()
    {
        _intentManager.ClearIntents();
        ActionQueue.Instance.IsLevelOver = true;
        StartCoroutine(ILevelOver());
    }

    private IEnumerator ILevelOver()
    {
        SFXManager.Instance.PlaySFX(SFXManager.Instance.EnemyDeath, false, 0.5f);
        _sprite.DOFade(0, 2f);
        _shadowSprite.DOFade(0, 2f);
        _sprite.transform.DOMove(_sprite.transform.position + new Vector3(0.4f, 0.2f, 0), 2f);
        _shadowSprite.transform.DOMove(_shadowSprite.transform.position + new Vector3(0.4f, 0f, 0), 2f);
        yield return new WaitForSeconds(2f);
        CombatManager.Instance.EndCombat(true);
    }
}


[Serializable]
public class EnemyIntent
{
    public int DMGAmount;
    public int BlockAmount;
    public int STRGain;
    public EnemyIntent(int dmgAmount, int blockAmount, int strGain)
    {
        DMGAmount = dmgAmount;
        BlockAmount = blockAmount;
        STRGain = strGain;
    }
}
