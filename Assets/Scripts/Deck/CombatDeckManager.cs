using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using System;

public class CombatDeckManager : Singleton<CombatDeckManager>
{
    [SerializeField][ReadOnly] private List<CardInstance> _deck;
    [SerializeField][ReadOnly] private List<CardInstance> _drawPile;
    [SerializeField][ReadOnly] private List<CardInstance> _discardPile;
    [SerializeField] private GameObject _backPrefab;

    public Transform DrawPileLocation;
    public Transform DiscardPileLocation;
    private List<GameObject> _backPool = new();

    #region INITIAL SETUP
    public void Initialize()
    {
        BuildDeck();
        InstantiateDisplayPrefabs();
    }

    private void InstantiateDisplayPrefabs()
    {
        for (int i = 0; i < _deck.Count + 3; i++)
        {
            GameObject cardBack = Instantiate(_backPrefab, DiscardPileLocation.position, Quaternion.identity, transform.GetChild(0));
            _backPool.Add(cardBack);
            cardBack.SetActive(false);
        }
    }

    private void BuildDeck()
    {
        _deck = new List<CardInstance>();
        foreach (var card in PersistentDeckManager.Instance.GetDeck())
        {
            _deck.Add(new CardInstance(card));
        }
    }
    #endregion

    #region COMBAT START
    public void OnCombatStart()
    {
        _drawPile = new List<CardInstance>(_deck);
        _drawPile.Shuffle();
        _discardPile = new List<CardInstance>();
        HeadsUpDisplay.Instance.UpdateDeckValues(_drawPile.Count, _discardPile.Count);
        //DeckDisplay.Instance.BuildDisplayPool(_deck.Count);
    }

    #endregion

    #region DRAW, SHUFFLE, DISCARD
    public void AttemptDrawCard(out bool causedReshuffle)
    {
        causedReshuffle = false;
        if (HandManager.Instance.IsHandFull)
        {
            MessageManager.Instance.DisplayMessage(DialogMessage.HandFull);
            
        }
        else if (_drawPile.Count > 0)
        {
            DrawCard();
        }
        else if (_discardPile.Count > 0)
        {
            causedReshuffle = true;
            ReshuffleAndDraw();
        }
        else MessageManager.Instance.DisplayMessage(DialogMessage.NoCardsLeft);
    }

    private void DrawCard()
    {
        SFXManager.Instance.PlaySFX(SFXManager.Instance.CardFlush, true);
        CardInstance drawnCard = _drawPile[0];
        _drawPile.RemoveAt(0);
        HandManager.Instance.AddCard(drawnCard);
        HeadsUpDisplay.Instance.UpdateDeckValues(_drawPile.Count, _discardPile.Count);
    }

    public void ReshuffleAndDraw() 
    {         
        StartCoroutine(ShuffleAnimation(_discardPile.Count));
    }

    private IEnumerator ShuffleAnimation(int count)
    {
        SFXManager.Instance.PlaySFX(SFXManager.Instance.CardFlush, true, 0.5f);
        ActionQueue.Instance.IsShuffling = true;
        for (int i = 0; i < count; i++)
        {
            GameObject back = _backPool[i];
            back.SetActive(true);
            back.transform.DOMove(DrawPileLocation.position, 0.5f).OnComplete(() => 
            {
                back.transform.position = DiscardPileLocation.position;
                back.SetActive(false);
            });
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.4f);
        ActionQueue.Instance.IsShuffling = false;
        _drawPile.AddRange(_discardPile);
        _discardPile.Clear();
        _drawPile.Shuffle();
        HeadsUpDisplay.Instance.UpdateDeckValues(_drawPile.Count, _discardPile.Count);
        DrawCard();
        DrawManager.Instance.OnShuffleFinished();
    }

    public void AddToDiscard(CardInstance card)
    {
        if (card == null) Debug.LogError("Null card discarded");
        _discardPile.Add(card);
        HeadsUpDisplay.Instance.UpdateDeckValues(_drawPile.Count, _discardPile.Count);
    }
    #endregion

    #region PILE DISPLAY
    public void ViewDrawPile()
    {
        DeckDisplay.Instance.Display(_drawPile);
    }

    public void ViewDiscardPile()
    {
        DeckDisplay.Instance.Display(_discardPile);
    }
    #endregion

    #region CARD EFFECT MODIFICATION

    public void ModifyAllCardEffects(Func<CardEffect[], CardEffect[]> modification)
    {
        foreach (var card in _deck)
        {
            card.Stats.Effects = modification(card.Stats.Effects);

        }
        HandManager.Instance.UpdateDisplays();
    }


    #endregion
}