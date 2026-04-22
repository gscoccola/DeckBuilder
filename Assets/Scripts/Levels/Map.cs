using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    [SerializeField] private LevelInfoSO[] _levels;
    [SerializeField] private SpriteRenderer[] _levelCrosses;
    [SerializeField] private Button[] _levelButtons;


    private void Awake()
    {
        for (int i = 0; i < _levels.Length; i++) 
        {
            int index = i;
            _levelButtons[index].onClick.AddListener(() => LoadLevel(index));
            CreatePlayerPrefs(index);
            SetCrossedLevels(index);
            CrossLevel(index);
        }
        SFXManager.Instance.PlaySFX(SFXManager.Instance.MapPage, true);
    }

    private void SetCrossedLevels(int index)
    {
        if (PlayerPrefs.GetInt("CrossedLevel" + index) == 1) _levelCrosses[index].enabled = true;
    }

    private void CrossLevel(int index)
    {
        if (PlayerPrefs.GetInt("CompletedLevel" + index) == 1 && PlayerPrefs.GetInt("CrossedLevel" + index) == 0) StartCoroutine(ICrossCoroutine(index));
    }

    private IEnumerator ICrossCoroutine(int index)
    {
        PlayerPrefs.SetInt("CrossedLevel" + index, 1);
        _levelCrosses[index].enabled = true;
        _levelCrosses[index].color = new Color(1, 0, 0, 0);
        SFXManager.Instance.PlaySFX(SFXManager.Instance.MapScribble, false);
        float elapsedTime = 0f;
        
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / 1f);
            _levelCrosses[index].color = new Color(1, 0, 0, alpha);
            yield return null;
        }

    }

    private void LoadLevel(int index)
    {
        SFXManager.Instance.PlaySFX(SFXManager.Instance.MapPage, true);
        CurrentLevelInfo.Instance.CurrentLevelInfoSO = _levels[index];
        CurrentLevelInfo.Instance.CurrentLevelIndex = index;
        LevelLoader.Instance.LoadLevel("CombatScene");
    }

    private void CreatePlayerPrefs(int index)
    {
        if (!PlayerPrefs.HasKey("CompletedLevel" + index)) PlayerPrefs.SetInt("CompletedLevel" + index, 0);
        if (!PlayerPrefs.HasKey("CrossedLevel" + index)) PlayerPrefs.SetInt("CrossedLevel" + index, 0);
    }

    /*void Start()
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in renderers)
        {
            sr.color = new Color(0, 0, 0, 1);
        }
        currentLevelIndex = PlayerPrefs.GetInt("LevelProgress", 0);
        UpdateMapVisuals();
    }

    private void UpdateMapVisuals()
    {
        for (int i = 0; i < levelNodes.Length; i++)
        {
            if (i < currentLevelIndex)
            {
                levelNodes[i].color = new Color(1, 1, 1, 1);
            }
            else
            {
                levelNodes[i].color = new Color(1, 1, 1, 0);
            }

        }

        int arrowIndex = Mathf.Clamp(currentLevelIndex, 0, levelNodes.Length - 1);

        Vector3 newArrowPos = arrow.position;
        newArrowPos.x = levelNodes[arrowIndex].transform.position.x;
        newArrowPos.y = levelNodes[arrowIndex].transform.position.y - 1f;
        arrow.position = newArrowPos;
    }*/

    /*public void LoadCurrentLevel()
    {
        if (currentLevelIndex < Levels.Length)
        {
            CurrentLevelInfo.Instance.CurrentLevelInfoSO = Levels[currentLevelIndex];
            CurrentLevelInfo.Instance.CurrentLevelIndex = currentLevelIndex;
            LevelLoader.Instance.LoadLevel("CombatScene");
        }
        else
        {
            Debug.Log("All levels complete!");
        }
    }*/

    [ContextMenu("Reset Progress")]
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
