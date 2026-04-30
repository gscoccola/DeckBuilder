using System;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class UpgradeScreen : Singleton<UpgradeScreen>
{
    [SerializeField] private CardDisplay[] _displays;
    [SerializeField] private TextMeshProUGUI[] _displaysText;
    [HideInInspector] public UnityEvent OnRewardChosen = new();

    public void DisplayRewards(Reward[] rewards, int levelIndex)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        for (int i = 0; i < rewards.Length; i++)
        {
            CardSO cardToShow = rewards[i].Type == RewardType.Remove 
                ? rewards[i].RemovedCard : rewards[i].AddedCard;

            _displays[i].Assign(new CardInstance(cardToShow), DisplayType.CardChoice, rewards[i].Type.ToString());
            int index = i;
            _displays[i].Draggable.OnRewardClicked.AddListener(() => ChooseCard(index, levelIndex));
        }
    }

    public void ChooseCard(int rewardIndex, int levelIndex)
    {

        SFXManager.Instance.PlaySFX(SFXManager.Instance.UIClick, false, 1.3f);
        PersistentInfo.Instance.OnRewardChosen(levelIndex, rewardIndex);

        for (int i = 0; i < _displays.Length; i++)
        {
            Destroy(_displays[i]);
            _displays[i].Draggable.OnRewardClicked.RemoveAllListeners();
        }

        transform.GetChild(0).gameObject.SetActive(false);
        OnRewardChosen?.Invoke();
    }
}
