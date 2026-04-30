using UnityEngine;
using System.Collections.Generic;
using System;

public class PersistentInfo : PersistentSingleton<PersistentInfo>
{
    [Header("Level SOs")]
    [SerializeField] public LevelInfoSO[] Levels;

    [Header ("Debug")]
    [ReadOnly] public bool RunInProgress;
    [ReadOnly] public GameType CurrentRunType;
    [ReadOnly] public int CurrentLevelIndex;
    [ReadOnly] public LevelCompletionInfo[] LevelsCompletion;
    [ReadOnly] public List<Reward> ChosenRewards;

    [HideInInspector] public bool ScribblePending;
    public LevelInfoSO CurrentLevelSO => Levels[CurrentLevelIndex];

    private void Start()
    {
        if (PlayerPrefs.HasKey("RunInProgress")) LoadRunData();
    }

    #region Load or create Data
    private void LoadRunData()
    {
        RunInProgress = true;   
        ScribblePending = true;

        CurrentRunType = (GameType)PlayerPrefs.GetInt("CurrentRunType");
        CurrentLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex");

        SetupCollections();
        for (int i = 0; i < Levels.Length; i++)
        {
            LevelsCompletion[i].IsCompleted = PlayerPrefs.GetInt($"Level{i}Completed") == 1;
            LevelsCompletion[i].RewardChosen = PlayerPrefs.GetInt($"Level{i}WasRewardChosen") == 1;
            LevelsCompletion[i].RewardIndex = PlayerPrefs.GetInt($"Level{i}RewardChosenIndex");
            if (LevelsCompletion[i].RewardChosen) ChosenRewards.Add(Levels[i].Rewards[LevelsCompletion[i].RewardIndex]);
        }
        PersistentDeckManager.Instance.UpdateDeck();
    }

    private void CreateRunData(GameType gameType)
    {
        PlayerPrefs.SetInt("RunInProgress", 1);
        PlayerPrefs.SetInt("CurrentRunType", (int)CurrentRunType);
        PlayerPrefs.SetInt("CurrentLevelIndex", 0);

        for (int i = 0; i < Levels.Length; i++)
        {
            PlayerPrefs.SetInt($"Level{i}Completed", 0);
            PlayerPrefs.SetInt($"Level{i}WasRewardChosen", 0);
            PlayerPrefs.SetInt($"Level{i}RewardChosenIndex", 0);
        }
    }
    #endregion

    public void StartNewRun(GameType gameType)
    {
        RunInProgress = true;
        CurrentRunType = gameType;
        CurrentLevelIndex = 0;
        SetupCollections();
        ScribblePending = true;
        CreateRunData(gameType);
    }

    private void SetupCollections()
    {
        LevelsCompletion = new LevelCompletionInfo[Levels.Length];
        for (int i = 0; i < Levels.Length; i++)
        {
            LevelsCompletion[i] = new LevelCompletionInfo();
        }
        ChosenRewards = new List<Reward>();
        PersistentDeckManager.Instance.UpdateDeck();
    }

    public void EndRun()
    {
        RunInProgress = false;
        PlayerPrefs.DeleteAll();
    }

    public void OnRewardChosen(int levelIndex, int newRewardIndex)
    {
        LevelInfoSO _levelSO = Levels[levelIndex];
        if (LevelsCompletion[levelIndex].RewardChosen)
        {
            int previousRewardIndex = LevelsCompletion[levelIndex].RewardIndex;
            ChosenRewards.Remove(_levelSO.Rewards[previousRewardIndex]);
        }

        LevelsCompletion[levelIndex].RewardChosen = true;
        LevelsCompletion[levelIndex].RewardIndex = newRewardIndex;
        ChosenRewards.Add(_levelSO.Rewards[newRewardIndex]);
        PersistentDeckManager.Instance.UpdateDeck();
        PlayerPrefs.SetInt($"Level{levelIndex}WasRewardChosen", 1);
        PlayerPrefs.SetInt($"Level{levelIndex}RewardChosenIndex", newRewardIndex);
    }

    public void OnLevelWon()
    {
        LevelsCompletion[CurrentLevelIndex].IsCompleted = true;
        PlayerPrefs.SetInt($"Level{CurrentLevelIndex}Completed", 1);
        CurrentLevelIndex++;
        PlayerPrefs.SetInt($"CurrentLevelIndex", CurrentLevelIndex);
        ScribblePending = true;
    }
}


public class LevelCompletionInfo
{
    public bool IsCompleted;
    public bool RewardChosen;
    public int RewardIndex;
}

public enum GameType
{
    Casual,
    Hardcore,
}

public static class BoolExtension
{
    public static int ToInt(this bool b)
    {
        if (b) return 1;
        else return 0;
    }
}