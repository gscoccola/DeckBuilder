using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class Health : MonoBehaviour
{
    [HideInInspector] public EntityStatsSO Stats;
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private TextMeshProUGUI _blockText;

    [ReadOnly] public int CurrentHealth;
    [ReadOnly] public int MaxHealth;
    [ReadOnly] public int CurrentBlock;
    [HideInInspector] public UnityEvent OnDeath = new();

    private Color _color;
    private Image _hpSliderFill;

    public void Initialize(EntityStatsSO Stats)
    {
        _hpSliderFill = _hpSlider.fillRect.GetComponent<Image>();
        _color = _hpSliderFill.color;
        MaxHealth = Stats.MaxHealth;
        CurrentHealth = MaxHealth;
        CurrentBlock = Stats.InitialBlock;
        UpdateValues(false);
    }

    public void TakeDamage(int damage)
    {
        if (CurrentBlock >= damage)
        {
            CurrentBlock -= damage;
            damage = 0;
        }
        else
        {
            damage -= CurrentBlock;
            CurrentBlock = 0;
        }
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Die();
        }
        UpdateValues();
    }

    public void Heal(int amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        UpdateValues();
    }

    public void AddBlock(int amount)
    {
        CurrentBlock += amount;
        UpdateValues();
    }

    public void RemoveBlock() 
    { 
        CurrentBlock = 0;
        UpdateValues();
    }

    private void Die()
    {
        OnDeath.Invoke();
    }

    private void UpdateValues(bool animateSlider = true)
    {
        if (animateSlider) _hpSlider.DOValue((float)CurrentHealth / MaxHealth, 0.15f);
        else _hpSlider.value = (float)CurrentHealth / MaxHealth;
        _hpText.text = $"{CurrentHealth} / {MaxHealth}";
        _blockText.text = $"{CurrentBlock}";
        if (CurrentBlock > 0) _hpSliderFill.color = new Color(0f, 4f / 254, 114f / 254);
        else _hpSliderFill.color = _color;
    }
}
