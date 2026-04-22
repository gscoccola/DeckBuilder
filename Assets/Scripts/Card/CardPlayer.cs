using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlayer : Singleton<CardPlayer>
{
    private CardEffect[] _effects;
    private int _effectIndex;
    private CardDraggable _draggable;
    private CardInstance _cardInstance;
    private bool _waitingForDraw;
    private bool _exhaust;

    public void Play(CardInstance cardInstance, CardDraggable draggable)
    {
        _exhaust = false;
        _cardInstance = cardInstance;
        ActionQueue.Instance.IsPlayingCard = true;
        _effects = cardInstance.Stats.Effects;
        _effectIndex = 0;
        _draggable = draggable;

        StartCoroutine(IPerformEffect(_effects[_effectIndex]));
        _effectIndex++;
    }

    public void OnDrawFinished()
    {
        if (!_waitingForDraw) return;
        OnEffectFinished();
        _waitingForDraw = false;
    }

    public void OnEffectFinished()
    {
        if (_effectIndex < _effects.Length)
        {
            _effectIndex++;
            StartCoroutine(IPerformEffect(_effects[_effectIndex - 1]));
        }
        else
        {
            OnAllEffectsFinished();
        }
    }

    private void OnAllEffectsFinished()
    {
        _draggable.OnFinishPlaying(_exhaust);
        ActionQueue.Instance.IsPlayingCard = false;
    }


    private IEnumerator IPerformEffect(CardEffect effect)
    {
        if(effect.Type == EffectType.DrawCard)
        {
            DrawManager.Instance.DrawCards(effect.Amount);
            _waitingForDraw = true;
            yield break;
        }

        else if (effect.Type == EffectType.ExhaustSelf)
        {
            _exhaust = true;
            OnEffectFinished();
            yield break;
        }


        if (effect.Type == EffectType.Damage)
        {
            EnemyCharacter.Instance.Health.TakeDamage(effect.Amount);
            PlayerCharacter.Instance.CharacterAnimator.PlayAttackAnimation();
            SFXManager.Instance.PlaySFX(SFXManager.Instance.PlayerAttack, true);
        }
        else if (effect.Type == EffectType.Heal)
        {
            PlayerCharacter.Instance.Health.Heal(effect.Amount);
        }
        else if (effect.Type == EffectType.Block)
        {
            PlayerCharacter.Instance.CharacterAnimator.PlayAttackAnimation();
            PlayerCharacter.Instance.Health.AddBlock(effect.Amount);
            SFXManager.Instance.PlaySFX(SFXManager.Instance.PlayerAttack, true, 0.5f);
        }
        else if (effect.Type == EffectType.GainEnergy)
        {
            PlayerCharacter.Instance.GainEnergy(effect.Amount);
        }
        else if (effect.Type == EffectType.IncreaseSelfDamage)
        {
            foreach (var otherEffect in _cardInstance.Stats.Effects)
            {
                if (otherEffect.Type == EffectType.Damage)
                {
                    otherEffect.Amount += effect.Amount;
                }
            }
        }
        else if (effect.Type == EffectType.IncreaseAllDamage)
        {
            Func<CardEffect[], CardEffect[]> damageIncrease = (effects) =>
            {
                foreach (var otherEffect in effects)
                {
                    if (otherEffect.Type == EffectType.Damage)
                    {
                        otherEffect.Amount += effect.Amount;
                    }
                }
                return effects;
            };
            CombatDeckManager.Instance.ModifyAllCardEffects(damageIncrease);
        }
        else if (effect.Type == EffectType.AttacksExhaust)
        {
            Func<CardEffect[], CardEffect[]> attacksExhaust = (previousEffects) =>
            {
                List<CardEffect> newEffects = new List<CardEffect>(previousEffects);
                bool isAttack = false;
                foreach (var curEffect in previousEffects)
                {
                    if (curEffect.Type == EffectType.ExhaustSelf) return previousEffects;
                    if (curEffect.Type == EffectType.Damage) isAttack = true;
                }
                if (isAttack) newEffects.Add(new CardEffect(EffectType.ExhaustSelf, 0));
                return newEffects.ToArray();
            };
            CombatDeckManager.Instance.ModifyAllCardEffects(attacksExhaust);
        }
        yield return new WaitForSeconds(0.35f);
        OnEffectFinished();
    }
}
