using System.Collections.Generic;
using UnityEngine;

public class IntentManager : MonoBehaviour
{
    [SerializeField] private IconDisplayGroup DisplayGroup;

    public void DisplayIntent(EnemyIntent intent)
    {
        List<Sprite> sprites = new();
        List<int> amounts = new();

        if (intent.DMGAmount > 0)
        {
            sprites.Add(IconDictionary.Instance.GetIcon(IntentActionType.Attack));
            amounts.Add(intent.DMGAmount);
        }
        if (intent.BlockAmount > 0)
        {
            sprites.Add(IconDictionary.Instance.GetIcon(IntentActionType.Block));
            amounts.Add(intent.BlockAmount);
        }
        if (intent.STRGain > 0)
        {
            sprites.Add(IconDictionary.Instance.GetIcon(IntentActionType.Buff));
            amounts.Add(intent.STRGain);
        }
        if (intent.EnemySpawned != null)
        {
            sprites.Add(IconDictionary.Instance.GetIcon(IntentActionType.Spawn));
            amounts.Add(0);
        }

        DisplayGroup.DisplayIcons(sprites.ToArray(), amounts.ToArray());
    }

    internal void ClearDisplays()
    {
        DisplayGroup.ClearDisplays();
    }
}
