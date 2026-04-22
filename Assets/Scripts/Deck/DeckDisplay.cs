using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DeckDisplay : Singleton<DeckDisplay>
{
    [SerializeField] private CardDisplay _cardDisplayPrefab;
    [SerializeField] private GameObject _panel;
    private List<CardDisplay> _cardDisplays = new List<CardDisplay>();
    [SerializeField] private Transform _layoutGroup;

    private void Start()
    {
        _panel.SetActive(false);
    }

    public void BuildDisplayPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            CardDisplay display = Instantiate(_cardDisplayPrefab, _layoutGroup);
            _cardDisplays.Add(display);
            display.gameObject.SetActive(false);
        }
    }

    public void Display(List<CardInstance> cards)
    {
        if (cards.Count == 0)
        {
            MessageManager.Instance.DisplayMessage(DialogMessage.EmptyPile);
            return;
        }
        List<CardInstance> sortedCards = cards.OrderBy(card => card.CardSO.ID).ToList();
        int i = 0;
        foreach (var card in sortedCards)
        {
            var display = _cardDisplays[i];
            display.gameObject.SetActive(true);
            display.Assign(card, DisplayType.Deck);
            i++;
        }
        Arrange(cards.Count);
        _panel.SetActive(true);
    }

    public void Close()
    {
        foreach (var display in _cardDisplays)
        {
            display.Clear();
            display.gameObject.SetActive(false);
        }
        _panel.SetActive(false);
    }

    private void Arrange(int activeCount)
    {
        for (int i = 0; i < activeCount; i++)
        {
            _cardDisplays[i].transform.position = -(activeCount / 2f) * Vector3.right + new Vector3(i * 2.3f, 0f, 0f);
        }
    }
}
