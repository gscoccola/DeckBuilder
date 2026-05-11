using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IconDisplay : MonoBehaviour
{
    //Components
    [SerializeField] private Image Icon;
    [SerializeField] private TextMeshProUGUI Amount;
    private CanvasGroupUpdate _canvasGroupUpdate;
    private RectTransform _rectTransform;

    //Values
    private bool _enabled;
    private Sprite _newSprite;
    private string _newText;
    private Vector2 _newPosition;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroupUpdate = GetComponent<CanvasGroupUpdate>();
        _canvasGroupUpdate.UpdateValues.AddListener(UpdateValues);
        Toggle(false);
    }

    public void Assign(Sprite sprite, int amount, Vector2 newPosition, bool enabled)
    {
        if (sprite != null) _newSprite = sprite;
        _newText = amount == 0 ? "" : amount.ToString();
        _newPosition = newPosition;
        _enabled = enabled;
        _canvasGroupUpdate.Switch();
    }

    private void UpdateValues() 
    {
        if (!_enabled || ActionQueue.Instance.IsLevelOver)
        {
            Toggle(false);
            return;
        }
        Toggle(true);
        Amount.text = _newText;
         Icon.sprite = _newSprite;
        _rectTransform.localPosition = _newPosition;
    }

    private void Toggle(bool enabled)
    {
        Icon.enabled = enabled;
        Amount.enabled = enabled;
    }
}

