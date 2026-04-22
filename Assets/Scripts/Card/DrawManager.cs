using UnityEngine;
using System.Collections;

public class DrawManager : Singleton<DrawManager>
{
    private int _cardsToDraw;
    public void DrawCards(int amount)
    {
        _cardsToDraw = amount;
        StartCoroutine(IDrawCards(amount));
    }

    public void OnShuffleFinished()
    {
        if (_cardsToDraw == 0)
        {
            CardPlayer.Instance.OnDrawFinished();
            return;
        }
        StartCoroutine(IDrawCards(_cardsToDraw));
    }

    private IEnumerator IDrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            _cardsToDraw--;
            CombatDeckManager.Instance.AttemptDrawCard(out bool causedReshuffle);
            if (causedReshuffle) yield break;
            else yield return new WaitForSeconds(0.1f);
        }
        CardPlayer.Instance.OnDrawFinished();
    }
}
