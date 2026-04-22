using UnityEngine;
using System.Collections.Generic;

public class PersistentDeckManager : PersistentSingleton<PersistentDeckManager>
{
    [SerializeField] private CardSO[] _starterCards;
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

    #region PERSISTENT ALTERATIONS
    public void AddCard(CardSO card)
    {
        _deck.Add(card);
    }

    public void RemoveCard(CardSO card)
    {
        _deck.Remove(card);
    }
    #endregion
}
