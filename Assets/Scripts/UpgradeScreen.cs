using System;
using UnityEngine;

public class UpgradeScreen : Singleton<UpgradeScreen>
{
    [SerializeField] private CardSO[] _cards;
    [SerializeField] private CardSO[] _cardsToReplace;
    [SerializeField] private CardDisplay[] _displays;

    private void Start()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        for (int i = 0; i < _displays.Length; i++)
        {
            _displays[i].Assign(new CardInstance(_cards[i]), DisplayType.CardChoice);
        }
    }

    public void ChooseCard(CardSO card)
    {
        int index = Array.IndexOf(_cards, card);

        SFXManager.Instance.PlaySFX(SFXManager.Instance.UIClick, false, 1.3f);
        PersistentDeckManager.Instance.AddCard(_cards[index]);
        PersistentDeckManager.Instance.RemoveCard(_cardsToReplace[index]);
        /*CombatDeckManager.Instance.AddCard(new CardInstance(_cards[index]));
        CombatDeckManager.Instance.AddCard(new CardInstance(_cardsToReplace[(index + 1) % 3]));
        CombatDeckManager.Instance.AddCard(new CardInstance(_cardsToReplace[(index + 2) % 3]));*/

        for (int i = 0; i < _displays.Length; i++)
        {
            Destroy(_displays[i]);
        }

        transform.GetChild(0).gameObject.SetActive(false);
        CombatManager.Instance.StartCombat();
    }
}
