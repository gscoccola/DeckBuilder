using UnityEngine;
using System.Collections.Generic;

public class ActionQueue : Singleton<ActionQueue>
{

    [ReadOnly] public int DiscardingCount;
    [ReadOnly] public int DrawingCount;
    [ReadOnly] public int ExhaustingCount;
    [ReadOnly] public bool IsShuffling;
    [ReadOnly] public bool IsPlayingCard;
    [ReadOnly] public bool IsPlayerTurn;
    [ReadOnly] public bool IsLevelOver;

    public bool IsPerformingAction()
    {
        return ExhaustingCount > 0 || DiscardingCount > 0 || DrawingCount > 0 || IsShuffling || IsPlayingCard || !IsPlayerTurn || IsLevelOver;
    }
}
