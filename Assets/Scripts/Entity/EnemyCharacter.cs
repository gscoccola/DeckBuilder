using DG.Tweening;
using System.Linq;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    [HideInInspector] public Health Health;
    [HideInInspector] public StatusManager StatusManager;
    [SerializeField] private EntityStatsSO _entityStats;
    private int _intentIndex;
    [HideInInspector] public CharacterAnimator CharacterAnimator;

    private IntentManager _intentManager;

    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private SpriteRenderer _shadowSprite;
    [SerializeField] private Canvas _canvas;

    

    public void Initialize(EntityStatsSO stats, int intentIndex)
    {
        CharacterAnimator = GetComponent<CharacterAnimator>();
        _intentManager = GetComponent<IntentManager>();
        StatusManager = GetComponent<StatusManager>();
        Health = GetComponent<Health>();
        _entityStats = stats;
        CharacterAnimator.Animator.runtimeAnimatorController = stats.Animator;
        Health.Initialize(_entityStats);
        Health.OnDeath.AddListener(OnDeath);
        _intentIndex = intentIndex;
        DisplayIntent();
    }

    public void OnTurnStart()
    {

        Health.RemoveBlock();

        PerformIntent();
        DisplayIntent();
    }

    /*public void OnTurnEnd()
    {
        DisplayIntent();
    }  */ 

    private void PerformIntent()
    {
        EnemyIntent intent = _entityStats.IntentSequence[_intentIndex];
        if (intent.DMGAmount > 0)
        {
            SFXManager.Instance.PlaySFX(SFXManager.Instance.EnemyAttack, true);
        }
        else
        {
            SFXManager.Instance.PlaySFX(SFXManager.Instance.EnemyAttack, true, 0.5f);
        }

        CharacterAnimator.PlayAttackAnimation();
        if (intent.DMGAmount > 0 ) PlayerCharacter.Instance.Health.TakeDamage(intent.DMGAmount + Strenght());
        if (intent.EnemySpawned != null) EnemyManager.Instance.SpawnEnemy(intent.EnemySpawned);
        Health.AddBlock(intent.BlockAmount);

        if (intent.STRGain  > 0)
        {
            StatusManager.ProcessStatus(new Status(StatusType.Strenght, intent.STRGain));
        }
        
        _intentIndex = (_intentIndex + 1) % _entityStats.IntentSequence.Length;
    }

    private void DisplayIntent()
    {
        EnemyIntent intent = _entityStats.IntentSequence[_intentIndex];
        _intentManager.DisplayIntent(intent, Strenght());
    }

    private void OnDeath()
    {
        EnemyManager.Instance.OnDeath(this);
        
        _intentManager.ClearDisplays();
        _canvas.enabled = false;

        SFXManager.Instance.PlaySFX(SFXManager.Instance.EnemyDeath, false, 0.5f);
        _sprite.DOFade(0, 2f);
        _shadowSprite.DOFade(0, 2f);
        _sprite.transform.DOMove(_sprite.transform.position + new Vector3(0.4f, 0.2f, 0), 2f);
        _shadowSprite.transform.DOMove(_shadowSprite.transform.position + new Vector3(0.4f, 0f, 0), 2f);
    }

    private int Strenght()
    {
        Status status = StatusManager.Statuses.Where(status => status.Type == StatusType.Strenght).SingleOrDefault();
        return status == null ? 0 : status.Amount;
    }
}

