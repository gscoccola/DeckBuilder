using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class CardInstance
{
    public CardSO CardSO;
    public CardStats Stats;

    public CardInstance(CardSO cardSO)
    {
        CardSO = cardSO;
        Stats = new CardStats(cardSO.Stats);
    }


    public bool TryToPlay(CardDraggable draggable)
    {
        if (ActionQueue.Instance.IsPerformingAction())
        {
            return false;
        }
        if (PlayerCharacter.Instance.CurrentEnergy >= Stats.Cost)
        {
            HandArranger.Instance.RemoveCard(draggable);
            PlayerCharacter.Instance.UseEnergy(Stats.Cost);
            CardPlayer.Instance.Play(this, draggable);
            return true;
        }
        else
        {
            MessageManager.Instance.DisplayMessage(DialogMessage.NotEnoughEnergy);
            return false;
        }
    }


    public void Discard()
    {
        HandManager.Instance.OnDiscardFinish(this);
    }

    
}
