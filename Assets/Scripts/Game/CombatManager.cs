using Unity.VisualScripting;
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
        
    }

    private void SetupCharacters()
    {
        _playerCharacter.Initialize();
        _enemyCharacter.Initialize();
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
            PlayerPrefs.SetInt("CompletedLevel" + CurrentLevelInfo.Instance.CurrentLevelIndex, 1);
        }
        LevelLoader.Instance.LoadLevel("Map");
    }
}
