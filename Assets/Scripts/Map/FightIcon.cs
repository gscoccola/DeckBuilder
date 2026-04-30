using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FightIcon : MonoBehaviour
{
    [SerializeField] public Button Button;

    [SerializeField] private Image _circle;
    [SerializeField] private Image _cross1;
    [SerializeField] private Image _cross2;

    private void Awake()
    {
        _circle.enabled = false;
        _cross1.enabled = false;
        _cross2.enabled = false;
    }

    public void Set(bool circleEnabled, bool crossEnabled)
    {
        _circle.enabled = circleEnabled;
        _cross1.enabled = crossEnabled;
        _cross2.enabled = crossEnabled;
    }

    public void SetFill()
    {
        _circle.fillAmount = 0;
        _cross1.fillAmount = 0;
        _cross2.fillAmount = 0;
    }

    public void Cross()
    {
        _cross1.fillAmount = 0;
        _cross2.fillAmount = 0;

        SFXManager.Instance.PlaySFX(SFXManager.Instance.MapScribble, false);
        _cross1.DOFillAmount(1f, 0.35f)
            .OnComplete(()=> _cross2.DOFillAmount(1f, 0.35f));
    }

    public void Circle()
    {
        SFXManager.Instance.PlaySFX(SFXManager.Instance.MapScribble, false);
        _circle.fillAmount = 0;
        _circle.DOFillAmount(1f, 0.45f);
    }

}
