using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class StatusManager : MonoBehaviour
{
    [ReadOnly] public List<Status> Statuses = new();
    [SerializeField] private IconDisplayGroup DisplayGroup;
   
    
    public void ProcessStatus(Status status)
    {
        Status previousState = Statuses.Where((stat) => stat.Type == status.Type).FirstOrDefault();
        if (previousState != null) previousState.Amount += status.Amount;
        else Statuses.Add(status);
        DisplayStatuses();
    }

    private void DisplayStatuses()
    {
        Sprite[] sprites = new Sprite[Statuses.Count];
        int[] amounts = new int[Statuses.Count];

        for (int i = 0; i < Statuses.Count; i++)
        {
            sprites[i] = IconDictionary.Instance.GetIcon(Statuses[i].Type);
            amounts[i] = Statuses[i].Amount;
        }

        DisplayGroup.DisplayIcons(sprites, amounts);
    }
}
