using UnityEngine;

[CreateAssetMenu(fileName = "LevelInfo", menuName = "ScriptableObjects/LevelInfo")]
public class LevelInfoSO : ScriptableObject
{
    public Sprite BackgroundSprite;
    public RuntimeAnimatorController EnemyAnimator;
    public EntityStatsSO EnemyStats;
}
