using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MessageManager : Singleton<MessageManager>
{
    [SerializeField] private TextMeshProUGUI _dialogText;
    [SerializeField] private Image _dialogBox;
    public Dictionary<DialogMessage, string> Messages = new Dictionary<DialogMessage, string>();

    private void Start()
    {
        /*_dialogBox.gameObject.SetActive(false);
        Messages.Add(DialogMessage.NotEnoughEnergy, "No tengo mana suficiente");
        Messages.Add(DialogMessage.HandFull, "No tengo espacio suficiente en la mano");
        Messages.Add(DialogMessage.NoCardsLeft, "No quedan cartas para robar");
        Messages.Add(DialogMessage.EmptyPile, "Esa pila está vacía");*/
        _dialogBox.gameObject.SetActive(false);
        Messages.Add(DialogMessage.NotEnoughEnergy, "Not enough engergy");
        Messages.Add(DialogMessage.HandFull, "Hand is full");
        Messages.Add(DialogMessage.NoCardsLeft, "No cards left to draw");
        Messages.Add(DialogMessage.EmptyPile, "The pile is empty");
    }

    public void DisplayMessage(DialogMessage message)
    {
        StopAllCoroutines();
        StartCoroutine(IMessageDisplay(Messages[message]));
    }

    private IEnumerator IMessageDisplay(string message)
    {
        _dialogText.text = message;
        _dialogBox.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        _dialogBox.gameObject.SetActive(false);
    }
}

public enum DialogMessage
{
    NotEnoughEnergy,
    HandFull,
    NoCardsLeft,
    EmptyPile,
}