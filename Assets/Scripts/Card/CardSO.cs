using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/Card")]
public class CardSO : ScriptableObject
{
    public string ID;
    public string Name;
    public CardStats Stats;
}


[System.Serializable]
public class CardStats
{
    public bool HasHighlight;

    public int Cost;
    public Sprite Sprite;
    public CardEffect[] Effects;

    public CardStats(CardStats stats)
    {
        HasHighlight = stats.HasHighlight;
        Cost = stats.Cost;
        Sprite = stats.Sprite;
        Effects = new CardEffect[stats.Effects.Length];
        for (int i = 0; i < stats.Effects.Length; i++)
        {
            Effects[i] = new CardEffect(stats.Effects[i].Type, stats.Effects[i].Amount);
        }
    }
}

