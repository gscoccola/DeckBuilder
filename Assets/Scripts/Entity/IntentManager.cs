using UnityEngine;
using System.Collections.Generic;

public class IntentManager : MonoBehaviour
{
    private List<IntentDisplay> _displayPool = new();
    private List<bool> _isUsed = new();
    [SerializeField] private int _poolCount;
    [SerializeField] private GameObject _displayPrefab;
    [SerializeField] private float _width;

    [SerializeField] private Sprite _attackIcon;
    [SerializeField] private Sprite _blockIcon;

    private void Awake()
    {
        for (int i = 0; i < _poolCount; i++)
        {
            IntentDisplay display = Instantiate(_displayPrefab.GetComponent<IntentDisplay>(), transform);
            _displayPool.Add(display);
            _isUsed.Add(false);
        }
    }

    public void DisplayIntent(EnemyIntent intent)
    {
        _isUsed[0] = intent.DMGAmount > 0;
        _isUsed[1] = intent.BlockAmount > 0;

        //List<bool> activeDisplays = new List<bool>() { _isUsed[0], _isUsed[1] };
        int activeIndex = 0;
        _displayPool[0].Assign(_attackIcon, intent.DMGAmount, AssignedPos(activeIndex, _isUsed), _isUsed[0]);
        activeIndex += _isUsed[0].ToInt();
        _displayPool[1].Assign(_blockIcon, intent.BlockAmount, AssignedPos(activeIndex, _isUsed), _isUsed[1]); 
    }

    public void ClearIntents()
    {
        EnemyIntent intent = new EnemyIntent(0, 0, 0);
        DisplayIntent(intent);
    }

    private Vector2 AssignedPos(int index, List<bool> activeElements)
    {
        Vector2 pos = Vector2.zero;
        int activeCount = 0;
        foreach (bool isActive in _isUsed)
        {
            if (isActive) activeCount++;
        }
        float centerDist = (index + (1 - activeCount) / 2f);
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