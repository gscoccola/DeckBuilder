using UnityEngine;

public class BackgroundLoader : MonoBehaviour
{
    public void Initialize()
    {
        GetComponent<SpriteRenderer>().sprite = PersistentInfo.Instance.CurrentLevelSO.BackgroundSprite;
    }
}
