using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DeckDisplay : Singleton<DeckDisplay>
{
    [Header ("Parameters")]
    [SerializeField] private int _rowLenght = 5;

    [Header ("References")]
    [SerializeField] private CardDisplay _cardDisplayPrefab;
    [SerializeField] private GameObject _panel;
    [SerializeField] private Transform _layoutGroup;
    private List<CardDisplay> _cardDisplays = new List<CardDisplay>();

    private void Start()
    {
        _panel.SetActive(false);
        BuildDisplayPool(16);
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
        int rowNumber = 0;
        int colNumber = 0;

        for (int i = 0; i < activeCount; i++)
        {
            
            if (colNumber == _rowLenght)
            {
                rowNumber++;
                colNumber = 0;
            }
            _cardDisplays[i].transform.position = -(2.3f * _rowLenght / 2f) * Vector3.right + new Vector3((colNumber + 0.5f) * 2.3f, 2f - rowNumber * 3f, 0f);
            colNumber++;
        }
    }
}
