using UnityEngine;

[CreateAssetMenu(fileName = "LevelInfo", menuName = "ScriptableObjects/LevelInfo")]
public class LevelInfoSO : ScriptableObject
{
    public Sprite BackgroundSprite;
    public EntityStatsSO[] EnemyStats;
    public int[] EnemyIntentStart;
    public Reward[] Rewards;
}
