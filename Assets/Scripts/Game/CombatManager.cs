using UnityEngine;

public class CombatManager : Singleton<CombatManager>
{
    [SerializeField] private PlayerCharacter _playerCharacter;
    [SerializeField] private EnemyCharacter _enemyCharacter;
    [SerializeField] private BackgroundLoader _background;
    [SerializeField] private CombatDeckManager _deck;
    [SerializeField] private HandManager _hand;

    private void Start()
    {
        SetupCharacters();
        StartCombat();
    }

    private void SetupCharacters()
    {
        _playerCharacter.Initialize();
        EnemyManager.Instance.Initialize(PersistentInfo.Instance.CurrentLevelSO.EnemyStats, PersistentInfo.Instance.CurrentLevelSO.EnemyIntentStart);
        _background.Initialize();
    }

    private void SetupDeck()
    {
        _deck.Initialize();
    }

    public void StartCombat()
    {
        SetupDeck();
        _deck.OnCombatStart();
        _hand.OnCombatStart();
         StartCoroutine(TurnManager.Instance.StartGame());
    }

    public void EndCombat(bool hasWon)
    {
        if (hasWon)
        {
            UpgradeScreen.Instance.DisplayRewards(PersistentInfo.Instance.CurrentLevelSO.Rewards, PersistentInfo.Instance.CurrentLevelIndex);
            UpgradeScreen.Instance.OnRewardChosen.AddListener(() => LoadMap());
            PersistentInfo.Instance.OnLevelWon();
        }
        else LoadMap();
    }

    private void LoadMap()
    {
        LevelLoader.Instance.LoadLevel("Map");
    }
}
