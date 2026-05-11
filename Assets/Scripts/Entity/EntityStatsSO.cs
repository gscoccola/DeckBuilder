using UnityEngine;

[CreateAssetMenu(fileName = "EntityStats", menuName = "ScriptableObjects/EntityStats")]
public class EntityStatsSO : ScriptableObject
{
    public int MaxHealth;
    public int InitialBlock;
    public EnemyIntent[] IntentSequence;
    public RuntimeAnimatorController Animator;
}
