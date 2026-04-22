using UnityEngine;

public class BackgroundLoader : MonoBehaviour
{
    public void Initialize()
    {
        GetComponent<SpriteRenderer>().sprite = CurrentLevelInfo.Instance.CurrentLevelInfoSO.BackgroundSprite;
    }
}
