using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;
public class PlayerCharacter : Singleton<PlayerCharacter>
{
    public int MaxEnergy;
    public int CurrentEnergy;

    public int CardDraw;

    [HideInInspector] public Health Health;
    [HideInInspector] public CharacterAnimator CharacterAnimator;
    [SerializeField] private EntityStatsSO _entityStats;

    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private SpriteRenderer _shadowSprite;

    public void Initialize()
    {
        CharacterAnimator = GetComponent<CharacterAnimator>();
        Health = GetComponent<Health>();
        Health.Initialize(_entityStats);
        Health.OnDeath.AddListener(OnDeath);
        HeadsUpDisplay.Instance.UpdateEnergyValues(CurrentEnergy, MaxEnergy);
    }

    public void UseEnergy(int amount)
    {
        CurrentEnergy -= amount;
        HeadsUpDisplay.Instance.UpdateEnergyValues(CurrentEnergy, MaxEnergy);
    }

    public void GainEnergy(int amount)
        {
            CurrentEnergy += amount;
            if (CurrentEnergy > MaxEnergy)
                CurrentEnergy = MaxEnergy;
    
            HeadsUpDisplay.Instance.UpdateEnergyValues(CurrentEnergy, MaxEnergy);
    }

    public void OnTurnStart()
    {
        CurrentEnergy = MaxEnergy;
        HeadsUpDisplay.Instance.UpdateEnergyValues(CurrentEnergy, MaxEnergy);

        Health.RemoveBlock();
    }

    private void OnDeath()
    {
        SFXManager.Instance.PlaySFX(SFXManager.Instance.PlayerDeath, false);
        ActionQueue.Instance.IsLevelOver = true;

        _sprite.DOFade(0, 2f);
        _shadowSprite.DOFade(0, 2f);
        _sprite.transform.DOMove(_sprite.transform.position + new Vector3(-0.4f, 0.2f, 0), 2f);
        _shadowSprite.transform.DOMove(_shadowSprite.transform.position + new Vector3(-0.4f, 0f, 0), 2f); ;


        StartCoroutine(ILevelOver());
    }

    private IEnumerator ILevelOver()
    {
        yield return new WaitForSeconds(2f);
        CombatManager.Instance.EndCombat(false);
    }
}
