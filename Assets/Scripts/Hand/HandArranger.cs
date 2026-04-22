using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class HandArranger : Singleton<HandArranger>
{
    private List<Transform> _transforms = new();
    private List<CardDraggable> _draggables = new();

    public void AddCard(CardDraggable card)
    {
        _draggables.Add(card);
        _transforms.Add(card.transform);
        ArrangeCards();
    }

    public void RemoveCard(CardDraggable card)
    {
        //card.transform.position = Vector3.up * 1000f;
        _draggables.Remove(card);
        _transforms.Remove(card.transform);
        ArrangeCards();
    }

    private void ArrangeCards()
    {
        int cardCount = _transforms.Count;
        float spacing = 2f - (cardCount - 1) * 0.05f;
        for (int i = 0; i < cardCount; i++)
        {
            float centerDist = (i + (1 - cardCount) / 2f);
            Vector3 targetPosition = spacing * Vector3.right * centerDist - Vector3.up * 0.06f * centerDist * centerDist;
            Vector3 targetRotation = -Vector3.forward * centerDist * 6f;
            //_transforms[i].localPosition = targetPosition;
            //_transforms[i].localEulerAngles = targetRotation;
            _transforms[i].DOLocalMove(targetPosition, 0.5f);
            _transforms[i].DOLocalRotate(targetRotation, 0.5f);


            _draggables[i].SetReturnTransform(targetPosition, targetRotation.z, i * 4);
            _draggables[i].AssignedDisplay.SetOrder(i * 4);
        }
    }
}
