using System;

[Serializable]
public class EnemyIntent
{
    public int DMGAmount;
    public int BlockAmount;
    public int STRGain;
    public EntityStatsSO EnemySpawned;
    public EnemyIntent(int dmgAmount, int blockAmount, int strGain, EntityStatsSO enemySpawned)
    {
        DMGAmount = dmgAmount;
        BlockAmount = blockAmount;
        STRGain = strGain;
        EnemySpawned = enemySpawned;
    }
}


public enum IntentActionType
{
    Attack,
    Block,
    Buff,
    Debuff,
    Spawn,
}
