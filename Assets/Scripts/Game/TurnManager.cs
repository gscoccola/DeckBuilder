using UnityEngine;
using System.Collections;

public class TurnManager : Singleton<TurnManager>
{
    public IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.2f);
        ActionQueue.Instance.IsPlayerTurn = true;
    }

    public void EndTurn()
    {
        if (ActionQueue.Instance.IsPerformingAction())
        {
            return;
        }
        StartCoroutine(INewTurnCoroutine());
    }



    private IEnumerator INewTurnCoroutine()
    {
        ActionQueue.Instance.IsPlayerTurn = false;
        HandManager.Instance.OnTurnEnd();
        yield return new WaitForSeconds(0.8f);
        EnemyCharacter.Instance.OnTurnStart();
        yield return new WaitForSeconds(1.4f);
        //EnemyCharacter.Instance.OnTurnEnd();
        PlayerCharacter.Instance.OnTurnStart();
        HandManager.Instance.OnTurnStart();
        ActionQueue.Instance.IsPlayerTurn = true;
    }
}
