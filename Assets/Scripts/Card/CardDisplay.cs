using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private TextMeshProUGUI _descriptionText;

    [SerializeField] private Image _spriteRenderer;
    [SerializeField] private Image _highlight;
    [SerializeField] private Canvas _canvas;
    [HideInInspector] public CardInstance CardInstance;
    [HideInInspector] public bool IsAssigned;
    [HideInInspector] public CardDraggable Draggable;

    private GameObject _display;
    private BoxCollider2D _boxCollider;
    [HideInInspector] public CanvasGroup CanvasGroup;

    private void Awake()
    {
        _display = transform.GetChild(0).gameObject;
        Draggable = GetComponent<CardDraggable>();
        _boxCollider = GetComponent<BoxCollider2D>();
        CanvasGroup = _canvas.GetComponent<CanvasGroup>();
        _boxCollider.enabled = false;
    }

    public void Assign(CardInstance cardInstance, DisplayType displayType = DisplayType.Hand)
    {
        if (displayType == DisplayType.Hand) transform.position = CombatDeckManager.Instance.DrawPileLocation.position;
        if (cardInstance == null)
        {
            Debug.LogError("Cannot assign null card instance to display.");
        }
        SetAssigned(true);
        Draggable.CardInstance = cardInstance;
        Draggable.DisplayType = displayType;
        CardInstance = cardInstance;
        _costText.text = cardInstance.Stats.Cost.ToString();
        _descriptionText.text = DescriptionGenerator.GenerateDescription(cardInstance.Stats.Effects);
        _spriteRenderer.sprite = cardInstance.Stats.Sprite;
        _highlight.enabled = cardInstance.Stats.HasHighlight;
        SetLayer(displayType);
    }

    public void UpdateDisplay()
    {
        _costText.text = CardInstance.Stats.Cost.ToString();
        _descriptionText.text = DescriptionGenerator.GenerateDescription(CardInstance.Stats.Effects);
        _spriteRenderer.sprite = CardInstance.Stats.Sprite;
        _highlight.enabled = CardInstance.Stats.HasHighlight;
    }

    public void Clear()
    {
        CardInstance = null;
        _costText.text = "";
        _descriptionText.text = "";
        _spriteRenderer.sprite = null;
        Draggable.CardInstance = null;
        _highlight.enabled = false;
        CanvasGroup.alpha = 1f;
        SetAssigned(false);
    }

    private void Update()
    {
        if (!_highlight.enabled) return;
        //_highlight.color = Color.Lerp(_highlight.color, Color.white, Time.deltaTime * 5f);
        _highlight.transform.localScale =  (1.03f + Mathf.Sin(Time.time * 5f) * 0.012f) * Vector3.one;
    }
    /*public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsAssigned) return;
        CardInstance.TryToPlay();
    }*/

    private void SetAssigned(bool assigned)
    {
        IsAssigned = assigned;
        _display.SetActive(assigned);
        _boxCollider.enabled = assigned;
    }

    public void SetOrder(int order)
    {
        _canvas.sortingOrder = order + 1;
    }

    private void SetLayer(DisplayType type)
    {
        string sortingLayerName;
        if (type == DisplayType.Hand) sortingLayerName = "Cards";
        else sortingLayerName = "CardsUI";

        _canvas.sortingLayerName = sortingLayerName;
    }
}

public enum DisplayType
{
    Hand,
    Deck,
    CardChoice
}