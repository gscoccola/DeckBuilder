using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CardDraggable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Vector3 _returnPosition;
    private float _returnRotation;
    private int _returnOrder;

    private Camera _camera;
    [ReadOnly] public CardInstance CardInstance;
    [ReadOnly] public CardDisplay AssignedDisplay;
    public DisplayType DisplayType;

    private void Awake()
    {
        _camera = FindAnyObjectByType<Camera>();
        AssignedDisplay = GetComponent<CardDisplay>();
    }

    public void SetReturnTransform(Vector3 position, float rotation, int order)
    {
        _returnPosition = position;
        _returnRotation = rotation;
        _returnOrder = order;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (DisplayType == DisplayType.CardChoice) UpgradeScreen.Instance.ChooseCard(CardInstance.CardSO);
        else if (DisplayType == DisplayType.Hand) AssignedDisplay.SetOrder(50);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (transform.localPosition.y - _returnPosition.y > 1f && CardInstance.TryToPlay(this))
        {
            transform.DOLocalMoveY(transform.localPosition.y + 1f, 0.6f);
        }
        else ReturnToHand();
    }

    public void OnFinishPlaying(bool exhaust = false)
    {
        if (exhaust) Exhaust();
        else SendToDiscard();
    }

    public void SendToDiscard()
    {
        ActionQueue.Instance.DiscardingCount++;
        transform.DOMove(CombatDeckManager.Instance.DiscardPileLocation.position, 0.3f)
            .OnComplete(() =>
            {
                CardInstance.Discard();
                ActionQueue.Instance.DiscardingCount--;
            });
    }

    private void Exhaust()
    {
        ActionQueue.Instance.ExhaustingCount++;
        AssignedDisplay.CanvasGroup.DOFade(0f, 0.3f).OnComplete(() =>
        {
            HandManager.Instance.OnExhaustFinish(CardInstance);
            AssignedDisplay.Clear();
            ActionQueue.Instance.ExhaustingCount--;
        });
    }

    private void ReturnToHand()
    {
        transform.DOLocalMove(_returnPosition, 0.2f);
        transform.DOLocalRotate(Vector3.forward * _returnRotation, 0.2f);

        AssignedDisplay.SetOrder(_returnOrder);
    }



    public void OnDrag(PointerEventData eventData)
    {
        if (DisplayType != DisplayType.Hand) return;
        transform.localEulerAngles = Vector3.zero;
        Vector3 mousePos = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.position = new Vector3(mousePos.x, Mathf.Min( mousePos.y, _returnPosition.y + 1.5f), 0f);
    }
}
