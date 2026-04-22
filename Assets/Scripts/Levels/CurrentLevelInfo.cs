using UnityEngine;

public class CurrentLevelInfo : PersistentSingleton<CurrentLevelInfo>
{
    public LevelInfoSO CurrentLevelInfoSO;
    [HideInInspector] public int CurrentLevelIndex; 
}
