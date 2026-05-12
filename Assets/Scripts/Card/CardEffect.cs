using System;
using UnityEngine;

[Serializable]
public class CardEffect
{
    public EffectType Type;
    public int Amount;

    public CardEffect(EffectType type, int amount)
    {
        Type = type;
        Amount = amount;
    }
}

public enum EffectType
{
    Damage,
    Heal,
    Block,
    DrawCard,
    GainEnergy,
    IncreaseSelfDamage,
    ExhaustSelf,
    IncreaseAllDamage,
    AttacksExhaust,
    ApplyVulnerable,
}

public static class DescriptionGenerator
{
    public static string GenerateDescription(CardEffect[] effects)
    {
        string description = "";
        foreach (CardEffect effect in effects)
        {
            switch (effect.Type)
            {
                case EffectType.Damage:
                    description += $"Deal {effect.Amount} damage\n";
                    break;
                case EffectType.Heal:
                    description += $"Heal {effect.Amount} health\n";
                    break;
                case EffectType.Block:
                    description += $"Gain {effect.Amount} block\n";
                    break;
                case EffectType.DrawCard:
                    description += $"Draw {effect.Amount} card(s)\n";
                    break;
                case EffectType.IncreaseSelfDamage:
                    description += $"Increase this card's damage by {effect.Amount}\n";
                    break;
                case EffectType.ExhaustSelf:
                    description += $"Exhaust this card.\n";
                    break;
                case EffectType.IncreaseAllDamage:
                    description += $"Increase all your attacks' damage by {effect.Amount}\n";
                    break;
                case EffectType.AttacksExhaust:
                    description += $"Attacks exhaust when played\n";
                    break;
                case EffectType.ApplyVulnerable:
                    description += $"Apply {effect.Amount} vulnerable\n";
                    break;
            }
        }
        return description.TrimEnd('\n');
    }
}