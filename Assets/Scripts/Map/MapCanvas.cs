using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapCanvas : MonoBehaviour
{
    [SerializeField] private FightIcon[] _fightIcons;


    private void Start()
    {
        for (int i = 0; i < PersistentInfo.Instance.Levels.Length; i++)
        {
            int index = i;

            if (PersistentInfo.Instance.CurrentLevelIndex > index)
            {
                _fightIcons[index].Set(false, true);
                _fightIcons[index].Button.onClick.AddListener(() => OpenRewardPanel(index));
            }

            if (PersistentInfo.Instance.CurrentLevelIndex == index)
            {
                _fightIcons[index].Set(true, false);
                _fightIcons[index].Button.onClick.AddListener(() => LoadLevel(index));      
            }
        }
        SFXManager.Instance.PlaySFX(SFXManager.Instance.MapPage, true);
        if (PersistentInfo.Instance.ScribblePending)
        {
            StartCoroutine(IScribble() );
            PersistentInfo.Instance.ScribblePending = false;
        }
    }

    private IEnumerator IScribble()
    {
        if (PersistentInfo.Instance.CurrentLevelIndex > 0) _fightIcons[PersistentInfo.Instance.CurrentLevelIndex - 1].SetFill();
        if (PersistentInfo.Instance.CurrentLevelIndex < _fightIcons.Length) _fightIcons[PersistentInfo.Instance.CurrentLevelIndex].SetFill();
        yield return new WaitForSeconds(0.3f);

        if (PersistentInfo.Instance.CurrentLevelIndex > 0)
        {
            _fightIcons[PersistentInfo.Instance.CurrentLevelIndex - 1].Cross();
            yield return new WaitForSeconds(0.8f);
        }

        if (PersistentInfo.Instance.CurrentLevelIndex < _fightIcons.Length) _fightIcons[PersistentInfo.Instance.CurrentLevelIndex].Circle();
    }


    private void LoadLevel(int index)
    {
        SFXManager.Instance.PlaySFX(SFXManager.Instance.MapPage, true);
        //PersistentInfo.Instance.CurrentLevelIndex = index;
        LevelLoader.Instance.LoadLevel("CombatScene");
    }

    private void OpenRewardPanel(int index)
    {
        UpgradeScreen.Instance.DisplayRewards(PersistentInfo.Instance.Levels[index].Rewards, index);
    }

    public void BackToMainMenu()
    {
        LevelLoader.Instance.LoadLevel("MainMenu");
    }

    public void DisplayDeck()
    {
        List<CardSO> deckSOs = PersistentDeckManager.Instance.GetDeck();
        List<CardInstance> deck = new();

        foreach (CardSO SO in deckSOs)
        {
            deck.Add(new CardInstance(SO));
        }

        DeckDisplay.Instance.Display(deck);
    }

}
