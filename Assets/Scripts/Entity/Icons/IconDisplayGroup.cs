using UnityEngine;
using System.Collections.Generic;

public class IconDisplayGroup : MonoBehaviour
{
    private List<IconDisplay> _displayPool = new();
    private List<bool> _isUsed = new();
    [SerializeField] private int _poolCount;
    [SerializeField] private GameObject _displayPrefab;
    [SerializeField] private float _width;

    private void Awake()
    {
        for (int i = 0; i < _poolCount; i++)
        {
            IconDisplay display = Instantiate(_displayPrefab.GetComponent<IconDisplay>(), transform);
            _displayPool.Add(display);
            _isUsed.Add(false);
        }
    }

    public void DisplayIcons(Sprite[] sprites, int[] amounts)
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            _displayPool[i].Assign(sprites[i], amounts[i], AssignedPos(i, sprites.Length), true);
        }

        for (int i = sprites.Length; i < _poolCount; i++)
        {
            _displayPool[i].Assign(null, -1, AssignedPos(i, sprites.Length), false);
        }
    }

    public void ClearDisplays()
    {
        DisplayIcons(new Sprite[0], new int[0]);
    }

    private Vector2 AssignedPos(int index, int length)
    {
        Vector2 pos = Vector2.zero;
        float centerDist = (index + (1 - length) / 2f);
        pos = _width * Vector3.right * centerDist;
        return pos;
    }
}

public static class BoolExtensions
{
    public static int ToInt(this bool value)
    {
        return value ? 1 : 0;
    }
}