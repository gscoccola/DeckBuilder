using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class HandManager : Singleton<HandManager>
{
    [SerializeField] private int _maxHandSize = 10;
    [SerializeField] private GameObject _displayPrefab;
    [SerializeField] private int _baseDraw = 5;

    [SerializeField][ReadOnly] private List<CardInstance> _hand = new();
    [SerializeField][ReadOnly] private List<CardDisplay> _handDisplays = new();

    private List<CardDisplay> _displayPool = new();
    [HideInInspector] public bool IsHandFull => _hand.Count >= _maxHandSize;

    #region COMBAT START AND END
    public void OnCombatStart()
    {
        _hand = new();
        for (int i = 0; i < _maxHandSize + 1; i++)
        {
            _displayPool.Add(Instantiate(_displayPrefab, CombatDeckManager.Instance.DrawPileLocation.position, Quaternion.identity, transform.GetChild(0)).GetComponent<CardDisplay>());
            _displayPool[i].Clear();
        }
        OnTurnStart();
    }

    public void OnCombatEnd()
    {
        foreach (var display in _displayPool)
        {
            Destroy(display.gameObject);
        }
    }
    #endregion

    #region TURN START AND END
    public void OnTurnStart()
    {
        DrawManager.Instance.DrawCards(_baseDraw);
    }

    public void OnTurnEnd()
    {
        DiscardHand();
    }

    public void DiscardHand()
    {
        for (int i = _hand.Count - 1; i >= 0; i--)
        {
            HandArranger.Instance.RemoveCard(_handDisplays[i].Draggable);
        }
        for (int i = _hand.Count - 1; i >= 0; i--)
        {
            _handDisplays[i].Draggable.SendToDiscard();
        }
    }
    #endregion

    public void AddCard(CardInstance card)
    {
        _hand.Add(card);
        CardDisplay display = GetFreeDisplay();
        display.Assign(card);
        _handDisplays.Add(display);
        _displayPool.Remove(display);
        HandArranger.Instance.AddCard(display.Draggable);
    }

    /*public void PlayCard(CardInstance card)
    {
        card.Play();
        DiscardCard(card);
    }*/

    public void OnDiscardFinish(CardInstance card)
    {
        CombatDeckManager.Instance.AddToDiscard(card);
        CardDisplay display = _handDisplays[_hand.IndexOf(card)];
        display.Clear();
        _hand.Remove(card);
        _handDisplays.Remove(display);
        _displayPool.Add(display);
        display.transform.position = CombatDeckManager.Instance.DrawPileLocation.position;
    }

    public void OnExhaustFinish(CardInstance card)
    {
        CardDisplay display = _handDisplays[_hand.IndexOf(card)];
        display.Clear();
        _hand.Remove(card);
        _handDisplays.Remove(display);
        _displayPool.Add(display);
    }

    private CardDisplay GetFreeDisplay()
    {
        //return _displayPool.FirstOrDefault(d => !d.IsAssigned);
        return _displayPool[0];
    }

    public void UpdateDisplays()
    {
        for (int i = 0; i < _hand.Count; i++)
        {
            _handDisplays[i].UpdateDisplay();
        }
    }
}
