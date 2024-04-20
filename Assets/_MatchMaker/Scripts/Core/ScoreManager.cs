using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private CardManager _cardManager;
    [SerializeField] private UIScoreHUD _ui;
    [SerializeField] private ScoreData _scoreData;
    private StreakHandler _streakHandler;
    private ScoreHandler _scoreHandler;
    public event Action<Score, Streak> ScoreFinalized;

    // Start is called before the first frame update
    void Start()
    {
        _streakHandler = new StreakHandler();
        _scoreHandler = new ScoreHandler();
        _cardManager.FlipEvaluated += _scoreHandler.Count;
        _cardManager.FlipEvaluated += _streakHandler.Count;
        _cardManager.FlipEvaluated += UpdateUI;
        _cardManager.AllCardsFlipped += ComputeFinalScore;
        LevelManager.LoadedLevel += Reset;
        Reset();
    }
    private void ComputeFinalScore()
    { 
        _scoreHandler.ComputeFinalScore();
        _streakHandler.ResetStreak();
        _ui.Hide();
        _scoreData.Save(_scoreHandler.Score, _streakHandler.Streak);
        ScoreFinalized?.Invoke(_scoreHandler.Score, _streakHandler.Streak);
    }
    
    private void OnDestroy()
    {
         _cardManager.FlipEvaluated -= _scoreHandler.Count;
         _cardManager.FlipEvaluated -= _streakHandler.Count;
         _cardManager.FlipEvaluated -= UpdateUI;
    }
    //TODO: potential issue here, delegates order could break this, use a update manager instead
    private void UpdateUI(bool success)
    {
        _ui.Refresh(_scoreHandler.Score, _streakHandler.Streak);
    }
    private void Reset()
    {
        _scoreHandler.Reset(); 
        _streakHandler.Reset();
        _ui.Refresh(_scoreHandler.Score, _streakHandler.Streak);
    }
}
[Serializable]
public struct Score
{
    public int correct;
    public int turns;
    public int wrong => turns - correct;
    public int totalPoints;
}
[Serializable]
public struct Streak
{
    public int streakCount;
    public int bestStreak;
}
public class ScoreHandler
{
    private Score _score;
    public Score Score => _score;
    const int MAXPOINTS = 500;
    public void Reset()
    {
        _score.turns = _score.correct = _score.totalPoints = 0;
    }
    public void Count(bool success)
    {
        if (success)
        {
          _score.correct++;
        };
        _score.turns++;
    }
    public void ComputeFinalScore()
    {
        _score.totalPoints = (int)(((float)_score.correct / _score.turns)* MAXPOINTS);
        Debug.Log(_score.totalPoints);
    }
}
public class StreakHandler
{
    public event Action Started;
    public event Action Adding;
    public event Action Ended;
    private bool _activated;
    private Streak _streak;
    public Streak Streak=> _streak;
    public void ResetStreak() 
    {
        CheckForBestStreak();
        _streak.streakCount = 0;
        _activated = false;
        Debug.Log("Reset Streak");
    }
    public void Reset()
    {
        _streak.streakCount = _streak.bestStreak = 0;
        _activated = false;
    }
    public void Count(bool success) {
        if (!success)
        { 
            ResetStreak();
            return;
        }
        _streak.streakCount++;
        if (!_activated)
        {
            if (_streak.streakCount > 1)
            {
                Started?.Invoke();
                _activated = true;
            }
        }
        if (_streak.streakCount > 2)
        { 
            Adding?.Invoke();
        }
        Debug.Log(_streak.streakCount);
    }
    private void CheckForBestStreak()
    {
        if (_streak.streakCount > _streak.bestStreak)
        {
            _streak.bestStreak = _streak.streakCount;
        }
    }
}
