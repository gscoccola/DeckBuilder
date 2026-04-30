using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _continueRunPanel;
    [SerializeField] private GameObject _newRunPanel;
    [SerializeField] private GameObject _creditsPanel;

    private void Start()
    {
        _startPanel.SetActive(true);
        _creditsPanel.SetActive(false);

        if (PersistentInfo.Instance.RunInProgress)
        {
            _continueRunPanel.SetActive(true);
            _newRunPanel.SetActive(false);
        }
        else
        {
            _continueRunPanel.SetActive(false);
            _newRunPanel.SetActive(true);
        }
    }

    public void ToggleCredits()
    {
        _creditsPanel.SetActive(!_creditsPanel.activeSelf);
        _startPanel.SetActive(!_creditsPanel.activeSelf);
    }

    public void StartCasualRun()
    {
        PersistentInfo.Instance.StartNewRun(GameType.Casual);
        StartRun();
    }

    public void StartHardcoreRun()
    {
        PersistentInfo.Instance.StartNewRun(GameType.Hardcore);
        StartRun();
    }

    public void StartRun()
    {
        LevelLoader.Instance.LoadLevel("Map");
    }

    public void AbandonRun()
    {
        PersistentInfo.Instance.EndRun();
        _continueRunPanel.SetActive(false);
        _newRunPanel.SetActive(true);
    }
}
