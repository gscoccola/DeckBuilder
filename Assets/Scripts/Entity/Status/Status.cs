using UnityEngine;

public class Status
{
    public StatusType Type;
    public int Amount;

    public Status(StatusType type, int amount)
    {
        Type = type;
        Amount = amount;
    }
}

public enum StatusType
{
    Strenght,
    Vulnerable,
    Weak,
}