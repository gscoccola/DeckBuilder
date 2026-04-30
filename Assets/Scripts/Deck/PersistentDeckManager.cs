using UnityEngine;
using System.Collections.Generic;

public class PersistentDeckManager : PersistentSingleton<PersistentDeckManager>
{
    [SerializeField] private List<CardSO> _starterCards;
    [SerializeField][ReadOnly] private List<CardSO> _deck;

    protected override void Awake()
    {
        base.Awake();
        _deck = new List<CardSO>(_starterCards);
    }

    public List<CardSO> GetDeck()
    {
        return _deck;
    }

    public void UpdateDeck()
    {
        _deck = RewardApplier.ApplyRewards(PersistentInfo.Instance.ChosenRewards, _starterCards);
    }

    /*
    #region PERSISTENT ALTERATIONS
    public void AddCard(CardSO card)
    {
        _deck.Add(card);
    }

    public void RemoveCard(CardSO card)
    {
        _deck.Remove(card);
    }
    #endregion*/
}
