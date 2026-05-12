using System.Collections.Generic;
using UnityEngine;

public class IconDictionary : Singleton<IconDictionary>
{
    [Header("Statuses")]
    [SerializeField] private Sprite Strenght;
    [SerializeField] private Sprite Vulnerable;
    [SerializeField] private Sprite Weak;

    [Header("Intents")]
    [SerializeField] private Sprite Attack;
    [SerializeField] private Sprite Block;
    [SerializeField] private Sprite Debuff;
    [SerializeField] private Sprite Buff;
    [SerializeField] private Sprite Spawn;

    private Dictionary<StatusType, Sprite> _statusSprites;
    private Dictionary<IntentActionType, Sprite> _intentSprites;

    protected override void Awake()
    {
        base.Awake();

        _statusSprites = new Dictionary<StatusType, Sprite>() 
        {
            [StatusType.Strenght] = Strenght,
            [StatusType.Vulnerable] = Vulnerable,
            [StatusType.Weak] = Weak,
        };

        _intentSprites = new Dictionary<IntentActionType, Sprite>() 
        {
            [IntentActionType.Attack] = Attack,
            [IntentActionType.Block] = Block,
            [IntentActionType.Debuff] = Debuff,
            [IntentActionType.Buff] = Buff,
            [IntentActionType.Spawn] = Spawn,
        };

    }

    public Sprite GetIcon(StatusType status)
    {
        return _statusSprites[status];
    }

    public Sprite GetIcon(IntentActionType intent)
    {
        return _intentSprites[intent];
    }
}
