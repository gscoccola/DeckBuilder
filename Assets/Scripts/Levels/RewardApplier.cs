using System;
using UnityEngine;
using System.Collections.Generic;

public static class RewardApplier
{
    public static List<CardSO> ApplyRewards(List<Reward> rewards, List<CardSO> deck)
    {
        List<CardSO> result = new List<CardSO>(deck);
        foreach (var reward in rewards)
        {
            reward.ApplyReward(result);
        }
        return result;
    }
}

[Serializable]
public class Reward
{
    public RewardType Type;
    public CardSO AddedCard;
    public CardSO RemovedCard;

    public void ApplyReward(List<CardSO> deck)
    {
        if (AddedCard != null) deck.Add(AddedCard);
        if (RemovedCard != null) deck.Remove(RemovedCard);
    }
}

public enum RewardType
{
    Add,
    Upgrade,
    Remove,
    Transform,
}