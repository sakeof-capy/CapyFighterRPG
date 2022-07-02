using System.Collections.Generic;
using System;
using UnityEngine;

public class AchievementDepictor : MonoBehaviour
{
    [SerializeField] private GameObject _achievementPrefab;

    private RectTransform _canvasTransform;
    private const int _NumOfRowsAtPage = 3;
    private Vector3[] _positions;
    private List<Achievement> _achievements;
    private List<GameObject> _depictedAchievements;
    private int _numberOfPages;
    private int _currentPage;

    private void Awake()
    {
        _canvasTransform = GetComponent<RectTransform>();
        _positions = new Vector3[_NumOfRowsAtPage];
        _currentPage = -1;
        _depictedAchievements = new List<GameObject>();
        EvaluatePositions();
        LoadAchievements();
        SetNumberOfPages();
        NextPage();
    }

    private void OnEnable()
    {
        AssignPredicateBlocksToButtons();
    }

    public void NextPage()
    {
        if (HasNextPage())
        {
            ++_currentPage;
            DestroyAllDepictedAchievements();
            SpawnAchievementsAtCurrentPage();
        }
        else
        {
            throw new InvalidOperationException("No next page present");
        }
    }

    public void PreviousPage()
    {
        if (HasPreviousPage())
        {
            --_currentPage;
            DestroyAllDepictedAchievements();
            SpawnAchievementsAtCurrentPage();
        }
        else
        {
            throw new InvalidOperationException("No next page present");
        }
    }

    public bool HasNextPage() => _currentPage + 1 < _numberOfPages;

    public bool HasPreviousPage() => _currentPage - 1 >= 0;

    private void EvaluatePositions()
    {
        var totalHeight = _canvasTransform.rect.height;
        var rowHeight = totalHeight / _NumOfRowsAtPage;
        var halfRowHeight = rowHeight / 2;
        var firstRowTop = totalHeight / 2;

        for (int i = 0; i < _NumOfRowsAtPage; i++)
        {
            _positions[i] = new Vector3(0f, firstRowTop - halfRowHeight - i * rowHeight, 1f);
        }
    }

    private void SpawnAchievementsAtCurrentPage()
    {
        int startingIndex = _currentPage * _NumOfRowsAtPage;
        Debug.Assert(_depictedAchievements.Count == 0);
        for (int i = 0; i < _NumOfRowsAtPage; ++i)
        {
            if (startingIndex + i >= _achievements.Count)
                break;

            GameObject ach = Instantiate(_achievementPrefab, _canvasTransform);
            var transform = (ach.transform) as RectTransform;
            transform.anchoredPosition = _positions[i];
            _depictedAchievements.Add(ach);

            ach.GetComponent<AchievementController>().Init(_achievements[startingIndex + i]);
        }
    }

    private void DestroyAllDepictedAchievements()
    {
        foreach (var ach in _depictedAchievements)
        {
            Destroy(ach);
        }

        _depictedAchievements.Clear();
    }

    private void LoadAchievements()
    {
        GameProgressSave save = Serializer.Deserialize();
        if (save == null)
            throw new InvalidOperationException("There are no files to load from.");
        _achievements = save.Achievements;
    }

    private void SetNumberOfPages()
        => _numberOfPages = Mathf.CeilToInt((float)_achievements.Count / _NumOfRowsAtPage);

    private void AssignPredicateBlocksToButtons()
    {
        var leftButton = GameObject.FindGameObjectWithTag("LeftArrow");
        leftButton.GetComponent<DynamicButtonBlocker>().CanInteract = _ => HasPreviousPage();

        var rightButton = GameObject.FindGameObjectWithTag("RightArrow");
        rightButton.GetComponent<DynamicButtonBlocker>().CanInteract = _ => HasNextPage();
    }
}
