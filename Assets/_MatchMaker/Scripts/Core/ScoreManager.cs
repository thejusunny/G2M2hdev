using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;


public class ScoreManager : MonoBehaviour
{
    [SerializeField] private CardManager _cardManager;
    [SerializeField] private UIScoreHUD _ui;
    private StreakHandler _streakHandler;
    private ScoreHandler _scoreHandler;

    // Start is called before the first frame update
    void Start()
    {
        _streakHandler = new StreakHandler();
        _scoreHandler = new ScoreHandler();
        _cardManager.FlipEvaluated += _scoreHandler.Count;
        _cardManager.FlipEvaluated += _streakHandler.Count;
        _cardManager.FlipEvaluated += UpdateUI;
        _scoreHandler.Reset();
        _streakHandler.Reset();
        _ui.Refresh(_scoreHandler.Score, _streakHandler.Streak);
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
}
public struct Score
{
    public int correct;
    public int turns;
    public int totalPoints;
}
public struct Streak
{
    public int streakCount;
    public int bestStreak;
}
public class ScoreHandler
{
    private Score _score;
    public Score Score => _score;
    public void Reset()
    {
        _score.turns = _score.correct = 0;
    }
    public void Count(bool success)
    {
        if (success) _score.correct++;
        _score.turns++;
    }
    public void ComputeFinalScore()
    {
        //Any bonuses or extras can be added here
        _score.totalPoints = _score.correct * 100;
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
    public void Reset() 
    {
        if (_streak.streakCount > _streak.bestStreak)
        {
            _streak.bestStreak = _streak.streakCount;
        }
        _streak.streakCount = 0;
        _activated = false;
        Debug.Log("Reset Streak");
    }
    public void Count(bool success) {
        if (!success)
        { 
            Reset();
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
}
