using Assets._MatchMaker.Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private SaveSystem _saveSystem;
    [SerializeField] private MatchMakerLayoutBuilder _layoutBuilder;
    [SerializeField] private UILevelEnd _uiLevelEnd;
    [SerializeField] List<MatchMakerData> _matchMakerData;
    public static event Action LoadedNewLevel;
    int _currentGame = 0;
    // Start is called before the first frame update
    void Start()
    {
        _scoreManager.ScoreFinalized += ShowLevelEnd;
        _scoreManager.ScoreFinalized += SaveLevelProgress;
        SaveProfile profile = _saveSystem.LoadProfile();
        _currentGame = profile.lastPlayedLevelIndex;
        _layoutBuilder.BuildLayout(GetCurrentLayout());
    }
    private MatchMakerData GetCurrentLayout()
    { 
        return _matchMakerData[_currentGame];
    }
    private void OnDestroy()
    {
        _scoreManager.ScoreFinalized -= ShowLevelEnd;
        _scoreManager.ScoreFinalized -= SaveLevelProgress;
    }
    private void ShowLevelEnd(Score score, Streak streak)
    {
        _uiLevelEnd.Show(score, streak);
    }
    private void SaveLevelProgress(Score score, Streak streak)
    {
        _saveSystem.SaveProfile(new SaveProfile(Mathf.Min( _currentGame+1, _matchMakerData.Count-1)));//Save progress even if you don't play next level 
    }
    public void NextLevel()
    {
        if (_currentGame >= _matchMakerData.Count-1)
        {
            return;
        }
        _currentGame = Mathf.Min(_currentGame + 1, _matchMakerData.Count);
        _layoutBuilder.BuildLayout(_matchMakerData[_currentGame]);
        _uiLevelEnd.Hide();
        LoadedNewLevel?.Invoke();
    }
}
